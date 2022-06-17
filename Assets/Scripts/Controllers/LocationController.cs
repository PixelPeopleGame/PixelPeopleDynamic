using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.Android;

namespace SpecialControllers
{
    public class LocationController : Singleton<LocationController>
    {
        [SerializeField] private float _updateInterval = 1;

        public double Longitude { get; private set; } = 0;
        public double Latitude { get; private set; } = 0;

        public override void Awake()
        {
            StartCoroutine(GetLocation());
        }

        //set up voor de locatie
        private IEnumerator GetLocation()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
                Permission.RequestUserPermission(Permission.CoarseLocation);
            }

            if (!Input.location.isEnabledByUser)
                yield break;

            Input.location.Start();

            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            if (maxWait < 1)
            {
                print("Timed out");
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                print("Unable to determine device location");
                yield break;
            }

            StartCoroutine(UpdateLocation());
        }


        //update de locatie van de tablet
        IEnumerator UpdateLocation()
        {
            while (true)
            {
                Longitude = Math.Round(Input.location.lastData.longitude, 6);
                Latitude = Math.Round(Input.location.lastData.latitude, 6);
                DebugUI.Instance.UpdateLongLatText(Longitude, Latitude);
                yield return new WaitForSeconds(_updateInterval);
            }
        }

        public void ChangeCoordinates(float f, float f1)
        {
        }
    }
}