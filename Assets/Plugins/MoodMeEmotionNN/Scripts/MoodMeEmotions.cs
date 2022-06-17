
using System.Runtime.InteropServices;
using UnityEngine;

namespace MoodMe
{
    public class MoodMeEmotions
    {
        public enum FR_FORMAT
        {
            FRAME_BGRA,
            FRAME_RGB,
            FRAME_RGBA
        };

        public struct MDMEmotions
        {
            public float surprised;
            public float scared;
            public float disgust;
            public float happy;
            public float sad;
            public float angry;
            public float neutral;
            public long latency;
            public long latency_avg;
            public bool AllZero;
            public bool Error;
        };

        public enum IOS_ROTATION
        {
            NONE = -1,
            UP = 1,
            DOWN = 3,
            LEFT = 8,
            RIGHT = 6,
            UPMIRRORED = 2,
            DOWNMIRRORED = 4,
            LEFTMIRRORED = 5,
            RIGHTMIRRORED = 7
        };

#if UNITY_IOS && !UNITY_EDITOR

        // On C# side this callback should be declared as:
        public unsafe delegate void EmotionsCallback(float* emotionsPointer);

        [System.Runtime.InteropServices.DllImportAttribute("__Internal", CharSet = CharSet.Ansi, EntryPoint = "CreateInstance")]
        public static extern System.IntPtr CreateInstance();

        // image must be of RGBA format
        // Whole frame
        [System.Runtime.InteropServices.DllImportAttribute("__Internal", CharSet = CharSet.Ansi, EntryPoint = "ProcessFrameBuffer")]
        public static extern bool ProcessImageBuffer(System.IntPtr pInstance, System.IntPtr pBuffer, int width, int height, int bytesPerRow, IOS_ROTATION rotation = IOS_ROTATION.UP);

        // image must be of RGBA format
        // Face image
        [System.Runtime.InteropServices.DllImportAttribute("__Internal", CharSet = CharSet.Ansi, EntryPoint = "ProcessFaceImageBuffer")]
        public static extern bool ProcessFaceImageBuffer(System.IntPtr pInstance, System.IntPtr pBuffer, int width, int height, int bytesPerRow);

        [System.Runtime.InteropServices.DllImportAttribute("__Internal", CharSet = CharSet.Ansi, EntryPoint = "SubscribeCallBack")]
        public static extern void SubscribeCallBack(EmotionsCallback func);

        [System.Runtime.InteropServices.DllImportAttribute("__Internal", CharSet = CharSet.Ansi, EntryPoint = "GetPreviewImage")]
        public unsafe static extern void GetPreviewImageJPG(out byte* buffer, out int length);

        [System.Runtime.InteropServices.DllImportAttribute("__Internal", CharSet = CharSet.Ansi, EntryPoint = "SetLicense")]
        public static extern int SetLicense(string email, string key);
 

#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

        // On C# side this callback should be declared as:
        public unsafe delegate void EmotionsCallback(float* emotionsPointer);

        [System.Runtime.InteropServices.DllImportAttribute("MDMEmotionsMac", CharSet = CharSet.Ansi, EntryPoint = "CreateInstance")]
        public static extern System.IntPtr CreateInstance();

        // image must be of RGBA format
        // Whole frame
        [System.Runtime.InteropServices.DllImportAttribute("MDMEmotionsMac", CharSet = CharSet.Ansi, EntryPoint = "ProcessFrameBuffer")]
        public static extern bool ProcessImageBuffer(System.IntPtr pInstance, System.IntPtr pBuffer, int width, int height, int bytesPerRow, IOS_ROTATION rotation = IOS_ROTATION.NONE);

        // image must be of RGBA format
        // Face image
        //[System.Runtime.InteropServices.DllImportAttribute("MDMEmotionsMac", CharSet = CharSet.Ansi, EntryPoint = "ProcessFaceImageBuffer")]
        //public static extern bool ProcessFaceImageBuffer(System.IntPtr pInstance, System.IntPtr pBuffer, int width, int height, int bytesPerRow);

        [System.Runtime.InteropServices.DllImportAttribute("MDMEmotionsMac", CharSet = CharSet.Ansi, EntryPoint = "SubscribeCallBack")]
        public static extern void SubscribeCallBack(EmotionsCallback func);

        //[System.Runtime.InteropServices.DllImportAttribute("MDMEmotionsMac", CharSet = CharSet.Ansi, EntryPoint = "GetPreviewImage")]
        //public unsafe static extern void GetPreviewImageJPG(out byte* buffer, out int length);

        [System.Runtime.InteropServices.DllImportAttribute("MDMEmotionsMac", CharSet = CharSet.Ansi, EntryPoint = "SetLicense")]
        public static extern int SetLicense(string email, string key);


#elif (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
        [System.Runtime.InteropServices.DllImportAttribute("EmotioNNSDK", CharSet = CharSet.Ansi, EntryPoint = "CreateInstance")]
        public static extern System.IntPtr CreateInstance([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string dataDir);

        [System.Runtime.InteropServices.DllImportAttribute("EmotioNNSDK", EntryPoint = "processFrameByFaceIndex")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.I1)]
        public unsafe static extern bool ProcessImageBuffer(System.IntPtr pInstance, System.IntPtr pBuffer, int width, int height, int bytesPerRow, FR_FORMAT format, int faceIndex, out float* retValue);

        [System.Runtime.InteropServices.DllImportAttribute("EmotioNNSDK", CharSet = CharSet.Ansi, EntryPoint = "GetLastTrackerError")]
        public static extern System.IntPtr GetLastTrackerError();

        [System.Runtime.InteropServices.DllImportAttribute("EmotioNNSDK", CharSet = CharSet.Ansi, EntryPoint = "SetLicense")]
        public static extern int SetLicense(string email, string key);

#endif
    }
}
