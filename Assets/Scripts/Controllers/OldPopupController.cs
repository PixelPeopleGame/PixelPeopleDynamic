using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;

using CurrentRoute;
using SpecialControllers;
using AllPopupTemplates;

namespace CurrentRoute
{
    public class OldPopupController : Singleton<OldPopupController>
    {
        //[field: SerializeField]
        //public GameObject Popup { get; private set; }

        //[field: SerializeField]
        //public PopupUI PopupUI { get; private set; }

        //[field: SerializeField]
        //public PopupAnimation PopupAnimation { get; private set; }

        //public bool IsPopupEnabled { get; private set; }

        //public bool IsCurrentWaypointDone { get; private set; }

        //public bool NeedlePopup { get; private set; } = false;

        //public override void Awake()
        //{
        //    base.Awake();

        //    PopupAnimation = GetComponentInChildren<PopupAnimation>();

        //    //RouteController.Instance.OnWaypointChanged += HandleWaypointChanged;
        //}

        //private void HandleWaypointChanged()
        //{
        //    RouteController.Instance.CurrentPopupIndex = 0;
        //    IsCurrentWaypointDone = false;
        //    ShowPopup();
        //}

        //public void ShowNextPopup()
        //{
        //    if (RouteController.Instance.CurrentPopupIndex >= RouteController.Instance.CurrentWaypoint.Popups.Count - 1)
        //    {
        //        IsCurrentWaypointDone = true;
        //        return;
        //    }

        //    RouteController.Instance.CurrentPopupIndex++;
        //    ShowPopup();
        //}

        //public void ShowPreviousPopup()
        //{
        //    if (RouteController.Instance.CurrentPopupIndex <= 0)
        //        return;

        //    RouteController.Instance.CurrentPopupIndex--;
        //    ShowPopup();
        //}

        //public void ShowPopup()
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(ShowPopupWithDelay());
        //}

        //private IEnumerator ShowPopupWithDelay()
        //{
        //    ApiWaypoint currentWaypoint = RouteController.Instance.CurrentWaypoint;

        //    yield return new WaitForSeconds(RouteController.Instance.CurrentPopup.Timer);
        //    ApiPopup popup = currentWaypoint.Popups[RouteController.Instance.CurrentPopupIndex];

        //    PopupUI.SetPopupData(popup);

        //    // Play Sounds
        //    if (popup.PopupType == PopupType.Jackson)
        //    {
        //        AudioController.Instance.RequestSounds("glitch");
        //    }
        //    else
        //        AudioController.Instance.RequestSounds("notification");


        //    if (PopupUI.gameObject.activeSelf && popup.PopupType != PopupType.Link)
        //    {
        //        PopupAnimation.PlayAnimation(AnimationType.Show);
        //    }
        //}
    }
}
