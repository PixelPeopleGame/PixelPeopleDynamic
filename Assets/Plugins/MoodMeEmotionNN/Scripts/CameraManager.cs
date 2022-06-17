using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Video Source")]
    public int DeviceIndex = 0;
    public GameObject WebcamPlane;
    public Texture WebcamTexture;
    public RenderTexture VideoTexure;


    //Main buffer texture
    public static WebCamTexture CameraTexture;

    private static Color32[] _pixels;

    private const int _width = 640, _height = 480;

    //Webcam ready state
    static bool _webcamSet = false;

    public Color32[] GetPixels { get { return _pixels; } }

    // Start is called before the first frame update
    void Start()
    {
        //Webcam select
        try
        {
            for (int cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++)
            {
                Debug.Log(cameraIndex + " name " + WebCamTexture.devices[cameraIndex].name + " isFrontFacing " + WebCamTexture.devices[cameraIndex].isFrontFacing);
            }
            DeviceIndex = (int)Mathf.Clamp(DeviceIndex, 0, WebCamTexture.devices.Length);
            if (DeviceIndex > WebCamTexture.devices.Length - 1)
            {
                DeviceIndex = WebCamTexture.devices.Length - 1;
            }
            if (WebCamTexture.devices[DeviceIndex].name.Contains("Kinect")) DeviceIndex = 0;
            string camName = WebCamTexture.devices[DeviceIndex].name;
            CameraTexture = new WebCamTexture(camName, _width, _height, 60);
        }
        catch (Exception)
        {

            Debug.Log("Camera not ready");
        }

        //Webcam start
        if (VideoTexure == null)
        {
            CameraTexture.Play();
            StartCoroutine(WaitForWebCamAndInitialize(CameraTexture));
            Debug.Log("Camera Texture size " + CameraTexture.width + " x " + CameraTexture.height);

        }
        //RGBA buffer creation
        _pixels = new Color32[CameraTexture.width * CameraTexture.height];

    }

    private void OnDisable()
    {
        CameraTexture.Stop();
    } 

    private void OnEnable()
    {
        CameraTexture.Play();
    } 

    public static bool WebcamReady
    {
        get
        {
            return _webcamSet;
        }
    }
    


    private IEnumerator WaitForWebCamAndInitialize(WebCamTexture _webCamTexture)
    {
        while (_webCamTexture.width < 100)
            yield return null;
        Debug.Log("******** Camera Texture size now is " + CameraTexture.width + " x " + CameraTexture.height);        
        _pixels= new Color32[CameraTexture.width * CameraTexture.height];
        _webcamSet = true;

    }


    // Update is called once per frame
    void Update()
    {
        if (VideoTexure != null)
        {
            RenderTexture.active = VideoTexure;
            ((Texture2D)WebcamTexture).ReadPixels(new Rect(0, 0, _width, _height), 0, 0, false);
            ((Texture2D)WebcamTexture).Apply();
            RenderTexture.active = null;
            _pixels = ((Texture2D)WebcamTexture).GetPixels32();
        }
        else
        {
            if (WebcamReady)
            {
                //Get the next frame from the webcam
                bool waitForCamera = true;
                while (waitForCamera)
                {
                    try
                    {
                        //Store to RGBA buffer
                        CameraTexture.GetPixels32(_pixels);
                        //Update the Webcam plane texture
                        ((Texture2D)WebcamTexture).SetPixels32(_pixels);
                        ((Texture2D)WebcamTexture).Apply();
                        waitForCamera = false;
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("Camera not running " + ex.Message);
                    }
                }
            }
        }

    }
}
