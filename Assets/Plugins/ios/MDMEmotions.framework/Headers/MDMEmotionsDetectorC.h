//
//  MDMEmotionsDetectorC.h
//  MDMAnalytics
//
//  Created by Leonid Mesentsev on 17/01/21.
//  Copyright © 2021 MoodMe. All rights reserved.
//

#ifndef MDMEmotionsDetectorC_h
#define MDMEmotionsDetectorC_h

#define MDMSDK_API



extern "C" {
    
    // On C# side this callback should be declared as:
    // public delegate void EmotionsCallback(float emotions[7]);
    //
    typedef void (*EmotionsCallback)(float emotions[7]);
    
    MDMSDK_API void *CreateInstance();

    // Whole frame
    // image should be of RGBA format
    MDMSDK_API bool ProcessFrameBuffer(void *pInstance, void *pBuffer, int width, int height, int bytesPerRow, int rotation = -1);

    // Face image
    // image should be of RGBA format
    MDMSDK_API bool ProcessFaceImageBuffer(void *pInstance, void *pBuffer, int width, int height, int bytesPerRow);
    
    // On C# side this callback should be declared as:
    // [DllImport("MDMAnalytics.dll", CallingConvention = CallingConvention.StdCall)]
    // public static extern double SubscribeCallBack(EmotionsCallback func);
    MDMSDK_API void SubscribeCallBack(EmotionsCallback callback);
    
    MDMSDK_API void GetPreviewImage(unsigned char **buf, int *length);
    
    MDMSDK_API int SetLicense(char *email, char *key);

}


#endif /* MDMEmotionsDetectorC_h */
