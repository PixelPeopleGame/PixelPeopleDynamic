using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UI;
using CurrentRoute;
using SpecialControllers;

public class Compass : MonoBehaviour
{
    private float _angle;

    private int totalHistory = 5;
    private int currentHistoryIndex = 0;

    private double[] x;
    private double[] y;


    private float currentHeading; //0-360

    private void Awake()
    {
        Input.compass.enabled = true;
        Input.location.Start();

        StartCoroutine(GetWaypointAngle());
    }

    private void Update()
    {
        RotateNeedle();
    }

    private void RotateNeedle()
    {
        if (RouteController.Instance.CurrentWaypoint == null)
            return;

        DebugUI.Instance.UpdateNorth(Input.compass.trueHeading);
        //DebugUI.Instance.UpdateNeedleAngle(_angle);
        float a2 = RemapNumber(_angle, 0, 360, 360, 0);

        DebugUI.Instance.UpdateNeedleAngle((a2 - Input.compass.trueHeading + 360) % 360);
    }


    //set het kompas goed. met een gemiddeld van ~.5 seconde om het kompas minder spinged te maken
    //en ze de hoek om in een 2 dimentionale waarde. dit is om problemen to ontlopen als de hoek rond de 0/360 graden zou zijn
    private IEnumerator GetWaypointAngle()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            _angle = (float)AngleFromCoordinate(LocationController.Instance.Latitude, LocationController.Instance.Longitude
                , WaypointController.Instance.LookAtNextWaypoint().Latitude, WaypointController.Instance.LookAtNextWaypoint().Longitude);

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

                currentHeading = (Input.compass.trueHeading + 360) % 360;
            }
            else
            {
                x[currentHistoryIndex] = Math.Cos(((Input.compass.trueHeading + 360) % 360) * Mathf.Deg2Rad);

                y[currentHistoryIndex] = Math.Sin(((Input.compass.trueHeading + 360) % 360) * Mathf.Deg2Rad);
                currentHistoryIndex++;

                if (currentHistoryIndex >= totalHistory)
                    currentHistoryIndex = 0;

                double tmpx=0;
                double tmpy = 0;
                for (int i = 0; i < totalHistory; i++)
                {
                    tmpx += x[i];
                    tmpy += y[i];
                }

                tmpx /= totalHistory;
                tmpy /= totalHistory;

                currentHeading = (float)Math.Atan2((float)tmpy, (float)tmpx) * Mathf.Rad2Deg;
                if (currentHeading < 0.0f) 
                    currentHeading += 360.0f;       
            }

            float a = _angle - currentHeading; 
            float a2 = RemapNumber(a, 0.0f, 360.0f, 360.0f, 0.0f);

            Vector3 needleDirection = new Vector3(0.0f, 0.0f, a2);
            transform.rotation = Quaternion.Euler(needleDirection);
            DebugUI.Instance.UpdateAngleText(_angle);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // berekent de hoek voor de compass. 
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

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnEnable()
    {
        StartCoroutine(GetWaypointAngle());
    }

}