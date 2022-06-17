using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoodMe;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Collections;
using Unity.Jobs;
using System.IO;

using UnityEngine.UI;

public class EmotionsManager : MonoBehaviour
{
    //public MeshRenderer PreviewMR;
    [Header("ENTER LICENSE HERE")]
    public string Email = "";
    public string AndroidLicense = "";
    public string IosLicense = "";
    public string OsxLicense = "";
    public string WindowsLicense = "";

    [Header("Input")]
    public CameraManager VideoInput;
    [Header("Performance")]
    [Range(1, 60)]
    public int ProcessEveryNFrames = 15;
    [Header("Processing")]
    public bool FilterAllZeros = true;
    [Range(0, 29f)]
    public int Smoothing;
    [Header("Emotions")]
    public bool TestMode = false;
    [Range(0, 1f)]
    public float Angry;
    [Range(0, 1f)]
    public float Disgust;
    [Range(0, 1f)]
    public float Happy;
    [Range(0, 1f)]
    public float Neutral;
    [Range(0, 1f)]
    public float Sad;
    [Range(0, 1f)]
    public float Scared;
    [Range(0, 1f)]
    public float Surprised;

    public float Seconds = 4f;
    public float Halftime = 2.5f;

    [SerializeField] private GameObject HappyText;
    [SerializeField] private GameObject HalfTimeText;
    [SerializeField] private GameObject AngryText;
    [SerializeField] private GameObject SurprisedText;
    [SerializeField] private GameObject SadText;

    [SerializeField] private GameObject _MoodMe;
    [SerializeField] private GameObject _MoodMeUI;

    [SerializeField] private GameObject[] UIList; //een lijst voor ui die uitgezet moet worden tijdens de emotiechecker

    //private PopupController _currentPopup;

    public static float EmotionIndex;

    public static MoodMeEmotions.MDMEmotions Emotions;
    private static MoodMeEmotions.MDMEmotions CurrentEmotions;


    //Main buffer texture
    public static WebCamTexture CameraTexture;

    private EmotionsInterface _emotionNN;


    //Main buffer

    private const int _width = 640, _height = 480;
    private byte[] _buffer;
    private bool _bufferProcessed = false;
    private bool _ThreadStarted = false;

    ManualResetEvent _suspendEvent;


    private Color[] clrs;

    private Thread jobThread;
    IntPtr byteBuffer = Marshal.AllocHGlobal(_width * _height * 3);

    private int NFramePassed;

    private static DateTime timestamp;


    public Text t;


    // Start is called before the first frame update
    void Start()
    {

        //RGB byte buffer creation
#if (UNITY_IOS && !UNITY_EDITOR) || (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX)
        _buffer = new byte[_width * _height * 4];
#elif (UNITY_ANDROID && !UNITY_EDITOR)
        _buffer = new byte[(_width) * (_height) * 3];
#else
        _buffer = new byte[_width * _height * 3];
#endif




        //Engine init

        string EnvKey = "";
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        EnvKey = OsxLicense;
#elif UNITY_IOS && !UNITY_EDITOR
        EnvKey = IosLicense;
#elif UNITY_ANDROID && !UNITY_EDITOR
        EnvKey = AndroidLicense;
#else
        EnvKey = WindowsLicense;
#endif
        _emotionNN = new EmotionsInterface();



        int remainingDays = _emotionNN.SetLicense(Email==""?null:Email, EnvKey==""?null:EnvKey);

        if (remainingDays == -1)
        {
            Debug.Log("INVALID OR EMPTY LICENSE. The SDK will run in demo mode.");
            remainingDays = _emotionNN.SetLicense(null, EnvKey);
        }

        if (remainingDays<0x7ff)
        {
            Debug.Log("Remaining " + remainingDays + " days");
            if(remainingDays==0)
            {
                Debug.Log("LICENSE EXPIRED. Please contact sales@mood-me.com to extend the license.");
            }            
        }
        else
        {
            Debug.Log("Lifetime license!");
        }


        ParameterizedThreadStart pts = new ParameterizedThreadStart(ThreadProcessFrame);
        jobThread = new System.Threading.Thread(pts);

        jobThread.Priority = System.Threading.ThreadPriority.BelowNormal; // Lowest, BelowNormal, Normal, AboveNormal, Highest
        jobThread.IsBackground = true;
        _suspendEvent = new ManualResetEvent(true);





    }

    void OnDestroy()
    {
        _emotionNN = null;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        //If a Render Texture is provided in the VideoTexture (or just a still image), Webcam image will be ignored

        if (!TestMode)
        {
            if (CameraManager.WebcamReady)
            {

                NFramePassed = (NFramePassed + 1) % ProcessEveryNFrames;
                if (NFramePassed == 0)
                {
                    //Allocate memory and rearrange buffer for the engine
                    GCHandle gch = GCHandle.Alloc(_buffer, GCHandleType.Pinned);
                    IntPtr buff = gch.AddrOfPinnedObject();
                    //Prepare buffer 
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
                    int square = _width * _height;
                    for (int i = 0; i < square; i++)
                    {
                        _buffer[i * 3 + 0] = VideoInput.GetPixels[square - i - 1].r;
                        _buffer[i * 3 + 1] = VideoInput.GetPixels[square - i - 1].g;
                        _buffer[i * 3 + 2] = VideoInput.GetPixels[square - i - 1].b;
                    }
                    
                    //Call the engine

                    try
                    {
                        if (jobThread.ThreadState.Equals(ThreadState.Unstarted | ThreadState.Background))
                        {

                            jobThread.Start(buff);
                            _ThreadStarted = true;
                        }
                        else
                        {
                            _suspendEvent.Set();
                        }


                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        _bufferProcessed = false;
                    }

#elif (UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN
                int square = _width * _height;
                for (int i = 0; i < square; i++)
                {
                    _buffer[i * 4 + 0] = VideoInput.GetPixels[i].r;
                    _buffer[i * 4 + 1] = VideoInput.GetPixels[i].g;
                    _buffer[i * 4 + 2] = VideoInput.GetPixels[i].b;
                    _buffer[i * 4 + 3] = 255;
                }
                //Call the engine
                try
                {
                    if (jobThread.ThreadState.Equals(ThreadState.Unstarted | ThreadState.Background))
                    {

                        jobThread.Start(buff);
                        _ThreadStarted = true;
                    }
                    else
                    {
                        _suspendEvent.Set();
                    }


                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    _bufferProcessed = false;
                }

#else
                int i = 0;

                    for (int x = _width - 1; x > -1; x--)
                    {
                        for (int y = _height - 1; y > -1; y--)
                        {
                            _buffer[i * 3 + 0] = VideoInput.GetPixels[x + y * _width].r;
                            _buffer[i * 3 + 1] = VideoInput.GetPixels[x + y * _width].g;
                            _buffer[i * 3 + 2] = VideoInput.GetPixels[x + y * _width].b;
                            i++;
                        }
                    }
                    _bufferProcessed = _emotionNN.ProcessFrame((IntPtr)buff, _height, _width, 0);



#endif


                    if (_bufferProcessed)
                    {
                        _bufferProcessed = false;
                        _suspendEvent.Reset();
                        //_ThreadStarted = false;
                        if (!(_emotionNN.DetectedEmotions.AllZero && FilterAllZeros))
                        {
                            CurrentEmotions = _emotionNN.DetectedEmotions;
                            Emotions = Filter(Emotions, CurrentEmotions, Smoothing);
                            //Debug.Log("angry " + Emotions.angry);
                            //Debug.Log("disgust " + Emotions.disgust);
                            //Debug.Log("happy " + Emotions.happy);
                            //Debug.Log("neutral " + Emotions.neutral);
                            //Debug.Log("sad " + Emotions.sad);
                            //Debug.Log("scared " + Emotions.scared);
                            //Debug.Log("surprised " + Emotions.surprised);
                            Angry = Emotions.angry;
                            Disgust = Emotions.disgust;
                            Happy = Emotions.happy;
                            Neutral = Emotions.neutral;
                            Sad = Emotions.sad;
                            Scared = Emotions.scared;
                            Surprised = Emotions.surprised;

                            if (Happy > 0.45f)
                            {
                                StartCoroutine(TakePhoto());
                                StartCoroutine(DeactivateEmotion());
                                StartCoroutine(ChangeText());

                                HappyText.SetActive(true);
                                AngryText.SetActive(false);
                                SadText.SetActive(false);
                                SurprisedText.SetActive(false);
                                HalfTimeText.SetActive(false);
                            }
                            else if (Angry > 0.45f)
                            {
                                StopCoroutine(DeactivateEmotion());
                                StopCoroutine(ChangeText());

                                HappyText.SetActive(false);
                                AngryText.SetActive(true);
                                SadText.SetActive(false);
                                SurprisedText.SetActive(false);
                                HalfTimeText.SetActive(false);
                            }
                            else if (Sad > 0.45f)
                            {
                                StopCoroutine(DeactivateEmotion());
                                StopCoroutine(ChangeText());

                                HappyText.SetActive(false);
                                AngryText.SetActive(false);
                                SadText.SetActive(true);
                                SurprisedText.SetActive(false);
                                HalfTimeText.SetActive(false);
                            }
                            else if (Surprised > 0.45f)
                            {
                                StopCoroutine(DeactivateEmotion());
                                StopCoroutine(ChangeText());
                                HappyText.SetActive(false);
                                AngryText.SetActive(false);
                                SadText.SetActive(false);
                                SurprisedText.SetActive(true);
                                HalfTimeText.SetActive(false);
                            }
                            else
                            {
                                StopCoroutine(DeactivateEmotion());
                                StopCoroutine(ChangeText());
                                HappyText.SetActive(false);
                                AngryText.SetActive(false);
                                SadText.SetActive(false);
                                SurprisedText.SetActive(false);
                                HalfTimeText.SetActive(false);
                            }


                            }

                    }
                    else
                    {
                        Emotions.Error = true;
                    }
                    gch.Free();

                }
            }

        }
        else
        {
            Emotions.angry = Angry;
            Emotions.disgust = Disgust;
            Emotions.happy = Happy;
            Emotions.neutral = Neutral;
            Emotions.sad = Sad;
            Emotions.scared = Scared;
            Emotions.surprised = Surprised;
        }
        EmotionIndex = (((3f * Happy + Surprised - (Sad + Scared + Disgust + Angry)) / 3f)+1f)/2f;

    }

    private void ThreadProcessFrame(System.Object buff)
    {
        while (true)
        {
            _suspendEvent.WaitOne(Timeout.Infinite);
            _bufferProcessed = _emotionNN.ProcessFrame((IntPtr)buff, _width, _height, 0);
        }
    }

    private IEnumerator AndroidProcessFrame(System.Object buff)
    {
        DateTime internalTimestamp = DateTime.Now;
        yield return _bufferProcessed = _emotionNN.ProcessFrame((IntPtr)buff, _width, _height, 0);
        Debug.Log("CALL THE ENGINE: " + DateTime.Now.Subtract(internalTimestamp).TotalMilliseconds + "ms");
    }


    // Smoothing function
    MoodMeEmotions.MDMEmotions Filter(MoodMeEmotions.MDMEmotions target, MoodMeEmotions.MDMEmotions source, int SmoothingGrade)
    {
        float targetFactor = SmoothingGrade / 30f;
        float sourceFactor = (30 - SmoothingGrade) / 30f;
        target.angry = target.angry * targetFactor + source.angry * sourceFactor;
        target.disgust = target.disgust * targetFactor + source.disgust * sourceFactor;
        target.happy = target.happy * targetFactor + source.happy * sourceFactor;
        target.neutral = target.neutral * targetFactor + source.neutral * sourceFactor;
        target.sad = target.sad * targetFactor + source.sad * sourceFactor;
        target.scared = target.scared * targetFactor + source.scared * sourceFactor;
        target.surprised = target.surprised * targetFactor + source.surprised * sourceFactor;

        return target;
    }

    IEnumerator DeactivateEmotion()
    {

        yield return new WaitForSeconds(Seconds);

        _MoodMe.SetActive(false);
        _MoodMeUI.SetActive(false);

       //_currentPopup.ShowNextPopup();

        TurnOffOrOnui(true);

    }

    IEnumerator ChangeText()
    {

        yield return new WaitForSeconds(Halftime);

        HappyText.SetActive(false);
        HalfTimeText.SetActive(true);

    }

    IEnumerator TakePhoto()
    {
        UploadPNG();
        yield return null;
    }

    void UploadPNG()  
    {
       // yield return new WaitForEndOfFrame();

        try {
        CameraTexture = CameraManager.CameraTexture;

        Texture2D photo = new Texture2D(CameraTexture.width, CameraTexture.height);
        photo.filterMode = FilterMode.Point;

        photo.SetPixels(CameraTexture.GetPixels());
        //photo.
        photo.Apply();

            Saves.SaveGameContoller.SaveImage(photo);
            //Encode to a PNG
            /*byte[] bytes = photo.EncodeToPNG();
            //Write out the PNG. Of course you have to substitute your_path for something sensible
            File.WriteAllBytes(Application.persistentDataPath + "/SavedScreen.png", bytes);*/
        }
        catch(Exception e)
        {
            //t.text = e.Message;
        }

    }

    private void TurnOffOrOnui(bool OnOrOff)
    {
        foreach (GameObject gameObject in UIList)
        {
            gameObject.SetActive(OnOrOff);
        }
    }

}

