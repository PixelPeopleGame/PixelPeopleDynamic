using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests
{
    public class waypoint
    {
        [Test]
        public void player_is_in_waypoint_range()
        {
            float playerLong = 50.100000f;
            float playerLat = 5;

            var waypoint = new ApiWaypoint();
            var distanceCheck = new DistanceCheck();
            
            Assert.IsFalse(distanceCheck.PlayerInWaypointRange(waypoint,playerLong, playerLat));

            waypoint.Longitude = 50.100090f;
            waypoint.Latitude = 5;

            Assert.IsTrue(distanceCheck.PlayerInWaypointRange(waypoint, playerLong, playerLat));
        }
    }
}