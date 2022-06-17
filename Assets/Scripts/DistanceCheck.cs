using System;
using UI;
using UnityEngine;

using CurrentRoute;

public class DistanceCheck
{
    //kijkt of de speler in een range is van de volgende waypoint
    public bool PlayerInWaypointRange(ApiWaypoint waypoint, double playerLongitude, double playerLatitude)
    {
        double distance = Distance(waypoint, playerLongitude, playerLatitude);
        
        DebugUI.Instance.UpdateDistanceText(distance);

        if (distance < WaypointController.Instance.LookAtNextWaypoint().Radius)
            return true;

        return false;
    }

    //berekent de afstand voormeer uitleg kijk op: https://www.youtube.com/watch?v=xvFZjo5PgG0
    private double Distance(ApiWaypoint waypoint, double playerLongitude, double playerLatitude)
    {
        var radEarth = 6371000;
        var latitudeA = waypoint.Latitude * Math.PI / 180.0;
        var latitudeB = playerLatitude * Math.PI / 180.0;
        var longitudeA = waypoint.Longitude * Math.PI / 180.0;
        var longitudeB = playerLongitude * Math.PI / 180.0;
        var Δlongitude = (longitudeB - longitudeA);
        var Δlatitude = (latitudeB - latitudeA);

        var t =
            Math.Sin(Δlatitude / 2) * Math.Sin(Δlatitude / 2) +
            Math.Sin(Δlongitude / 2) * Math.Sin(Δlongitude / 2) *
            Math.Cos(latitudeA) * Math.Cos(latitudeB);
        return radEarth * (2 * Math.Atan2(Math.Sqrt(t), Math.Sqrt(1 - t)));

    }
}