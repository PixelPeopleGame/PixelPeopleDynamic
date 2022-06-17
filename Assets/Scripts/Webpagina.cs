using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI;
using CurrentRoute;
using System;

namespace SpecialControllers
{
    public class Webpagina : Singleton<Webpagina>
    {
        public bool IsShowing { get; private set; } = false;

        [field: SerializeField]
        public WebContoller WebContoller { get; private set; }

        [field: SerializeField]
        public PopupAnimation PopupAnimation { get; private set; }

        [field: SerializeField]
        public Animator Animator { get; private set; }

        public override void Awake()
        {
            base.Awake();
        }

        public IEnumerator ShowWebPage(string url)
        {
            IsShowing = true;

            // Disable UI
            UIController.Instance.PopupUI.SetActive(false);

            // Play Animation
            Animator.Play("Base Layer.FrameOpenWeb");
            PopupAnimation.PlayAnimation(AnimationType.Hide);
            yield return new WaitForSeconds(1.5f);

            // Open Web Page
            WebContoller.LoadPage(url);
            UIController.Instance.CloseWebButton.SetActive(true);
        }

        public IEnumerator CloseCurrentWebPage()
        {
            // Play animation
            Animator.Play("Base Layer.frameClose");
            yield return new WaitForSeconds(1.5f);

            // Stop Web Page
            WebContoller.UnloadPage();
            UIController.Instance.CloseWebButton.SetActive(false);

            // Enable UI
            UIController.Instance.PopupUI.SetActive(true);

            // Show next popup
            PopupController.Instance.ShowNextPopup();

            IsShowing = false;
        }

        /// <summary>
        /// Click event for CloseWebPage
        /// </summary>
        public void CloseButtonClick()
        {
            // Close Ui
            StartCoroutine(CloseCurrentWebPage());
        }
    }
}
