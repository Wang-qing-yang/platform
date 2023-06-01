using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
public class ChuanKOU : MonoBehaviour
{
    //状态
    private int CONDITION;
    //校验
    private const int VERIFY = 0;
    //校验长度
    private const int VERIFY_LG = 1;
    //解析
    private const int ANALYSIS = 2;


    //可用串口缓存

    private string[] sps;

    //串口号缓存

    private string protName;

    //波特率

    private int baudRate = 9600;

    //开关串口判断

    private bool close =false;

    //实例化串口

    public SerialPort serialport = new SerialPort();
    void Start()
    {
        try
        {
            sps = SerialPort.GetPortNames();
            ; protName = sps[0];
            Debug.Log("串口号" + protName);
            serialport = new SerialPort(protName, baudRate,Parity.None,8,StopBits.One);
            serialport.Open();
            Debug.Log("打开");
            close=true;
        }
        catch
        {
            close = false;
            serialport.Close();
            Debug.Log("关闭");
        }
    }
    public void analysis_data()
    {
        byte[] recv = new byte[1024];//定长13位数据
        int recvLen = serialport.Read(recv, 0, recv.Length);
        Debug.Log(recv[0]);
        
        if (recvLen > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                parseByte(recv);
            }
        }
    }
    //数据解析
    private void parseByte(byte[] recv)
    {
        switch (CONDITION)
        {
            case VERIFY:
                if (recv[0] == 0xAA && recv[1] == 0xAA)
                {
                    CONDITION = VERIFY_LG;
                }
                break;
            case VERIFY_LG:
                if (recv[12] == 0xDD)
                {
                    CONDITION = ANALYSIS;
                }
                break;
            case ANALYSIS:
                Debug.Log("Att" + recv[3]);
                Debug.Log("Med" + recv[4]);
                CONDITION = VERIFY;
                break;
        }

    }
    void Update()
    {
        if (close)
        {
            analysis_data();
        }
    }

    private void OnDestroy()
    {
        //关闭串口
        serialport.Close();
    }
}