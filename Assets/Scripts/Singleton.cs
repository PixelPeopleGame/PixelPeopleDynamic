using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    /// <summary>
    /// Need to be called before base.Awake()
    /// </summary>
    protected bool _destroyOnLoad = false;

    public static bool IsInited => instance != null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    //Debug.Log("Instance is stil null");
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    obj.hideFlags = HideFlags.DontSave;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null || instance == this)
        {
            instance = this as T;
            if (!_destroyOnLoad)
            {
                DontDestroyOnLoad(this);
                // Debug.Log("Dont Destroy on load");
            }
            //Debug.Log("Set Instance");
        }
        else if (instance != this)
        {
            // Debug.Log("Destroy it " + instance.gameObject.name);
            DestroyImmediate(gameObject);
            return;
        }
    }

    public virtual void OnApplicationQuit()
    {
        // Debug.Log("Set null");
        instance = null;
    }
}