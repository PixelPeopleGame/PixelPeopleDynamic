using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WebContoller : MonoBehaviour
{
    public bool isLoaded = false;
    WebViewObject WebObject = new WebViewObject();

    public void LoadPage(string url)
    {
        if (WebObject != null)
        {
            UnloadPage();
        }

        WebObject = (new GameObject("WebObjectParent")).AddComponent<WebViewObject>();
        WebObject.Init(cb: (msg) => {
            Debug.Log(string.Format("CallFromJS[{0}]", msg));
        }, err: (msg) => {
            Debug.Log(string.Format("CallOnError[{0}]", msg));
        }, started: (msg) => {
            Debug.Log(string.Format("CallOnStarted[{0}]", msg));
        }, hooked: (msg) => {
            Debug.Log(string.Format("CallOnHooked[{0}]", msg));
        }, ld: (msg) => {
            Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
            WebObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");
        },
            //ua: "custom user agent string",
#if UNITY_EDITOR
            separated: false,
#endif
            enableWKWebView: true,
    wkContentMode: 0);
        WebObject.SetMargins(60, 220, 60, 110);
        WebObject.SetVisibility(true);

        WebObject.LoadURL(url.Replace(" ", "%20"));
        isLoaded = true;
    }

    public void UnloadPage()
    {
        var g = GameObject.Find("WebObjectParent");
        if (g != null)
        {
            Destroy(g);
            WebObject = null;

        }
        else
        {
            
        }
        isLoaded = false;
    }
}
