using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using Random = UnityEngine.Random;

public class InfoCubeManger : MonoBehaviour
{

    public delegate void StateAction();
    public event StateAction FinsihedCubes;


    [SerializeField]HaveIBeenPownedRetriever retriever;
    [SerializeField] Texture2D Image;
    [SerializeField] List<InfoCubeObject> cubes;
    [SerializeField] Text text;

    [SerializeField] int NeedCubes = 2;

    int collected = 0;


    bool isImageTaken;
    int Total = 1;
    List<string> allcontent;
    List<string> UnlockedContent;

    public void LoadCubes()
    {


        GameObject[] cubeslist = GameObject.FindGameObjectsWithTag("InfoCube");
        cubes = new List<InfoCubeObject>();


        foreach (GameObject gameObject in cubeslist)
        {
            InfoCubeObject C;

            if (gameObject.TryGetComponent<InfoCubeObject>(out C) == true)
            {
                cubes.Add(C);
            }
        }





        foreach (InfoCubeObject item in cubes)
        {

            item.OnCubeTabbed += NextUnlock;

        }

        try
        {

            allcontent = new List<string>();

  


            allcontent.AddRange(retriever.GetAll());
            Total = cubes.Count;

            if (NeedCubes > cubes.Count)
            {
                NeedCubes = cubes.Count;
            }
            collected = 0;
            string Message = collected + "/" + NeedCubes;

            text.text = Message;



        }
        catch (Exception ex)
        {
            //text.text = Saves.SaveGameContoller.getMail();
            Debug.Log("ERROR: " + ex);
        }
    }

    private void NextUnlock(object sender, infoCubeData e)
    {
        collected++;

        int remaining = cubes.Count;

        /*
        if (remaining > 1)
        {
            int total = allcontent.Count;
            int Todo = Random.Range(0, total - remaining);

            for (int i = 0; i < Todo; i++)
            {
                UnlockedContent.Add(allcontent[0]);
                allcontent.RemoveAt(0);
            }

        }
        else if (remaining < 1)
        {
            return;
        }
        else
        {
            for (int i = 0; i < allcontent.Count; i++)
            {
                UnlockedContent.Add(allcontent[0]);
                allcontent.RemoveAt(0);

                isImageTaken = true;
            }
        }
        */

            cubes.Remove(e.Sender);
        e.Sender.OnCubeTabbed -= NextUnlock;

        string Message = collected + "/" + NeedCubes;

        text.text = Message;

        if (collected >= NeedCubes)
        {
            try
            {
                text.text = "";

                FinsihedCubes();
               
            }
            catch (Exception O)
            {
                text.text = O.Message;
                
            }
            

        }


    }

    public void EndViaDebug()
    {
        text.text = "";
    }

}



public class infoCubeData : EventArgs
{
    public InfoCubeObject Sender;
}
