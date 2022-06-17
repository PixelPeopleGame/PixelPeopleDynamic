using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CurrentRoute;

public class PopupTestUI : MonoBehaviour
{
    [SerializeField] private Button _previousWaypointButton;
    [SerializeField] private Button _nextWaypointButton;

    [SerializeField] private Button _previousPopupButton;
    [SerializeField] private Button _nextPopupButton;

    private void Awake()
    {
        // Will become functions, soon
        _previousWaypointButton.onClick.AddListener(() => { WaypointController.Instance.PreviousWaypoint(); });
        _nextWaypointButton.onClick.AddListener(() => { WaypointController.Instance.NextWaypoint(); });

        _previousPopupButton.onClick.AddListener(() => { PopupController.Instance.ShowPreviousPopup(); });
        _nextPopupButton.onClick.AddListener(() => { PopupController.Instance.ShowNextPopup(); });
    }
}