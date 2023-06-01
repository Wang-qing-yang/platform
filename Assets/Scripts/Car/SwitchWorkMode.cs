using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
///</summary>
public class SwitchWorkMode : MonoBehaviour
{
    public Toggle myToggle;
    public Toggle myToggle1;
    public Toggle myToggle2;
    //小车数组
    private GameObject[] cars;
    private int count;

    public GameObject go;
    public float speed =5;
    public float moveSpeed = 2;
    public float rotateSpeed = 20;
    public Transform target;
    [Range(0f, 2f)]
    public int workMode=0;


    private float hor, ver;

    private void Awake()
    {
        cars = GameObject.FindGameObjectsWithTag("Player");
      
    }

    private void Start()
    {
        myToggle.onValueChanged.AddListener((bool isOn) => { OnToggleClick(myToggle, isOn); });
        myToggle1.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(myToggle, isOn); });
        myToggle2.onValueChanged.AddListener((bool isOn) => { OnToggleClick2(myToggle, isOn); });
    }

    private void Update()
    {
        if (workMode==2 && target != null)
        {
            go.active = true;
            //carTransform.localPosition = new Vector3(Mathf.Lerp(carTransform.localPosition.x, target.localPosition.x, speed * Time.deltaTime),
            //                                                                     Mathf.Lerp(carTransform.localPosition.y, target.localPosition.y, speed * Time.deltaTime),
            //                                                                     Mathf.Lerp(carTransform.localPosition.z, target.localPosition.z, speed * Time.deltaTime));
            if ((cars[0] .transform.parent.position - target.position).sqrMagnitude > 0.1f)
            {

                float tempAngle = Vector3.Angle(cars[0].transform.parent.forward, target.position - cars[0].transform.parent.position);//自身和目标的夹角
                cars[0].transform.parent.rotation = Quaternion.Lerp(cars[0].transform.parent.rotation, Quaternion.LookRotation(target.position - cars[0].transform.parent.position), 0.1f);
                //cars[0].transform.parent.right = Vector3.Lerp(cars[0].transform.parent.right, target.position - cars[0].transform.parent.position, 0.1f);


                Debug.Log(tempAngle);

                if (tempAngle <= 1f)//是否需要旋转到一定角度在进行移动操作
                {
                    //cars[0].transform.parent.position = Vector3.MoveTowards(cars[0].transform.parent.position, target.position, 0.2f);
                    cars[0].transform.parent.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
                }
            }
            else
            {
                cars[0].transform.parent.position = target.position;
                //carTransform.rotation = Quaternion.LookRotation(target.position - carTransform.position);
                return;
            }

        
        }

        else if (workMode==1)
        {
            
            go.active = false;
            hor = Input.GetAxis("Horizontal");
            ver = Input.GetAxis("Vertical");
            cars[0].transform.parent.Translate(Vector3.forward* ver * Time.deltaTime * moveSpeed);
            cars[0].transform.parent.Rotate(transform.up * hor * Time.deltaTime * rotateSpeed);
            //Debug.Log(cars[0].GetComponent<Rigidbody>().velocity.magnitude);
            //Vector3 targetPosition = new Vector3(hor, 0, ver);
            //carTransform.Translate(targetPosition * speed * Time.deltaTime);
        }
        else if(workMode==3)
        {
            Debug.Log(cars[0].name);
            go.active = true;
            cars[0].transform.parent.localPosition = new Vector3(DataSaver.Instance.x,0.15f,DataSaver.Instance.y);
            cars[0].transform.parent.localEulerAngles = new Vector3(0, -DataSaver.Instance.yaw/3.14f*180, 0);
            Debug.Log(DataSaver.Instance.x);

        }
        else
            return;
    }

    private void OnToggleClick(Toggle toggle, bool isOn)
        {
            //if(isOn)
            //    workMode= 1;
            //else
            //workMode = 0;
            workMode=isOn ? 1 : 0;
        }

    private void OnToggleClick1(Toggle toggle, bool isOn)
    {
        //if (isOn)
        //    workMode = 2;
        //else
        //    workMode = 0;

        workMode =isOn ? 2 : 0;
    }

    private void OnToggleClick2(Toggle toggle, bool isOn)
    {
        //if (isOn)
        //    workMode = 2;
        //else
        //    workMode = 0;

        workMode = isOn ? 3 : 0;
    }
}
