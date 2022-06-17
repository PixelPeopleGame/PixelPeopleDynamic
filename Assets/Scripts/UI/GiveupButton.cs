using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpecialControllers;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class GiveupButton : MonoBehaviour
{

    [Range(0,100)]
    [SerializeField] float lossrate;

    Button _button;
    [SerializeField] private Button _settingsButton;

    PostMazeUI _postMazeUI;

    // Start is called before the first frame update
    void Awake()
    {
        _button = this.GetComponent<Button>();
    }

    public void EnableGiveupButton(MazeController maze)
    {
        _button.onClick.AddListener(() =>  {

            _settingsButton.onClick.Invoke();
            maze.EndMaze();
            this.gameObject.SetActive(false);

            /*for (int i = 0; i < _postMazeUI.pownedInfo.Length; i++)
            {
                if(Random.Range(0, 100) > lossrate) { 

                _postMazeUI.pownedInfo[i] = "[Redacted]";
                }
            }
            */
        });
    }
}
