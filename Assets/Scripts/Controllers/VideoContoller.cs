using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI;
using UnityEngine.Video;

namespace SpecialControllers
{
    public class VideoContoller : Singleton<VideoContoller>
    {
        public bool IsPlaying { get; private set; } = false;

        [field: SerializeField]
        public PopupAnimation PopupA { get; private set; }

        [field: SerializeField]
        public VideoPlayer VideoPlayer { get; private set; }

        public void PlayVideo(string url)
        {
            IsPlaying = true;

            // Play Video
            VideoPlayer.gameObject.SetActive(true);

            VideoPlayer.Stop();
            VideoPlayer.url = url;
            VideoPlayer.Play();

            UIController.Instance.CloseVideoButton.SetActive(true);

            PopupA.StopAllCoroutines();
        }

        public void CloseVideo()
        {
            // Stop Video Playing
            PopupA.PlayAnimation(AnimationType.Show);

            // Disable Video UI
            VideoPlayer.Stop();
            VideoPlayer.gameObject.SetActive(false);
            UIController.Instance.CloseVideoButton.SetActive(false);

            IsPlaying = false;
        }
    }
}
