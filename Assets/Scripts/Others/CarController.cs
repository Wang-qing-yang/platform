using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///控制小车移动
///</summary>
[RequireComponent(typeof (Rigidbody))]
public class CarController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float moveSpeed=0.1f;
    public float speed = 50;
    private Transform carTransform;
    public Transform target;
    public bool workMode;

    private float hor, ver;


    protected virtual void Awake()
    {
        carTransform = GetComponent<Transform>();
    }

    private void Update()
    {
            if(!workMode&&target!=null)
            {
                carTransform.localPosition = new Vector3(Mathf.Lerp(carTransform.localPosition.x, target.localPosition.x, moveSpeed * Time.deltaTime),
                                                                                     Mathf.Lerp(carTransform.localPosition.y, target.localPosition.y, moveSpeed * Time.deltaTime),
                                                                                     Mathf.Lerp(carTransform.localPosition.z, target.localPosition.z, moveSpeed * Time.deltaTime));
                if ((carTransform.localPosition - target.localPosition).sqrMagnitude < 0.1)
                    return;
            } 
            else
            {
                hor = Input.GetAxis("Horizontal");
                ver = Input.GetAxis("Vertical");
                Vector3 targetPosition = new Vector3(hor, 0, ver);
            }
    }

}
