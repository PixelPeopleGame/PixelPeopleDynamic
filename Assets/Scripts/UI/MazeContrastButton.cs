using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpecialControllers;

[RequireComponent(typeof(Button))]
public class MazeContrastButton : MonoBehaviour
{
    [SerializeField] private MazeController _mazeController;


    // Start is called before the first frame update
    void Awake()
    {

        _mazeController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MazeController>();

        this.GetComponent<Button>().onClick.AddListener(() =>
        {

            _mazeController.MaterialSwitch();


        });
    }
}
