using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurrentRoute
{
    /// <summary>
    /// This class is mandatory to use, the methods within are NOT mandatory
    /// You can save stuff within classes or add methods here
    /// </summary>
    public class RouteController : Singleton<RouteController>
    {
        [field: SerializeField, Header("Route")]
        public ApiRoute Route { get; private set; }

        private ApiWaypoint _currentWaypoint;

        public ApiWaypoint CurrentWaypoint 
        {
            get => _currentWaypoint;
            set 
            { 
                _currentWaypoint = value;

                if (_currentWaypoint != null)
                    OnWaypointChanged?.Invoke();
            }
        }
        public delegate void OnVariableChangeDelegate();
        public event OnVariableChangeDelegate OnWaypointChanged;

        [field: SerializeField]
        public int CurrentPopupIndex { get; set; }

        // Returns the current popup
        public ApiPopup CurrentPopup => CurrentWaypoint.Popups[CurrentPopupIndex];

        public async void GetRoute(string routeName)
        {
            // Less Dynamic
            // SetCurrentRoute(JsonHelperNew<ApiRoute>.FromJSON(
            //    await ApiHandler.GetRequest($"https://{""}pixelpeople.nl/PixelPeopleAPI/RouteFolder/{routeName}.php/")));

            // Modular
            SetCurrentRoute(JsonHelperNew<ApiRoute>.FromJSON(
                await ApiHandler.GetRequest(routeName)));
        }

        /// <summary>
        /// Useless function that sets the Route
        /// </summary>
        /// <param name="route">To set the Route to</param>
        public void SetCurrentRoute(ApiRoute route)
        {
            Route = route;
        }

        public void StartRoute()
        {
            if (Route != null)
            {
                CurrentWaypoint = Route.Waypoints[0];
            }
        }

        public void StopRoute()
        {
            Route = null;
            CurrentWaypoint = null;
        }
    }
}
