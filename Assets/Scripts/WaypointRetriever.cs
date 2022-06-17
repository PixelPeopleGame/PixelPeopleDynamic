using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using CurrentRoute;

public class WaypointRetriever : MonoBehaviour
{
    public List<ApiWaypoint> Waypoints { get; private set; }

    private void Awake()
    {
        //StartCoroutine(WaypointRequest());
    }

    //private IEnumerator WaypointRequest()
    //{
    //    UnityWebRequest webRequest = UnityWebRequest.Get("https://pixelpeople.nl/PixelPeopleAPI/PixelPeopleAPI.php");
        
    //    yield return webRequest.SendWebRequest();

    //    if (webRequest.isNetworkError || webRequest.isHttpError)
    //    {
    //        Debug.LogError(webRequest.error);
    //        yield break;
    //    }

    //    string[] removedParent = webRequest.downloadHandler.text.Split(new char[] { ':' }, 2);
    //    string finalString = removedParent[1].Remove(removedParent[1].Length - 1);
    //    Debug.Log(finalString);
    //    Waypoint[] waypoints = JsonHelper.FromJson<Waypoint>(FixJsonString(finalString));
           
    //    FindObjectOfType<WaypointController>().FillWaypointList(waypoints.ToList());
    //    Waypoints = waypoints.ToList();
    //}

    //private string FixJsonString(string value)
    //{
    //    value = "{\"Items\":" + value + "}";
    //    return value;
    //}
}