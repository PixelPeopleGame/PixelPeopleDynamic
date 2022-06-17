using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UI;
using AllPopupTemplates;
using SpecialControllers;

namespace CurrentRoute
{
    public class PopupController : Singleton<PopupController>
    {
        public bool IsPopupShowing { get; set; } = false;

        public bool IsPopupFinished { get; set; } = false;

        [field: SerializeField]
        public PopupUI PopupObject { get; private set; }

        [field: SerializeField]
        public PopupAnimation PopupAnimation { get; private set; }

        [field: SerializeField]
        public PopupQuestionUI PopupQuestionUI { get; private set; }

        public override void Awake()
        {
            base.Awake();

            RouteController.Instance.OnWaypointChanged += OnWaypointChanged;
            PopupQuestionUI.OnRightAnswerClick += PopupQuestionUI_OnRightAnswerClick;
            PopupQuestionUI.OnWrongAnswerClick += PopupQuestionUI_OnWrongAnswerClick;
        }

        private void OnWaypointChanged()
        {
            // Show Popup
            RouteController.Instance.CurrentPopupIndex = 0;

            // Show the current popup
            StopAllCoroutines();
            StartCoroutine(ShowCurrentPopup());
        }

        #region QuestionClickEvents
        private void PopupQuestionUI_OnRightAnswerClick()
        {
            OnPopupFinished();
        }

        private void PopupQuestionUI_OnWrongAnswerClick()
        {
            
        }
        #endregion QuestionClickEvents

        private void OnPopupFinished()
        {
            // Show Popup
            ShowNextPopup();
        }

        public void ShowNextPopup()
        {
            if (RouteController.Instance.CurrentPopupIndex + 1 < RouteController.Instance.CurrentWaypoint.Popups.Count)
            {
                RouteController.Instance.CurrentPopupIndex += 1;

                // Show the current popup
                StopAllCoroutines();
                StartCoroutine(ShowCurrentPopup());
            }
            else
            {
                StopAllCoroutines();
                HidePopup();
            }
        }

        public void ShowPreviousPopup()
        {
            RouteController.Instance.CurrentPopupIndex = Mathf.Clamp(
                RouteController.Instance.CurrentPopupIndex - 1,
                0,
                RouteController.Instance.CurrentWaypoint.Popups.Count - 1
            );

            StopAllCoroutines();
            StartCoroutine(ShowCurrentPopup());
        }

        public IEnumerator ShowCurrentPopup()
        {
            yield return new WaitForSeconds(RouteController.Instance.CurrentPopup.Timer);

            UIController.Instance.PopupUI.SetActive(true);
            IsPopupShowing = true;

            // Show UI
            WaypointController.Instance.UpdateUI();

            switch (RouteController.Instance.CurrentPopup.PopupType)
            {
                // Jackson
                case PopupType.Jackson:
                    PopupObject.SetPopupData(RouteController.Instance.CurrentPopup);
                    PopupAnimation.PlayAnimation(AnimationType.Show);
                    AudioController.Instance.RequestSounds("jackson");
                    break;

                // Regular
                case PopupType.Information:
                    // Information
                    PopupObject.SetPopupData(RouteController.Instance.CurrentPopup);
                    PopupAnimation.PlayAnimation(AnimationType.Show);
                    AudioController.Instance.RequestSounds("notification");
                    break;

                // Question
                case PopupType.Question:
                    // Questions
                    PopupObject.SetPopupData(RouteController.Instance.CurrentPopup);
                    PopupAnimation.PlayAnimation(AnimationType.Show);
                    AudioController.Instance.RequestSounds("notification");
                    break;

                // Nothing
                case PopupType.NoPopup:
                    break;

                // Link
                case PopupType.Link:
                    // Enable Web Page thing
                    if (Webpagina.Instance.IsShowing)
                        StartCoroutine(Webpagina.Instance.CloseCurrentWebPage());
                    else
                    {
                        // Show web Page
                        StartCoroutine(Webpagina.Instance.ShowWebPage(RouteController.Instance.CurrentPopup.Link));

                        // Hide UI
                        UIController.Instance.MinimapUI.SetActive(false);
                        UIController.Instance.SettingsButton.SetActive(false);
                    }
                    break;

                // Video
                case PopupType.Video:
                    // Close if playing
                    if (VideoContoller.Instance.IsPlaying)
                        VideoContoller.Instance.CloseVideo();
                    else
                    {
                        // Show Video
                        VideoContoller.Instance.PlayVideo(RouteController.Instance.CurrentPopup.Link);

                        // Hide UI
                        UIController.Instance.MinimapUI.SetActive(false);
                        UIController.Instance.SettingsButton.SetActive(false);
                    }
                    break;

                // Wut
                default:
                    Debug.Log("ERROR: PopupType doesnt exist!");
                    break;
            }
        }

        private void HidePopup()
        {
            UIController.Instance.PopupUI.SetActive(false);
            IsPopupShowing = false;
        }
    }
}
