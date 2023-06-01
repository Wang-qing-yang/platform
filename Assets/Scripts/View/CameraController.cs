using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
///</summary>
public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    private void Start()
    {
        offset = this.transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }


}
