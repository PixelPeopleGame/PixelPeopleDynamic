using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCamera : MonoBehaviour
{
   // private AndroidJavaObject plugin = null;

    // Start is called before the first frame update
    void Awake()
    {
        //plugin = new AndroidJavaClass("com.MadLab.PixelPeopleRemastered.UnityAndroidVibrator").CallStatic<AndroidJavaObject>("instance");
        Handheld.Vibrate();
    }

    public void VibrateForGivenDuration(int DurationInMilliseconds)
    {
       // plugin.Call("VibrateForGivenDuration", DurationInMilliseconds);

    }
    private void OnCollisionEnter(Collision collision)
    {
        Handheld.Vibrate();
    }
    private void OnTriggerEnter(Collider other)
    {
        /* if (other.gameObject.tag == "Maze")
         {*/
        Handheld.Vibrate();
        //}
    }

}
