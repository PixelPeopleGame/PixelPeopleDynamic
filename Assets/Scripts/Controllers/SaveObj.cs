using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObj : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Saves.SaveGameContoller.Init();
    }

}
