using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

/// <summary>
///
///</summary>
public class DataSaver : MonoBehaviour
{
    public static DataSaver Instance { get; private set; }
    //串口
    public string portName { get; set; }="COM4";
    public int baudRate { get; set; } = 115200;
    public int databits { get; set; } = 8;
    public StopBits stopbits { get; set; } = StopBits.One;
    public Parity parity { get; set; } = Parity.None;
    //场景
    public Vector3 SceneSize { get; set; } = new Vector3(1,1,1);
    public Transform MouseObjects { get; set; } = null;

    //无人车位置与姿态
    public byte[] data = null;
    public float x=0;//无人车X坐标
    public float y=0;//无人车Y坐标
    public float yaw=0;//无人车偏航角
    public int dataNum = 0;//无人车编号

    public int count = 0;

    //命令帧
    public string s = "";    //s:命令帧
    public float v = 0;    //v:线速度
    public float w = 0;    //w:角速度
    public float cmdNum = 0;    //num:编号

    //状态监控
    public float moveSpeed;//速度
    public float rotateSpeed;//角速度

    //ROS-OdometryMsg

    //ROS-Vector3Msg



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
