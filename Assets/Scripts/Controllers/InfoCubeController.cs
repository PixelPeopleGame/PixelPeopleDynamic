using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCubeController : MonoBehaviour
{
    [SerializeField] private HaveIBeenPownedRetriever haveIBeenPownedRetriever;


    
    private void Awake()
    {
        haveIBeenPownedRetriever = GameObject.FindGameObjectWithTag("GameController").GetComponent<HaveIBeenPownedRetriever>();
    }


    //wordt genoemd wanner een spelen de cube opent in de maze
    public string RequestInfo()
    {
        //Destroy(this,.5f);
        return haveIBeenPownedRetriever.randomInfo();    
    }
}
