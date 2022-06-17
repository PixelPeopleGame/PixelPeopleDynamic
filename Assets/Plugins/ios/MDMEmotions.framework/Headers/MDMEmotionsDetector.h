//
//  MDMEmotionsDetector.h
//  MoodMeEmotionsSDK
//
// Copyright (c) 2019 MoodMe (http://www.mood-me.it)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#import <Foundation/Foundation.h>
#import <CoreVideo/CoreVideo.h>


/*!
 * @header
 * @copyright Copyright 2015-2019 MoodMe (@link http://www.mood-me.com @/link)
 * @meta http-equiv="Content-Type" content="text/html; charset=UTF-8"
 * @framework MDMEmotionsDetector
 * @abstract
 * @discussion MDMEmotionsDetector Framework provides the APIs and support for
 * Realtime Human Face Emotions detection
 * @author L.Y.Mesentsev
 * @version 1.0
 * @encoding utf-8
 * @frameworkcopyright Copyright (c) 2015-2019 MoodMe (@link http://www.mood-me.com @/link)
 */


NS_ASSUME_NONNULL_BEGIN


/*!
 * @class MDMEmotions
 * @brief MoodMe Emotions container object
 * @discussion This interface contains all emotions values.
 */
@interface MDMEmotions : NSObject
@property float surprised;
@property float scared;
@property float disgust;
@property float happy;
@property float sad;
@property float angry;
@property float neutral;
@property float latency;
- (MDMEmotions *)meanValuesOf:(int)amount;
@end


typedef void(^EmotionsCompletionHandler)(MDMEmotions * _Nullable , NSError * _Nullable );

/*!
 * @class MDMEmotionsDetector
 * @brief MoodMe Emotions Detector
 * @discussion You can instantiate and call its methods that usually return the MDMEmotions objects
 * @see MDMEmotions
 */
@interface MDMEmotionsDetector : NSObject

/*!
 *  @brief process  camera frame, returns emotions from given bounds
 *  @discussion iOS function that takes pixel buffer from camera,
 *  crop to face bounds and processes it.
 *  Returns emotions for this face image enclosed in bounds
 */
- (void)processPixelBuffer:(CVPixelBufferRef)buffer byFaceIndex:(int)faceIndex onComplete:(EmotionsCompletionHandler)completion;

/*!
 *  @brief process  camera frame, returns emotions from given bounds
 *  @discussion iOS function that takes pixel buffer from camera,
 *  crop to face bounds and processes it.
 *  Returns emotions for this face image enclosed in bounds
 */
- (void)processPixelBuffer:(CVPixelBufferRef)buffer byFaceBounds:(CGRect)faceBounds onComplete:(EmotionsCompletionHandler)completion;


/*!
 *  @brief process  camera frame, returns emotions from given image
 */
- (void)processFaceBuffer:(void *)buffer width:(int)width height:(int)height bytesPerRow:(int)bytesPerRow onComplete:(EmotionsCompletionHandler)completion;

- (void)processFrameBuffer:(void *)buffer width:(int)width height:(int)height bytesPerRow:(int)bytesPerRow rotation:(int)rotation onComplete:(EmotionsCompletionHandler)completion;

@end

NS_ASSUME_NONNULL_END
