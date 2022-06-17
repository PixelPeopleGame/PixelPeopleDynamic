using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{

    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _switchDebug;

    [SerializeField] private GameObject[] _UI;
    [SerializeField] private GameObject[] _DebugUI;
    [SerializeField] private GameObject settingsCanves;
    [SerializeField] private GameObject _explanationMark;
    [SerializeField] private Text _DebugText;
    [SerializeField] private PopupAnimation _popupAnimation;
    private List<GameObject> ActiveObjects;
   

    bool OnOff_settings = false;
    bool OnOff_Debug = false;

    private void Awake()
    {
        ActiveObjects = new List<GameObject>();

        _settingsButton.onClick.AddListener(() =>
        {
            _explanationMark.SetActive(false);

            if(OnOff_settings == false) {
                OnOff_settings = true;
                foreach (GameObject gameObject in _UI)
                {
                    if(gameObject.activeSelf == true)
                    {
                        ActiveObjects.Add(gameObject);
                    }
                   
                    gameObject.SetActive(false);
                    settingsCanves.GetComponent<Canvas>().enabled = true;
                }
                if(OnOff_Debug == true)
                {
                    foreach (GameObject gameObject in _DebugUI)
                    {

                        if (gameObject.activeSelf == true)
                        {
                            ActiveObjects.Add(gameObject);
                        }

                        gameObject.SetActive(false);

                    }

                }
            }
            else
            {
                OnOff_settings = false;
                foreach (GameObject gameObject in ActiveObjects)
                {
                    
                    settingsCanves.GetComponent<Canvas>().enabled = false;
                    gameObject.SetActive(true);
                    
                }
                _popupAnimation.PlayAnimation(AnimationType.Show);

                ActiveObjects = new List<GameObject>();
            }
        });

        _switchDebug.onClick.AddListener(() =>
        {
            if (OnOff_Debug == false)
            {
                _DebugText.text = "Disable Debug UI";
                OnOff_Debug = true;

                foreach (GameObject O in _DebugUI)
                {
                    ActiveObjects.Add(O);
                }
            }
            else
            {
                _DebugText.text = "Enable Debug UI";
                OnOff_Debug = false;

                foreach(GameObject O in _DebugUI)
                {
                    try
                    {
                        ActiveObjects.Remove(O);
                    }
                    catch
                    {

                        
                    }
                }
            }
        });
    }
}
