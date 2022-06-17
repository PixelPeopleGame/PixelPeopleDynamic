using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using AOT;

namespace MoodMe
{
    public class EmotionsInterface
    {
        private IntPtr handler;
        private static MoodMeEmotions.MDMEmotions _detectedemotions;
        private unsafe float* buff;
        private DateTime timestamp;




#if UNITY_ANDROID && !UNITY_EDITOR
        //For future purpose
        //string directory = Application.persistentDataPath + "/data";           
        //ExtractAndroidAssets(directory);

        private static AndroidJavaClass jEmotionClass;
        private static AndroidJavaObject jEmotionObject;
        private static AndroidJavaObject activityContext;
        float[] jbuff;

        public EmotionsInterface()
        {
            Init();
        }

        public void Init()
        {
            //Debug.Log("emotionnn - START");
            if (jEmotionObject == null)
            {
                using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                    //Debug.Log("emotionnn - Got activityContext");
                }

            }

            CreateInstance("");
            //Debug.Log("END");
        }

        public void CreateInstance(string dataDir)
        {
            try
            {
                jEmotionObject = new AndroidJavaObject("com.moodme.emotionsdk.MDMEmotionsDetector", activityContext);
                //jEmotionObject = jEmotionClass.Call<AndroidJavaObject>("MDMEmotionsDetector", activityContext, "");
                //Debug.Log("emotionnn - JEmotionObject not null " + jEmotionObject!=null);
            }
            catch (Exception ex)
            {
                Debug.Log("emotionnn - ERROR " + ex.Message);
                Debug.Log("emotionnn - ERROR " + ex.StackTrace.ToString());

            }

        }

        public unsafe bool ProcessFrame(IntPtr buffer, int width, int height, int faceid)
        {
            bool res = false;
            object[] argumentList;
            argumentList = new object[3];

            int size = width * height * 3;
            byte[] managedArray = new byte[size];
            //timestamp = DateTime.Now;

            Marshal.Copy(buffer, managedArray, 0, size);
            //Debug.Log("MARSHAL COPY: " + DateTime.Now.Subtract(timestamp).TotalMilliseconds + "ms");
            //timestamp = DateTime.Now;
            
            //argumentList[0] = AndroidJNIHelper.ConvertToJNIArray(managedArray); 
            //Debug.Log("JNI CONVERSION: " + DateTime.Now.Subtract(timestamp).TotalMilliseconds + "ms");

            argumentList[0] = managedArray;             
            //timestamp = DateTime.Now;
            argumentList[1] = width;
            argumentList[2] = height;
            //argumentList[1].c = Char.Parse(" ");
            try
            {
                //Debug.Log("emotionnn - " + "-------------width " + width);
                //Debug.Log("emotionnn - " + "-------------height " + height);
                //Debug.Log("emotionnn - " + "-------------jEmotionObject is " + ((jEmotionObject==null)?"null":"not null"));
                //Debug.Log("emotionnn - " + "-------------jbuff "+ jbuff);
                //Debug.Log("emotionnn - " + "-------------buffer " + buffer);
                //Debug.Log("emotionnn - " + "-------------argumentList " + argumentList);
                
                //timestamp = DateTime.Now;

                jbuff = jEmotionObject.Call<float[]>("processEntireFrame", argumentList);
                //Debug.Log("CALL JAVA ANDROID: " + DateTime.Now.Subtract(timestamp).TotalMilliseconds + "ms");
                //Debug.Log("emotionnn - " + "SIZE: " + (jbuff == null));
            }
            catch (Exception ex)
            {
                Debug.Log("emotionnn - " + ex.Message);
                Debug.Log("emotionnn - " + ex.StackTrace.ToString());
            }

            
            res = (jbuff!=null)?(jbuff.Length >= 7):false;
            //if (res)
            //{
            //    Debug.Log("emotionnn - " + "angry " + jbuff[5]);
            //    Debug.Log("emotionnn - " + "disgust " + jbuff[2]);
            //    Debug.Log("emotionnn - " + "happy " + jbuff[3]);
            //    Debug.Log("emotionnn - " + "neutral " + jbuff[6]);
            //    Debug.Log("emotionnn - " + "sad " + jbuff[4]);
            //    Debug.Log("emotionnn - " + "scared " + jbuff[1]);
            //    Debug.Log("emotionnn - " + "surprised " + jbuff[0]);
            //}
            return res;
        }


     

        public unsafe MoodMeEmotions.MDMEmotions DetectedEmotions
        {
            get
            {
                if (jbuff != null)
                {
                    _detectedemotions = new MoodMeEmotions.MDMEmotions()
                    {
                        surprised = (float)jbuff[0],
                        scared = (float)jbuff[1],
                        disgust = (float)jbuff[2],
                        happy = (float)jbuff[3],
                        sad = (float)jbuff[4],
                        angry = (float)jbuff[5],
                        neutral = (float)jbuff[6],
                        latency = 0,
                        latency_avg = 0,
                        AllZero = (jbuff[0] + jbuff[1] + jbuff[2] + jbuff[3] + jbuff[4] + jbuff[5] + jbuff[6]) == 0,
                        Error = false
                    };

                }
                return _detectedemotions;
            }
        }

        private static string GetLastTrackerError()
        {
            string s = "X";

            //s = Marshal.PtrToStringAnsi(MoodMeEmotions.GetLastTrackerError());

            return (s);
        }

        public int SetLicense(string email, string key)
        {
            //Debug.Log("emotionnn - Call SetLicense");
            int res = -1;
            object[] argumentList;
            argumentList = new object[2];
            argumentList[0] = email;
            argumentList[1] = key;
            res = jEmotionObject.Call<int>("setLicense", argumentList);
            //Debug.Log("emotionnn - " + "Days "+res);
            return res;
        }

#elif UNITY_IOS && !UNITY_EDITOR

        static bool waitForCallback;

        public unsafe EmotionsInterface()
        {
            //string directory = Application.dataPath + "/StreamingAssets/data";


            try
            {

                handler = MoodMeEmotions.CreateInstance();
                if (handler == IntPtr.Zero)
                {
                    Debug.Log("Emotions STATUS: ERROR");
                }
                else
                {
                    MoodMeEmotions.SubscribeCallBack(ResultsCallback);
                    waitForCallback=false;
                }

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        [MonoPInvokeCallback(typeof(MoodMeEmotions.EmotionsCallback))]
        private unsafe static void ResultsCallback(float* buff)
        {
            if (buff != null)
            {
                _detectedemotions = new MoodMeEmotions.MDMEmotions()
                {
                    surprised = buff[0],
                    scared = buff[1],
                    disgust = buff[2],
                    happy = buff[3],
                    sad = buff[4],
                    angry = buff[5],
                    neutral = buff[6],
                    latency = 0,
                    latency_avg = 0,
                    AllZero = (buff[0] + buff[1] + buff[2] + buff[3] + buff[4] + buff[5] + buff[6]) == 0,
                    Error = false

                };
                Debug.Log("surprised " + buff[0]);
                Debug.Log("scared " + buff[1]);
                Debug.Log("disgust " + buff[2]);
                Debug.Log("happy " + buff[3]);
                Debug.Log("sad " + buff[4]);
                Debug.Log("angry " + buff[5]);
                Debug.Log("neutral " + buff[6]);
                waitForCallback = false;
            }
        }


        public unsafe bool ProcessFrame(IntPtr buffer, int width, int height, int faceid)
        {
            bool res = false;
            if (waitForCallback) return true;

            res = MoodMeEmotions.ProcessImageBuffer(handler, buffer, width, height, width * 4, MoodMeEmotions.IOS_ROTATION.RIGHT);
            waitForCallback = false;
            //res = true;
            //if (res)
            //{
            //    Debug.Log("angry " + buff[0]);
            //    Debug.Log("disgust " + buff[1]);
            //    Debug.Log("happy " + buff[2]);
            //    Debug.Log("neutral " + buff[3]);
            //    Debug.Log("sad " + buff[4]);
            //    Debug.Log("scared " + buff[5]);
            //    Debug.Log("surprised " + buff[6]);
            //}
            return res;
        }

        public unsafe Texture2D ProcessFrameWithPreview(IntPtr buffer, int width, int height, int faceid, MeshRenderer MR)
        {
            Debug.Log("CALL PROCESS FRAME WITH PREVIEW");
            Texture2D res = new Texture2D(640,480, TextureFormat.RGBA32, false);
            //if (waitForCallback) return res;
            byte* buff;
            int buffLength;
            MoodMeEmotions.ProcessImageBuffer(handler, buffer, width, height, width * 4, MoodMeEmotions.IOS_ROTATION.RIGHT);
            MoodMeEmotions.GetPreviewImageJPG(out buff, out buffLength);
            Debug.Log("PREVIEW LENGTH "+buffLength);
            byte[] JPGBuff = new byte[buffLength];
            for(int i=0; i<buffLength; i++)
            {
                JPGBuff[i] = buff[i];
            }
            if (!res.LoadImage(JPGBuff))
            {
                Debug.Log("BAD JPG:" + buff[0] + buff[1] + buff[2] + buff[3]);
            }
            else
            {
                Debug.Log("GOOD JPG:" + buff[0] + buff[1] + buff[2] + buff[3]);
                MR.material.mainTexture = res;
                //GameObject.Find("")
            }

            waitForCallback = false;
            //res = true;
            //if (res)
            //{
            //    Debug.Log("angry " + buff[0]);
            //    Debug.Log("disgust " + buff[1]);
            //    Debug.Log("happy " + buff[2]);
            //    Debug.Log("neutral " + buff[3]);
            //    Debug.Log("sad " + buff[4]);
            //    Debug.Log("scared " + buff[5]);
            //    Debug.Log("surprised " + buff[6]);
            //}
            return res;
        }


        public unsafe MoodMeEmotions.MDMEmotions DetectedEmotions
        {
            get
            {
                return _detectedemotions;
            }
        }

        private static string GetLastTrackerError()
        {
            string s = "X";

            return (s);
        }
        public int SetLicense(string email, string key)
        {
            return MoodMeEmotions.SetLicense(email, key);
        }
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

        static bool waitForCallback;

        public unsafe EmotionsInterface()
        {
            //string directory = Application.dataPath + "/StreamingAssets/data";


            try
            {

                handler = MoodMeEmotions.CreateInstance();
                if (handler == IntPtr.Zero)
                {
                    Debug.Log("Emotions STATUS: ERROR");
                }
                else
                {
                    MoodMeEmotions.SubscribeCallBack(ResultsCallback);
                    waitForCallback=false;
                }

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        [MonoPInvokeCallback(typeof(MoodMeEmotions.EmotionsCallback))]
        private unsafe static void ResultsCallback(float* buff)
        {
            if (buff != null)
            {
                _detectedemotions = new MoodMeEmotions.MDMEmotions()
                {
                    surprised = buff[0],
                    scared = buff[1],
                    disgust = buff[2],
                    happy = buff[3],
                    sad = buff[4],
                    angry = buff[5],
                    neutral = buff[6],
                    latency = 0,
                    latency_avg = 0,
                    AllZero = (buff[0] + buff[1] + buff[2] + buff[3] + buff[4] + buff[5] + buff[6]) == 0,
                    Error = false

                };
                Debug.Log("OSX ");
                Debug.Log("surprised " + buff[0]);
                Debug.Log("scared " + buff[1]);
                Debug.Log("disgust " + buff[2]);
                Debug.Log("happy " + buff[3]);
                Debug.Log("sad " + buff[4]);
                Debug.Log("angry " + buff[5]);
                Debug.Log("neutral " + buff[6]);
                waitForCallback = false;
            }
        }


        public unsafe bool ProcessFrame(IntPtr buffer, int width, int height, int faceid)
        {
            bool res = false;
            if (waitForCallback) return true;

            res = MoodMeEmotions.ProcessImageBuffer(handler, buffer, width, height, width * 4, MoodMeEmotions.IOS_ROTATION.DOWN);
            waitForCallback = false;
            //res = true;
            //if (res)
            //{
            //    Debug.Log("angry " + buff[0]);
            //    Debug.Log("disgust " + buff[1]);
            //    Debug.Log("happy " + buff[2]);
            //    Debug.Log("neutral " + buff[3]);
            //    Debug.Log("sad " + buff[4]);
            //    Debug.Log("scared " + buff[5]);
            //    Debug.Log("surprised " + buff[6]);
            //}
            return res;
        }

        //public unsafe Texture2D ProcessFrameWithPreview(IntPtr buffer, int width, int height, int faceid, MeshRenderer MR)
        //{
        //    Debug.Log("CALL PROCESS FRAME WITH PREVIEW");
        //    Texture2D res = new Texture2D(640,480, TextureFormat.RGBA32, false);
        //    //if (waitForCallback) return res;
        //    byte* buff;
        //    int buffLength;
        //    MoodMeEmotions.ProcessImageBuffer(handler, buffer, width, height, width * 4, MoodMeEmotions.IOS_ROTATION.NONE);
        //    MoodMeEmotions.GetPreviewImageJPG(out buff, out buffLength);
        //    Debug.Log("PREVIEW LENGTH "+buffLength);
        //    byte[] JPGBuff = new byte[buffLength];
        //    for(int i=0; i<buffLength; i++)
        //    {
        //        JPGBuff[i] = buff[i];
        //    }
        //    if (!res.LoadImage(JPGBuff))
        //    {
        //        Debug.Log("BAD JPG:" + buff[0] + buff[1] + buff[2] + buff[3]);
        //    }
        //    else
        //    {
        //        Debug.Log("GOOD JPG:" + buff[0] + buff[1] + buff[2] + buff[3]);
        //        MR.material.mainTexture = res;
        //        //GameObject.Find("")
        //    }

        //    waitForCallback = false;
        //    //res = true;
        //    //if (res)
        //    //{
        //    //    Debug.Log("angry " + buff[0]);
        //    //    Debug.Log("disgust " + buff[1]);
        //    //    Debug.Log("happy " + buff[2]);
        //    //    Debug.Log("neutral " + buff[3]);
        //    //    Debug.Log("sad " + buff[4]);
        //    //    Debug.Log("scared " + buff[5]);
        //    //    Debug.Log("surprised " + buff[6]);
        //    //}
        //    return res;
        //}


        public unsafe MoodMeEmotions.MDMEmotions DetectedEmotions
        {
            get
            {
                return _detectedemotions;
            }
        }

        private static string GetLastTrackerError()
        {
            string s = "X";

            s = "";

            return (s);
        }

          public int SetLicense(string email, string key)
        {
            return MoodMeEmotions.SetLicense(email, key);
        }

#else
        public EmotionsInterface()
        {
            string directory = Application.dataPath + "/StreamingAssets/data";


            try
            {

                handler = MoodMeEmotions.CreateInstance(directory);
                if (handler == IntPtr.Zero)
                {
                    Debug.Log("Emotions STATUS:" + GetLastTrackerError());
                }

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }


        public unsafe bool ProcessFrame(IntPtr buffer, int width, int height, int faceid)
        {
            bool res = false;
            res = MoodMeEmotions.ProcessImageBuffer(handler, buffer, width, height, 0, MoodMeEmotions.FR_FORMAT.FRAME_RGB, faceid, out buff);
            //res = true;
            //if (res)
            //{
            //    Debug.Log("surprised " + buff[0]);
            //    Debug.Log("scared " + buff[1]);
            //    Debug.Log("disgust " + buff[2]);
            //    Debug.Log("happy " + buff[3]);
            //    Debug.Log("sad " + buff[4]);
            //    Debug.Log("angry " + buff[5]);
            //    Debug.Log("neutral " + buff[6]);
            //}
            return res;
        }


        public unsafe MoodMeEmotions.MDMEmotions DetectedEmotions
        {
            get
            {
                if (buff != null)
                {
                    _detectedemotions = new MoodMeEmotions.MDMEmotions()
                    {
                        surprised = buff[0],
                        scared = buff[1],
                        disgust = buff[2],
                        happy = buff[3],
                        sad = buff[4],
                        angry = buff[5],
                        neutral = buff[6],
                        latency = 0,
                        latency_avg = 0,
                        AllZero = (buff[0] + buff[1] + buff[2] + buff[3] + buff[4] + buff[5] + buff[6]) == 0,
                        Error = false
                    };

                }
                return _detectedemotions;
            }
        }

        private static string GetLastTrackerError()
        {
            string s = "X";

            s = Marshal.PtrToStringAnsi(MoodMeEmotions.GetLastTrackerError());

            return (s);
        }
          public int SetLicense(string email, string key)
        {
            return MoodMeEmotions.SetLicense(email, key);
        }

#endif

    }
}