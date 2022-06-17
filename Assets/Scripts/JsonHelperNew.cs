using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Networking;

public static class JsonHelperNew<T>
{
    /// <summary>
    /// Converts an C# Object to a json string
    /// </summary>
    /// <param name="obj">C# Object to convert</param>
    /// <returns></returns>
    public static string ToJSON(T obj)
    {
        try
        {
            return JsonConvert.SerializeObject(obj);
        }
        catch (Exception ex)
        {
            Debug.Log("ERROR: Jsonhelper.cs ln18: " + ex);

            return "";
        }
    }

    /// <summary>
    /// Converts a string to an C# object
    /// </summary>
    /// <param name="json">JSON string to convert</param>
    /// <returns></returns>
    public static T FromJSON(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception ex)
        {
            Debug.Log("ERROR: Jsonhelper.cs ln32: " + ex);

            return default(T);
        }
    }
}
