using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using System.Text;
using CRC;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Port : MonoBehaviour
{
    [Header("串口名")] public string portName = "COM4";
    [Header("波特率")] public int baudRate = 115200;
    [Header("效验位")] public Parity parity = Parity.None;
    [Header("数据位")] public int dataBits = 8;
    [Header("停止位")] public StopBits stopBits = StopBits.One;

    private SerialPort sp = null;
    private Thread dataReceiveThread;
    //private Global global;
    private byte[] datasBytes;
    int num = 0;
    //private string OneString;
    //private string OtherString;
    private byte[] MyBytes;
    private GameObject dataSaver;

    //小车数组
    private GameObject[] cars;
    private GameObject[] obstacles;
    private int count;

    //public Button m_Button;

    private void Awake()
    {
        //carTransform =this.transform.GetChild(0).GetComponent<Transform>();
        dataSaver = GameObject.Find("DataSaver");
        cars = GameObject.FindGameObjectsWithTag("Player");
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    }

    private void Start()
    {
        //global = Global.Instance;
        portName = dataSaver.GetComponent<DataSaver>().portName;
        baudRate = dataSaver.GetComponent<DataSaver>().baudRate;
        dataBits = DataSaver.Instance.databits;
        stopBits = DataSaver.Instance.stopbits;
        parity = DataSaver.Instance.parity;

        OpenPortControl();
        MyBytes = new byte[1024];

        //m_Button.onClick.AddListener(onClick);
    }

    //public void onClick()
    //{
    //    ClosePortControl();
    //    for (int i = 0; i < cars.Length; i++)
    //    {
    //        GameObject.Destroy(cars[i]);
    //    }
    //    for (int i = 0; i < obstacles.Length; i++)
    //    {
    //        GameObject.Destroy(obstacles[i]);
    //    }
    //    DataSaver.Instance.flag = true;
    //    DataSaver.Instance.flag2 = true;
    //    SceneManager.LoadScene("Homepage");
    //}
    /// <summary>
    /// 开启串口
    /// </summary>
    public void OpenPortControl()
    {
        sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        //串口初始化
        if (!sp.IsOpen)
        {
            sp.Open();
        }
        dataReceiveThread = new Thread(ReceiveData);//该线程用于接收串口数据 
        dataReceiveThread.Start();
    }
    /// <summary>
    /// 关闭串口
    /// </summary>
    public void ClosePortControl()
    {
        if (sp != null && sp.IsOpen)
        {
            sp.Close();//关闭串口
            sp.Dispose();//将串口从内存中释放掉
        }
    }

    private void ReceiveData()
    {
        int bytesToRead = 0;
        while (true)
        {
            if (sp != null && sp.IsOpen)
            {
                try
                {
                    datasBytes = new byte[1024];
                    //从串口缓冲区读取数据
                    bytesToRead = sp.Read(datasBytes, 0, datasBytes.Length);
                    if (bytesToRead == 0)
                    {
                        continue;
                    }
                    else
                    {
                        //string strbytes = Encoding.UTF8.GetString(datasBytes);
                        num++;
                        if (num == 1)
                        {
                            //OneString = strbytes[0].ToString();
                            Array.Copy(datasBytes, 0, MyBytes, 0, 1);
                        }
                        else if (num == 2)
                        {
                            //OtherString = OneString + strbytes;
                            Array.Copy(datasBytes, 0, MyBytes, 1, MyBytes.Length-1);
                            num = 0;
                            //Debug.Log(OtherString);
                            //MyBytes = Encoding.UTF8.GetBytes(OtherString);
                            //Debug.Log(MyBytes[0]);
                            //Debug.Log(MyBytes[7]);
                            //Debug.Log(MyBytes[8]);
                        }
                        //Debug.Log(strbytes);
                    }

                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
            Thread.Sleep(100);
        }
    }

    void OnApplicationQuit()
    {
        ClosePortControl();
    }

    public int frameState = 0;
    private const int stateHead = 0;
    private const int stateTail = 1;
    private const int stateCRC = 2;
    private const int analysis = 3;
    private bool isRight = true;
    //private byte[] date = { 0x31, 0x32, 0x31, 0x3e, 0x32, 0x33, 0x34, 0x35, 0x36, 0xe4, 0x09, 0xd8, 0x3f };

    //private void Update()
    //{
    //    //从数组中抽取数据帧，每次10个为一个新数组
    //    for (int i = 0; i < MyBytes.Length - 9; i++)
    //    {
    //        byte[] newDate = new byte[10];
    //        Array.Copy(MyBytes, i, newDate, 0, 10);
    //        //检验3次，第4次提取数据，共四次
    //        for (int j = 0; j < 4; j++)
    //        {
    //            Fun1(newDate);
    //        }
    //    }
    //}

    private void OnGUI()
    {
        if(GUILayout.Button("接受数据"))
        {
            for (int i = 0; i < MyBytes.Length-9; i++)
            {
                byte[] newDate = new byte[10];
                Array.Copy(MyBytes, i, newDate, 0, 10);
                //检验3次，第4次提取数据，共四次
                for (int j = 0; j < 4; j++)
                {
                    Fun1(newDate);
                    //if(!isRight)
                    //{
                    //    isRight = true;
                    //    j = 4;
                    //}
                }
            }
        }
    }


    private float x, y, yaw;

    /// <summary>
    /// 判断是否为正确数据帧，并读出数据
    /// </summary>
    /// <param name="recv">待检测数据（10字节）</param>
    private void Fun1(byte[] recv)
    {
        //判断帧头？=0x3e
        switch (frameState)
        {
            case stateHead:
                if (recv[0] == 0x3e)
                    frameState = stateTail;
                //else
                //    isRight=false;
                break;
            //判断帧尾？=0x3f
            case stateTail:
                if (recv[9] == 0x3f)
                    frameState = stateCRC;
                //else
                //    isRight = false;
                break;
            //CRCmodbus校验(第7、8位）
            case stateCRC:
                byte[] bytes = new byte[8];
                Array.Copy(recv, 1, bytes, 0, 8);
                if (Crc.IsCrcOK(bytes))
                    frameState = analysis;
                //else
                //    isRight = false;
                break;
            //提取数据
            case analysis:

                cars[0].transform.localPosition =new Vector3 (recv[1], 0.2f, recv[3]);

                x = (recv[1] + recv[2] * (256)) / 100f;
                y = (recv[3] + recv[4] * (256)) / 100f;
                yaw = (recv[5] + recv[6] * (256)) / 100f;
                Debug.Log("x坐标：" + x);
                Debug.Log("y坐标：" + y);
                Debug.Log("俯仰角：" + yaw);
                frameState = stateHead;
                break;
        }
    }
}