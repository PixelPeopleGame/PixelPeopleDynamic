using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UI;

namespace SpecialControllers
{
    public class EmotionController : Singleton<EmotionController>
    {
        public bool MoodMeEnabled { get; private set; } = false;

        [field: SerializeField] 
        public GameObject MoodMe { get; private set; }

        // Ask for Camera permissions
        private void Start()
        {
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
                Permission.RequestUserPermission(Permission.Camera);
#endif
        }

        public void StartMoodMe()
        {
            MoodMeEnabled = true;

            // Enable MoodMe
            MoodMe.SetActive(true);
            UIController.Instance.MoodMeUI.SetActive(true);
        }

        public void StopMoodMe()
        {
            // Disable MoodMe
            MoodMe.SetActive(false);
            UIController.Instance.MoodMeUI.SetActive(false);

            MoodMeEnabled = false;
        }
    }
}