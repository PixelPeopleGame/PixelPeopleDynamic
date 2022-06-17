using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using UnityEngine.Video;

[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileImage : MonoBehaviour, IPointerDownHandler {
    public VideoPlayer VideoPlayer;
    public CameraManager CameraManager;
    public RenderTexture VideoTexture;
    private Text guiText;
#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".png, .jpg", false);
    }

    // Called from browser
    public void OnFileUpload(string url) {
        StartCoroutine(OutputRoutine(url));
    }
#else
    //
    // Standalone platforms & editor
    //
    public void OnPointerDown(PointerEventData eventData) { }

    void Start() {
        var button = GetComponent<Button>();
        guiText = GetComponentInChildren<Text>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        if (guiText.text.Contains("Video"))
        {

            var paths = StandaloneFileBrowser.OpenFilePanel("Open video", "", "mp4", false);
            if (paths.Length > 0)
            {
                StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
                guiText.text = "Use Webcam";
            }
        }
        else
        {
            CameraManager.VideoTexure = null;
            VideoPlayer.Stop();
            guiText.text = "Open Video";
        }
    }
#endif

    private IEnumerator OutputRoutine(string url) {
        var loader = new WWW(url);
        yield return loader;

        CameraManager.VideoTexure = VideoTexture;
#if UNITY_EDITOR_OSX||UNITY_STANDALONE_OSX
        url=WWW.UnEscapeURL(url);
#endif
        VideoPlayer.url = url;
        VideoPlayer.isLooping = true;
        VideoPlayer.Play();

        //output.texture = loader.texture;
    }
}