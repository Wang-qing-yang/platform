using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
///</summary>
public class Show : MonoBehaviour
{
    private Vector3 lastPosition;
    private Vector3 toward;
    public float result;
    public float speed;

    private float r;
    public float rotate;
    private Vector3 lastAngle;

    private void Update()
    {
        toward = transform.position- lastPosition;
        result = Vector3.Dot(transform.forward, toward);
        result = result < 0 ? -1 : 1;
        speed = toward.magnitude / Time.deltaTime * result;

        r = transform.eulerAngles.y - lastAngle.y;
        rotate = r/Time.deltaTime;
        //更新状态
        lastPosition = transform.position;
        lastAngle = transform.eulerAngles;

    }


}
