using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using System.Text;

public class PortController : MonoBehaviour
{
    [Header("串口名")] public string portName = "COM3";
    [Header("波特率")] public int baudRate = 9600;
    [Header("效验位")] public Parity parity = Parity.None;
    [Header("数据位")] public int dataBits = 8;
    [Header("停止位")] public StopBits stopBits = StopBits.One;

    private SerialPort sp = null;
    private Thread dataReceiveThread;
    //private Global global;
    private byte[] datasBytes;
    int i = 0;
    private string OneString;
    private string OtherString;
    private byte[] MyBytes;

    private void Start()
    {
        //global = Global.Instance;
        OpenPortControl();
    }
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
                    bytesToRead = sp.Read(datasBytes, 0, datasBytes.Length);
                    if (bytesToRead == 0)
                    {
                        continue;
                    }
                    else
                    {
                        string strbytes = Encoding.Default.GetString(datasBytes);
                        i++;
                        if (i == 1)
                        {
                            OneString = strbytes[0].ToString();
                        }
                        else if (i == 2)
                        {
                            OtherString = OneString + strbytes;
                            i = 0;
                            Debug.Log(OtherString);
                            MyBytes=Encoding.ASCII.GetBytes(OtherString);

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
}
