using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UI;
using CurrentRoute;
using SpecialControllers;

public class Minimap : MonoBehaviour
{
    [field: SerializeField, Header("GameObjects")]
    public GameObject NextWaypointAngle { get; private set; }

    [field: SerializeField]
    public RectTransform MapRect { get; private set; }

    [field: SerializeField]
    public RectTransform NorthRect { get; private set; }

    [field: SerializeField]
    public GameObject North { get; private set; }

    [Header("Zooming")]
    public float minZoom = 1.0f;
    public float maxZoom = 8.5f;
    public float zoomLevel = 7.0f;
    public float zoomValue = 1.5f;

    private bool _locationEnabled = false;
    UnityEngine.Compass cp = new UnityEngine.Compass();

    private float time = 0.0f;

    private float _angle;

    private int totalHistory = 5;
    private int currentHistoryIndex = 0;

    private double[] x;
    private double[] y;

    private float _currentHeading; // 0-360

    private float _currentLatitude;
    private float _currentLongitude;

    private float _centerLatitude = 51.429153f;
    private float _centerLongitude = 5.45879121986f;

    private void Awake()
    {
        Input.compass.enabled = true;
        Input.location.Start();

        EnableLocation();
        StartCoroutine(GetWaypointAngle());
    }

    private bool _setRotation = false;
    private float oldRotation = 0.0f;
    private float newRotation = 0.0f;
    public float currentRotation = 0.0f;
    float waitTime = 0.0f;

    private void Update()
    {
        time += Time.deltaTime;

        RotateNeedle();

        if (_locationEnabled)
        {
            Vector2 toMove = CalculateNewPositionOnMap(
                new Vector2(_centerLatitude, _centerLongitude),
                new Vector2(_currentLatitude, _currentLongitude)
            );

            // 1920 is map original size, NOT SCREEN SIZE!
            MapRect.pivot = new Vector2(
                1.0f / 1920.0f * toMove.x + 0.5f,
                1.0f / 1920.0f * toMove.y + 0.5f
            );

            float rotation = -360.0f + Mathf.Round(cp.trueHeading);

            if (Mathf.Abs(rotation - newRotation) > 10.0f)
            {
                // Move counter clockwise
                if (oldRotation > 350.0f && rotation > 0.0f && rotation < 10.0f)
                {
                    SetRotation(MapRect, rotation);
                }
                else if (oldRotation < 10.0f && rotation > 350.0f)
                {
                    SetRotation(MapRect, rotation);
                }
                else
                {
                    // Regular
                    waitTime = 0.0f;
                    _setRotation = true;
                    oldRotation = newRotation;
                    newRotation = rotation;
                }
            }

            if (_setRotation)
            {
                waitTime += Time.deltaTime;

                if (waitTime >= 0.25f)
                    waitTime = 0.25f;

                // Set Rotation
                currentRotation = Mathf.Lerp(oldRotation, newRotation, waitTime * 4.0f);
                SetRotation(MapRect, currentRotation);
                SetRotation(NorthRect,currentRotation);

                if (waitTime >= 0.25f)
                {
                    _setRotation = false;
                    waitTime = 0.0f;
                }
            }
        }
    }

    public void EnableLocation()
    {
        // Enabke Gyro
        Input.gyro.enabled = true;

        // Start Location Service
        if (Input.location.isEnabledByUser)
        {
            // Starts the location service.
            Input.location.Start(0.1f, 0.001f);

            // Waits until the location service initializes
            int maxWait = 10;
            while (Input.location.status == LocationServiceStatus.Initializing)
            {
                Debug.Log("INFO: Waiting...");
                new WaitForSeconds(1);
                maxWait--;

                // If the service didn't initialize in 10 seconds this cancels location service use.
                if (maxWait < 1)
                {
                    Debug.Log("Timed out");
                    break;
                }
            }

            // If the connection failed this cancels location service use.
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determine device location");
            }
            else
            {
                // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
                Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
                _locationEnabled = true;
                cp.enabled = true;
            }

            // Stops the location service if there is no need to query location updates continuously.
            //Input.location.Stop();s
        }
        else
            Debug.Log("ERROR: Location is not enabled by the user");
    }

    private void RotateNeedle()
    {
        if (RouteController.Instance.CurrentWaypoint == null)
            return;

        DebugUI.Instance.UpdateNorth(Input.compass.trueHeading);
        //DebugUI.Instance.UpdateNeedleAngle(_angle);
        float a2 = RemapNumber(_angle, 0.0f, 360.0f, 360.0f, 0.0f);

        DebugUI.Instance.UpdateNeedleAngle((a2 - Input.compass.trueHeading + 360.0f) % 360.0f);
    }

    private IEnumerator GetWaypointAngle()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            _angle = (float)AngleFromCoordinate(LocationController.Instance.Latitude, LocationController.Instance.Longitude,
                WaypointController.Instance.LookAtNextWaypoint().Latitude, WaypointController.Instance.LookAtNextWaypoint().Longitude);

            if (x == null)
            {
                x = new double[totalHistory];
                y = new double[totalHistory];

                currentHistoryIndex = 0;

                for (int i = 0; i < totalHistory; i++)
                {

                    x[i] = Math.Sin((Input.compass.trueHeading + 360) % 360 * Mathf.Deg2Rad);
                    y[i] = Math.Cos((Input.compass.trueHeading + 360) % 360 * Mathf.Deg2Rad);
                }

                _currentHeading = (Input.compass.trueHeading + 360) % 360;
            }
            else
            {
                x[currentHistoryIndex] = Math.Cos(((Input.compass.trueHeading + 360) % 360) * Mathf.Deg2Rad);

                y[currentHistoryIndex] = Math.Sin(((Input.compass.trueHeading + 360) % 360) * Mathf.Deg2Rad);
                currentHistoryIndex++;

                if (currentHistoryIndex >= totalHistory)
                    currentHistoryIndex = 0;

                double tmpx = 0;
                double tmpy = 0;
                for (int i = 0; i < totalHistory; i++)
                {
                    tmpx += x[i];
                    tmpy += y[i];
                }

                tmpx /= totalHistory;
                tmpy /= totalHistory;

                _currentHeading = (float)Math.Atan2((float)tmpy, (float)tmpx) * Mathf.Rad2Deg;
                if (_currentHeading < 0)
                    _currentHeading += 360f;
            }

            _currentLatitude = Input.location.lastData.latitude;
            _currentLongitude = Input.location.lastData.longitude;

            float a = _angle - _currentHeading;
            float a2 = RemapNumber(a, 0, 360, 360, 0);

            Vector3 needleDirection = new Vector3(0, 0, a2);

            // Set Rotation
            NextWaypointAngle.transform.rotation = Quaternion.Euler(needleDirection);

            // Update UI
            DebugUI.Instance.UpdateAngleText(_angle);
            yield return new WaitForSeconds(.1f);
        }
    }

    private double AngleFromCoordinate(double lat1, double long1, double lat2, double long2)
    {
        double dLat = (lat2 - lat1) * Mathf.Deg2Rad;
        double dLon = (long2 - long1) * Mathf.Deg2Rad;

        lat1 *= Mathf.Deg2Rad;
        lat2 *= Mathf.Deg2Rad;


        double y = Math.Sin(dLon) * Math.Cos(lat2);
        double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1)
            * Math.Cos(lat2) * Math.Cos(dLon);

        double brng = Math.Atan2(y, x);

        brng *= Mathf.Rad2Deg;
        brng = (brng + 360) % 360;

        if (brng < 0)
        {
            brng = 360 - Math.Abs(brng);
        }


        return brng;

    }

    /*verzet de nummer ik een range naar een anderen 
     * bijvoorbeeld
     * RemapNumber(float 50, float 0, float 200, float 100, float 500) zal 200 terug geven
     * de value zou ook buiten de range gebruikt moeten kunnen worden maar dit is niet getest
     * */
    private float RemapNumber(float value, float start1, float stop1, float start2, float stop2)
    {
        float outgoing =
          start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
        return (float)outgoing;
    }

    public float HaversineDistance(Vector2 pos1, Vector2 pos2)
    {
        float R = 6371.0f;
        float lat = (pos2.x - pos1.x) * Mathf.Deg2Rad;
        float lng = (pos2.y - pos1.y) * Mathf.Deg2Rad;
        float h1 = Mathf.Sin(lat / 2.0f) * Mathf.Sin(lat / 2.0f) +
                      Mathf.Cos(pos1.x * Mathf.Deg2Rad) * Mathf.Cos(pos2.x * Mathf.Deg2Rad) *
                      Mathf.Sin(lng / 2.0f) * Mathf.Sin(lng / 2.0f);
        float h2 = 2 * Mathf.Asin(Mathf.Min(1, Mathf.Sqrt(h1)));

        return (R * h2) * 1000.0f;
    }

    private float MetersPerPixel(float centerLatitude, int zoom)
    {
        // https://wiki.openstreetmap.org/wiki/Zoom_levels
        // getal * cos(lat in degrees) / 2 ^ (zoom + 8);
        // (40075016.686 * cos(51.428708 degrees) / 2 ^ (16 + 8));
        // (40075016.686f * Mathf.Cos(float.Parse((Mathf.Deg2Rad * latitude).ToString()))) / Mathf.Pow(2, 16 + 8) * pixels;
        float metersPerPixel = (40075016.686f * Mathf.Cos(float.Parse((Mathf.Deg2Rad * centerLatitude).ToString()))) / Mathf.Pow(2, zoom + 8);
        return metersPerPixel;
    }

    private Vector2 CalculateNewPositionOnMap(Vector2 centerPoint, Vector2 newPoint, int zoom = 16)
    {
        // Idea to do this on an offline map
        // Get distance in meters per pixel
        float mpp = MetersPerPixel(centerPoint.x, zoom);
        /*
         // MetersPerPixel
         MetersPerPixel = 5
         Pixels: 10px
         Meters: 10 * 5 = 50m

         // PixelPerMeter
         MetersPerPixel = 5
         Pixels: 50 / 5 = 10px
         Meters: 50m
        */

        // Calculate horizontal distance
        //float horizontalDistance = Haversine(p1.x, p1.y, p2.x, p2.y);
        float verticalDistance = HaversineDistance(centerPoint, new Vector2(newPoint.x, centerPoint.y));
        float verticalPixelCount = verticalDistance / mpp;

        // Calculate vertical distance
        //float verticalDistance = Haversine(p2.x, p2.y, p3.x, p3.y);
        float horizontalDistance = HaversineDistance(new Vector2(newPoint.x, centerPoint.y), newPoint);
        float horizontalPixelCount = horizontalDistance / mpp;

        // Output this once
        //Debug.Log("mX: " + horizontalDistance + " mY: " + verticalDistance);
        //Debug.Log("pX: " + horizontalPixelCount + " pY: " + verticalPixelCount);

        return new Vector2(horizontalPixelCount, verticalPixelCount);
    }

    /// <summary>
    /// Zooms in on the Map with a default amount
    /// </summary>
    public void ZoomIn()
    {
        SetZoom(zoomLevel + zoomValue);
    }

    /// <summary>
    /// Zooms out on the Map with a default amount
    /// </summary>
    public void ZoomOut()
    {
        SetZoom(zoomLevel - zoomValue);
    }

    private void SetZoom(float newZoom, bool overrideLimiters = false)
    {
        // Set and limit Zoom level
        zoomLevel = overrideLimiters ? newZoom : newZoom < minZoom ? minZoom : newZoom > maxZoom ? maxZoom : newZoom;

        // Change map
        MapRect.localScale = new Vector3(zoomLevel, zoomLevel, 1.0f);
    }

    private void SetRotation(RectTransform rt, float rotation)
    {
        // <-- 360<-0 * 360->0 -->
        float toRotate = (Mathf.Round(rotation * 100.0f) / 100.0f) - (Mathf.Round(rt.localEulerAngles.z * 100.0f) / 100.0f);
        rt.Rotate(new Vector3(0.0f, 0.0f, toRotate));
    }
}
