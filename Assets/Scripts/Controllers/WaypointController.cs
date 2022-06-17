using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using CurrentRoute;
using SpecialControllers;

using UI;
using System;
using TMPro;

namespace CurrentRoute
{
    public class WaypointController : Singleton<WaypointController>
    {
        [field: SerializeField]
        public TextMeshProUGUI WaypointNumberText { get; private set; }

        public DistanceCheck DistanceCheck { get; private set; }

        public override void Awake()
        {
            base.Awake();

            DistanceCheck = new DistanceCheck();

            RouteController.Instance.OnWaypointChanged += OnWaypointChanged;
        }

        private void Update()
        {
            if (RouteController.Instance.CurrentWaypoint == null)
                return;

            if (DistanceCheck.PlayerInWaypointRange(RouteController.Instance.CurrentWaypoint, LocationController.Instance.Longitude, LocationController.Instance.Latitude))
            {
                WaypointController.Instance.NextWaypoint();

                // No Clue
                Saves.SaveGameContoller.SavePlayer(RouteController.Instance.CurrentWaypoint.Id);
            }
        }

        private void OnWaypointChanged()
        {
            UpdateUI();
        }

        public void NextWaypoint()
        {
            for (int i = 0; i < RouteController.Instance.Route.Waypoints.Count; i++)
            {
                if (RouteController.Instance.CurrentWaypoint.Id == RouteController.Instance.Route.Waypoints[i].Id)
                {
                    if (i < RouteController.Instance.Route.Waypoints.Count + 1)
                    {
                        RouteController.Instance.CurrentWaypoint = RouteController.Instance.Route.Waypoints[i + 1];
                        break;
                    }
                    else
                    {
                        Debug.Log("Already on last Waypoint");
                    }
                }
            }
        }

        public void PreviousWaypoint()
        {
            for (int i = 0; i < RouteController.Instance.Route.Waypoints.Count; i++)
            {
                if (RouteController.Instance.CurrentWaypoint.Id == RouteController.Instance.Route.Waypoints[i].Id)
                {
                    if (i > 0)
                    {
                        RouteController.Instance.CurrentWaypoint = RouteController.Instance.Route.Waypoints[i - 1];
                        break;
                    }
                    else
                    {
                        Debug.Log("Already on first Waypoint");
                    }
                }
            }
        }

        public void SetWaypointByID(int id)
        {
            RouteController.Instance.CurrentWaypoint = RouteController.Instance.Route.Waypoints[id];
            return;
        }

        public void UpdateUI()
        {
            // Special UI
            // Call this first
            SpecialUI specialUI = RouteController.Instance.CurrentWaypoint.SpecialUI;

            // Loop thru possible values and enable or disable them
            foreach (SpecialUI value in Enum.GetValues(typeof(SpecialUI)))
            {
                bool enabled = false;

                if (value == specialUI)
                {
                    SetSpecialUIState(value, true);
                    enabled = true;
                }

                // Disable if not enabled
                if (!enabled)
                    SetSpecialUIState(value, false);
            }

            // Regular UI
            List<VisibleUI> visibleUis = (List<VisibleUI>)RouteController.Instance.CurrentWaypoint.UIVisible;

            // Loop thru possible values and enable or disable them
            foreach (VisibleUI value in Enum.GetValues(typeof(VisibleUI)))
            {
                bool enabled = false;

                for (int i = 0; i < visibleUis.Count; i++)
                {
                    if (value == visibleUis[i])
                    {
                        SetUIState(value, true);
                        enabled = true;
                        break;
                    }
                }

                // Disable if not enabled
                if (!enabled)
                    SetUIState(value, false);
            }

            // Update Debug UI
            WaypointNumberText.text = $"WP: {RouteController.Instance.CurrentWaypoint.Id}";
        }

        private void SetUIState(VisibleUI ui, bool enabled)
        {
            switch (ui)
            {
                case VisibleUI.SettingsIcon:
                    UIController.Instance.SettingsButton.SetActive(enabled);
                    break;
                case VisibleUI.Minimap:
                    UIController.Instance.MinimapUI.SetActive(enabled);
                    break;
                case VisibleUI.Dummy:
                    UIController.Instance.DummyUI.SetActive(enabled);
                    break;
                default:
                    Debug.Log("UI Doesnt exist!");
                    break;
            }
        }

        private void SetSpecialUIState(SpecialUI ui, bool enabled)
        {
            switch (ui)
            {
                case SpecialUI.None:
                    break;

                // Camera
                case SpecialUI.MoodMe:
                    if (enabled)
                        EmotionController.Instance.StartMoodMe();
                    else
                        if (EmotionController.Instance.MoodMeEnabled)
                            EmotionController.Instance.StopMoodMe();
                    break;

                // AR
                case SpecialUI.ARMaze:
                    if (enabled)
                        MazeController.Instance.EnableMaze();
                    else
                        if (MazeController.Instance.MazeActive)
                            MazeController.Instance.DisableMaze();
                    break;
                case SpecialUI.ARDrone:
                    break;

                // Other
                //case SpecialUI.WebPage:
                //    // Enable Web Page thing
                //    if (enabled)
                //        // Show new
                //        StartCoroutine(Webpagina.Instance.ShowWebPage(RouteController.Instance.CurrentPopup.Link));
                //    else
                //        if (Webpagina.Instance.IsShowing)
                //            StartCoroutine(Webpagina.Instance.CloseCurrentWebPage());
                //    break;
                //case SpecialUI.VideoPlayer:
                //    if (enabled)
                //        //VideoContoller.Instance.PlayVideo(RouteController.Instance.CurrentPopup.Link);
                //        VideoContoller.Instance.PlayVideo("https://www.youtube.com/watch?v=L7L9ZUWisfM");
                //    else
                //        // Close if playing
                //        if (VideoContoller.Instance.IsPlaying)
                //            VideoContoller.Instance.CloseVideo();
                //    break;

                default:
                    Debug.Log("Special UI doesnt exist!");
                    break;
            }
        }

        public ApiWaypoint LookAtNextWaypoint()
        {
            if (RouteController.Instance.Route.Waypoints.Count >= RouteController.Instance.CurrentWaypoint.Id)
            {
                return RouteController.Instance.Route.Waypoints[RouteController.Instance.CurrentWaypoint.Id + 1];
            }
            else return RouteController.Instance.Route.Waypoints[0];
        }
    }
}
