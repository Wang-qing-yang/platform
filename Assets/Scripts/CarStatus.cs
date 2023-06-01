using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
///显示Unity中小车坐标（x,z）、姿态（偏航角）、线速度（v）与角速度（w）
///</summary>
public class CarStatus : MonoBehaviour
{
    public Text text01;
    public Text text02;
    public Text text03;
    private Text currentText;

    private GameObject[] cars;//无人车

    private Vector3[] lastPosition= new Vector3[3];
    private Vector3 toward;
    public float result;
    public float speed;

    private float r;
    public float rotate;
    private Vector3[] lastAngle =new Vector3[3];

    private void Awake()
    {
        cars = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Update()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            switch (i)
            {
                case 0:
                    currentText = text01;
                    break;
                case 1:
                    currentText = text02;
                    break;
                case 2:
                    currentText = text03;
                    break;
            }
            toward = cars[i].transform.parent.position - lastPosition[i];
            result = Vector3.Dot(cars[i].transform.parent.forward, toward);
            result = result < 0 ? -1 : 1;
            speed = toward.magnitude / Time.deltaTime * result;

            r = cars[i].transform.parent.eulerAngles.y - lastAngle[i].y;
            rotate = -r / Time.deltaTime;
            //更新状态
            lastPosition[i] = cars[i].transform.parent.position;
            lastAngle[i] = cars[i].transform.parent.eulerAngles;

            if (currentText != null)
            {
                //Debug.Log(currentText);
                currentText.text = "   编号：0" + (i+1) + "\n" + " X坐标：" + cars[i].transform.parent.position.x.ToString("0.00") + "\n" + " Y坐标：" + cars[i].transform.parent.position.z.ToString("0.00") + "\n" + "偏航角：" + (-(cars[i].transform.parent.eulerAngles.y-90)).ToString("0.00") + "\n"
                    + "线速度：" + speed.ToString("0.00") + "\n" + "角速度：" + rotate.ToString("0.00");
            }
        }
    }
}
