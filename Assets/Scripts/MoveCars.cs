using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///键盘WASD控制无人车线速度与角速度
///</summary>
public class MoveCars : MonoBehaviour
{

    private float hor, ver;
    public float linearVelocity = 0.3f;
    public float angularVelocity = 0.1f;

    private void Update()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");

        DataSaver.Instance.v = ver* linearVelocity;
        DataSaver.Instance.w = -hor * angularVelocity;
    }

}
