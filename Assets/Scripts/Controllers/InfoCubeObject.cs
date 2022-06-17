using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoCubeObject : MonoBehaviour
{
    public event EventHandler<infoCubeData> OnCubeTabbed;


    bool Used = false;

    public void CubeTapped()
    {

        OnCubeTabbed?.Invoke(this, new infoCubeData { Sender = this });

        Destroy(this.gameObject);

    }
}
