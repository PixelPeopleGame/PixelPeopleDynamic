using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class RequestHeader
{
    public string Name { get; set; }
    public string Value { get; set; }
}

public static class ApiHandler
{
    // Example on how to use this
    //void CallApiProperly()
    //{
    //    List<Waypoint> waypoints = null;
    //    try
    //    {
    //        StartCoroutine(ApiHandler.GetRequest("https://pixelpeople.nl/PixelPeopleAPI/RouteFolder/API1.php", (json) => {
    //            waypoints = JsonHelper<List<Waypoint>>.FromJSON(json);
    //        }));
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.Log("ERROR: " + ex);
    //        throw;
    //    }
    //}

    //public static IEnumerator GetRequest(string uri, System.Action<string> callback)
    //{
    //    using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    //    {
    //        // Request and wait for the desired page.
    //        yield return webRequest.SendWebRequest();
    //        callback(webRequest.downloadHandler.text);

    //        switch (webRequest.result)
    //        {
    //            case UnityWebRequest.Result.Success:
    //                break;
    //            default:
    //                Debug.LogError("Error: " + webRequest.error);
    //                break;
    //        }
    //    }
    //}

    public static async Task<string> GetRequest(string uri, List<RequestHeader> requestHeaders = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            if (requestHeaders != null)
            {
                // Set Request Headers
                for (int i = 0; i < requestHeaders.Count; i++)
                {
                    webRequest.SetRequestHeader(requestHeaders[i].Name, requestHeaders[i].Value);
                }
            }
            
            // Send Request
            var asyncOp = webRequest.SendWebRequest();

            // await until request finished downloading
            while (asyncOp.isDone == false)
            {
                // Delay Task
                await Task.Delay(1000 / 30);
            }

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    return webRequest.downloadHandler.text;
                default:
                    Debug.LogError("Error: " + webRequest.error);
                    return null;
            }
        }
    }

    public static IEnumerator PostRequest(string uri, string data, List<RequestHeader> requestHeaders = null)
    {
        UnityWebRequest webRequest = new UnityWebRequest(uri, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        // Set Request Headers
        for (int i = 0; i < requestHeaders.Count; i++)
        {
            webRequest.SetRequestHeader(requestHeaders[i].Name, requestHeaders[i].Value);
        }

        webRequest.SetRequestHeader("Content-Type", "application/json");

        // Not sure why this is here other tha fething errors
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:
                break;
            default:
                Debug.LogError("Error: " + webRequest.error);
                break;
        }
    }

    /// <summary>
    /// Gets an image from a web link/api
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static async Task<Texture2D> GetRemoteTexture(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
        {
            // Request
            var asyncOp = webRequest.SendWebRequest();

            // await until request finished downloading
            while (asyncOp.isDone == false)
            {
                // Delay Task
                await Task.Delay(1000 / 30);
            }

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    return DownloadHandlerTexture.GetContent(webRequest);
                default:
                    Debug.LogError("Error: " + webRequest.error);
                    return null;
            }
        }
    }
}
