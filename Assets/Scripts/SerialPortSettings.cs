using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using System.Text;
using UnityEngine.UI;
using MyTools;

public class SerialPortSettings : MonoBehaviour
{
    [Header("串口名")] public string portName = "COM4";
    [Header("波特率")] public int baudRate = 115200;
    [Header("效验位")] public Parity parity = Parity.None;
    [Header("数据位")] public int dataBits = 8;
    [Header("停止位")] public StopBits stopBits = StopBits.One;

    private SerialPort sp = null;
    private Thread dataReceiveThread;
    private byte[] datasBytes;
    int num = 0;
    private byte[] MyBytes;

    public InputField portNameInputField;
    public InputField baudRateInputField;
    public Dropdown parityDropdown;
    public Dropdown dataBitsDropdown;
    public Dropdown stopBitsDropdown;
    public Button openButton;
    public Button closeButton;

    private void Start()
    {
        portNameInputField.onEndEdit.AddListener(GetPortName);
        baudRateInputField.onEndEdit.AddListener(GetBaudRate);
        parityDropdown.onValueChanged.AddListener(GetParity);
        dataBitsDropdown.onValueChanged.AddListener(GetDataBits);
        stopBitsDropdown.onValueChanged.AddListener(GetStopBits);
        openButton.onClick.AddListener(OnClikOpen);
        closeButton.onClick.AddListener(OnClickClose);
        MyBytes = new byte[1024];
        //testdata = Main5(test);
        //m_Button.onClick.AddListener(onClick);
    }

    private void GetPortName(string str)
    {
            portName = str;
    }
    private void GetBaudRate(string str)
    {
        baudRate = int.Parse(str);
    }
    private void GetParity(int value)
    {
        switch (value)
        {
            case 0:
                parity = Parity.None;
                break;
            case 1:
                parity = Parity.Odd;
                break;
            case 2:
                parity = Parity.Even;
                break;
        }
    }
    private void GetDataBits(int value)
    {
        switch (value)
        {
            case 0:
                dataBits = 8;
                break;
            case 1:
                dataBits = 7;
                break;
            case 2:
                dataBits = 6;
                break;
            case 3:
                dataBits = 5;
                break;
        }
    }
    private void GetStopBits(int value)
    {
        switch (value)
        {
            case 0:
                stopBits = StopBits.One;
                break;
            case 1:
                stopBits = StopBits.OnePointFive;
                break;
            case 2:
                stopBits = StopBits.Two;
                break;
            case 3:
                stopBits = StopBits.None;
                break;
        }
    }
    private void OnClikOpen()
    {
        OpenPortControl();
    }
    private void OnClickClose()
    {
        ClosePortControl();
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
        Debug.Log("串口打开成功！");
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
                            Array.Copy(datasBytes, 0, MyBytes, 1, MyBytes.Length - 1);
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

    //private string test = "3E 03 FF FF 0D 00 F1 FF F6 F4 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0D 00 F1 FF F6 F4 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0D 00 F1 FF F6 F4 3F 3E 03 FF FF 0D 00 F1 FF F6 F4 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 FF FF 0D 00 F1 FF F6 F4 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 00 00 0D 00 F1 FF F6 EF 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 00 00 0E 00 F1 FF F6 AB 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F1 FF F6 B0 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0F 00 F0 FF F6 DC 3F 3E 03 FF FF 0F 00 F0 FF F6 DC 3F 3E 03 FF FF 0F 00 F0 FF F6 DC 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0F 00 F0 FF F6 DC 3F 3E 03 FF FF 0F 00 F0 FF F6 DC 3F 3E 03 FE FF 0F 00 F0 FF F7 0D 3F 3E 03 FF FF 0F 00 F0 FF F6 DC 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 32 47 DF 64 F7 22 EA 87 DF 13 2D D6 6C 3E C1 E5 41 D6 C9 98 B3 AA 08 03 23 51 9A 8B A5 42 C2 5E AC AC 72 71 D2 26 CF A6 12 2C 11 4C D8 AF 03 C1 A9 A3 00 22 41 56 CC C2 43 06 A0 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 00 00 0E 00 F0 FF F7 3B 3F 3E 03 00 00 0E 00 F0 FF F7 3B 3F 3E 03 FF FF 0D 00 F0 FF F7 64 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 FF FF 0E 00 F0 FF F7 20 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 FF FF 00 00 F0 66 3F 3E 03 01 00 FF FF 00 00 F0 66 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 00 00 00 00 00 00 F1 93 3F 3E 03 01 00 00 00 00 00 F0 42 3F 3E 03 02 00 00 00 01 00 F1 E1 3F 3E 03 02 00 00 00 03 00 F0 81 3F 3E 03 03 00 00 00 06 00 F2 00 3F 3E 03 03 00 01 00 09 00 F6 0C 3F 3E 03 02 00 00 00 0A 00 F6 D1 3F 3E 03 02 00 00 00 0B 00 F7 41 3F 3E 03 01 00 00 00 0C 00 F5 42 3F 3E 03 02 00 00 00 0C 00 F5 71 3F 3E 03 02 00 00 00 0C 00 F5 71 3F 3E 03 04 00 01 00 0D 00 F5 7B 3F 3E 03 07 00 01 00 0D 00 F5 48 3F 3E 03 0B 00 02 00 0E 00 F5 30 3F 3E 03 0E 00 03 00 0E 00 F4 99 3F 3E 03 11 00 03 00 0F 00 F7 66 3F 3E 03 13 00 04 00 10 00 FF C0 3F 3E 03 17 00 05 00 13 00 FF 48 3F 3E 03 1A 00 05 00 16 00 FD 05 3F 3E 03 1C 00 06 00 1A 00 F8 27 3F 3E 03 1F 00 07 00 1E 00 FB 28 3F 3E 03 22 00 09 00 22 00 EC 2D 3F 3E 03 24 00 0A 00 26 00 EE CF 3F 3E 03 27 00 0C 00 2A 00 EB 74 3F 3E 03 2A 00 0E 00 2D 00 E9 E1 3F 3E 03 2E 00 10 00 30 00 E7 1D 3F 3E 03 31 00 12 00 35 00 E7 9A 3F 3E 03 34 00 15 00 38 00 E2 2B 3F 3E 03 35 00 17 00 39 00 E3 D2 3F 3E 03 37 00 19 00 39 00 E0 D8 3F 3E 03 3B 00 1B 00 3A 00 E1 5C 3F 3E 03 3F 00 1E 00 3A 00 E0 14 3F 3E 03 43 00 22 00 3B 00 E6 28 3F 3E 03 47 00 25 00 3B 00 E6 D8 3F 3E 03 4C 00 28 00 3B 00 E5 0F 3F 3E 03 50 00 2B 00 3B 00 E7 17 3F 3E 03 54 00 2F 00 3B 00 E7 A3 3F 3E 03 58 00 32 00 3C 00 E3 33 3F 3E 03 5C 00 35 00 3C 00 E3 C3 3F 3E 03 61 00 39 00 3C 00 E4 7E 3F 3E 03 65 00 3C 00 3C 00 E5 36 3F 3E 03 6A 00 40 00 3C 00 FC 59 3F 3E 03 6E 00 43 00 3B 00 FF A9 3F 3E 03 72 00 46 00 39 00 FC 59 3F 3E 03 77 00 49 00 37 00 FB 78 3F 3E 03 7B 00 4D 00 33 00 F8 44 3F 3E 03 81 00 50 00 30 00 EA 82 3F 3E 03 86 00 53 00 2E 00 E2 D1 3F 3E 03 8B 00 55 00 2B 00 E0 14 3F 3E 03 90 00 57 00 28 00 E2 B7 3F 3E 03 95 00 5A 00 27 00 E5 BE 3F 3E 03 9B 00 5C 00 26 00 E5 88 3F 3E 03 A1 00 5E 00 26 00 E1 6A 3F 3E 03 A7 00 61 00 26 00 ED 18 3F 3E 03 AC 00 63 00 25 00 ED 2B 3F 3E 03 B2 00 65 00 25 00 EE 1D 3F 3E 03 B8 00 68 00 25 00 EC 1B 3F 3E 03 BD 00 6B 00 25 00 EC 0A 3F 3E 03 C4 00 6E 00 25 00 E7 6F 3F 3E 03 C9 00 70 00 24 00 E1 CA 3F 3E 03 CF 00 73 00 23 00 E3 D8 3F 3E 03 D5 00 75 00 22 00 E0 FA 3F 3E 03 DA 00 77 00 21 00 E1 4D 3F 3E 03 E0 00 7A 00 1F 00 F6 DB 3F 3E 03 E6 00 7C 00 1D 00 F7 55 3F 3E 03 EB 00 7D 00 1C 00 F6 24 3F 3E 03 F1 00 7F 00 1B 00 F7 96 3F 3E 03 F8 00 81 00 1A 00 C7 77 3F 3E 03 FE 00 83 00 19 00 C6 59 3F 3E 03 05 01 85 00 18 00 EF 0A 3F 3E 03 0A 01 86 00 17 00 EA 41 3F 3E 03 11 01 88 00 17 00 EB 42 3F 3E 03 18 01 8A 00 16 00 EB F3 3F 3E 03 1E 01 8C 00 16 00 EB 1D 3F 3E 03 25 01 8E 00 16 00 EE 2E 3F 3E 03 2B 01 8F 00 16 00 EE FC 3F 3E 03 32 01 91 00 16 00 EA DD 3F 3E 03 38 01 93 00 16 00 EB CF 3F 3E 03 3F 01 95 00 16 00 EA F0 3F 3E 03 46 01 96 00 16 00 E1 1D 3F 3E 03 4D 01 98 00 16 00 E2 8E 3F 3E 03 54 01 9A 00 16 00 E1 3F 3F 3E 03 5C 01 9C 00 16 00 E0 FF 3F 3E 03 63 01 9F 00 16 00 E5 B4 3F 3E 03 6A 01 A1 00 16 00 E8 C5 3F 3E 03 72 01 A3 00 16 00 EA A5 3F 3E 03 7A 01 A5 00 16 00 EB 65 3F 3E 03 81 01 A6 00 15 00 FE 5A 3F 3E 03 89 01 A8 00 15 00 FD FA 3F 3E 03 90 01 AA 00 14 00 FF DB 3F 3E 03 98 01 AC 00 13 00 FC 2B 3F 3E 03 9F 01 AD 00 12 00 FD F0 3F 3E 03 A6 01 AE 00 11 00 F8 2D 3F 3E 03 AD 01 B0 00 10 00 FE EE 3F 3E 03 B5 01 B1 00 0F 00 F4 FA 3F 3E 03 BC 01 B3 00 0E 00 F4 4B 3F 3E 03 C3 01 B4 00 0E 00 FE F0 3F 3E 03 CA 01 B6 00 0E 00 FF D1 3F 3E 03 D1 01 B8 00 0E 00 FE D2 3F 3E 03 D8 01 B9 00 0E 00 FF B7 3F 3E 03 DF 01 BA 00 0E 00 FE 44 3F 3E 03 E6 01 BB 00 0E 00 FA D1 3F 3E 03 ED 01 BD 00 0E 00 FB 22 3F 3E 03 F5 01 BE 00 0E 00 F8 BE 3F 3E 03 FC 01 C0 00 0E 00 E0 0F 3F 3E 03 03 02 C1 00 0E 00 B1 FC 3F 3E 03 0A 02 C3 00 0E 00 B0 DD 3F 3E 03 11 02 C4 00 0E 00 B2 42 3F 3E 03 17 02 C5 00 0E 00 B3 D8 3F 3E 03 1E 02 C7 00 0E 00 B2 F9 3F 3E 03 25 02 C8 00 0E 00 B5 66 3F 3E 03 2C 02 CA 00 0E 00 B4 47 3F 3E 03 32 02 CB 00 0E 00 B6 05 3F 3E 03 39 02 CD 00 0E 00 B7 F6 3F 3E 03 3F 02 CE 00 0F 00 B6 44 3F 3E 03 45 02 CF 00 10 00 B4 12 3F 3E 03 4C 02 D1 00 11 00 B3 33 3F 3E 03 52 02 D3 00 12 00 B1 C5 3F 3E 03 58 02 D5 00 14 00 B2 47 3F 3E 03 5E 02 D7 00 16 00 B2 F9 3F 3E 03 64 02 D8 00 18 00 B0 D7 3F 3E 03 6A 02 DA 00 1A 00 B1 21 3F 3E 03 71 02 DC 00 1B 00 B3 D2 3F 3E 03 77 02 DE 00 1C 00 B0 3C 3F 3E 03 7D 02 E0 00 1D 00 BC EE 3F 3E 03 83 02 E2 00 1D 00 A8 88 3F 3E 03 89 02 E4 00 1E 00 A8 5A 3F 3E 03 8F 02 E6 00 1E 00 A9 84 3F 3E 03 96 02 E8 00 1F 00 A8 F5 3F 3E 03 9B 02 EA 00 1F 00 A8 50 3F 3E 03 A1 02 ED 00 20 00 E8 A4 3F 3E 03 A7 02 EE 00 21 00 BC 3C 3F 3E 03 AD 02 F1 00 23 00 BA 22 3F 3E 03 B3 02 F4 00 26 00 BA 00 3F 3E 03 B9 02 F7 00 28 00 BE 8E 3F 3E 03 BE 02 FA 00 2A 00 BC F5 3F 3E 03 C3 02 FD 00 2C 00 B4 0C 3F 3E 03 C8 02 FF 00 2D 00 B5 5F 3F 3E 03 CE 02 03 01 2D 00 D4 A9 3F 3E 03 D4 02 06 01 2E 00 D6 AF 3F 3E 03 D9 02 09 01 2E 00 D4 A6 3F 3E 03 DE 02 0D 01 2E 00 D4 21 3F 3E 03 E3 02 10 01 2E 00 D6 A0 3F 3E 03 E8 02 13 01 2F 00 D6 0F 3F 3E 03 EE 02 16 01 2F 00 D6 A5 3F 3E 03 F3 02 19 01 30 00 DE 0C 3F 3E 03 F9 02 1C 01 31 00 DF FA 3F 3E 03 FE 02 1E 01 33 00 DE 95 3F 3E 03 03 03 22 01 35 00 F9 48 3F 3E 03 09 03 25 01 38 00 FC 06 3F 3E 03 0E 03 28 01 3C 00 FD DD 3F 3E 03 12 03 2C 01 41 00 DF E1 3F 3E 03 16 03 30 01 45 00 DB 35 3F 3E 03 1A 03 34 01 49 00 DF C9 3F 3E 03 1E 03 39 01 4D 00 DE 21 3F 3E 03 21 03 3D 01 51 00 D2 DE 3F 3E 03 24 03 41 01 55 00 C9 DB 3F 3E 03 27 03 45 01 5A 00 CD 28 3F 3E 03 2A 03 49 01 5F 00 CC 35 3F 3E 03 2D 03 4D 01 65 00 DE 12 3F 3E 03 2E 03 52 01 6A 00 DC 05 3F 3E 03 30 03 56 01 70 00 D5 EB 3F 3E 03 31 03 5B 01 76 00 D5 36 3F 3E 03 32 03 5F 01 7B 00 D0 A5 3F 3E 03 33 03 64 01 81 00 9E F0 3F 3E 03 33 03 69 01 87 00 9F FC 3F 3E 03 33 03 6E 01 8B 00 9B 88 3F 3E 03 33 03 74 01 90 00 96 60 3F 3E 03 33 03 78 01 94 00 97 F0 3F 3E 03 34 03 7E 01 99 00 92 5F 3F 3E 03 32 03 82 01 9D 00 A0 A9 3F 3E 03 32 03 87 01 A2 00 B1 95 3F 3E 03 31 03 8C 01 A7 00 B0 D2 3F 3E 03 30 03 91 01 AD 00 B1 CF 3F 3E 03 2F 03 96 01 B3 00 BB 74 3F 3E 03 2E 03 9A 01 BA 00 BF A5 3F 3E 03 2B 03 9F 01 C0 00 9C 5C 3F 3E 03 29 03 A3 01 C6 00 92 4E 3F 3E 03 26 03 A7 01 CB 00 97 11 3F 3E 03 23 03 AA 01 D1 00 9E 88 3F 3E 03 20 03 AD 01 D7 00 AE 6C 3F 3E 03 1D 03 B0 01 DE 00 98 BE 3F 3E 03 1A 03 B3 01 E5 00 8A 7D 3F 3E 03 16 03 B5 01 ED 00 8D F9 3F 3E 03 12 03 B9 01 F3 00 86 8D 3F 3E 03 0D 03 BB 01 F9 00 83 FA 3F 3E 03 08 03 BD 01 FF 00 80 87 3F 3E 03 03 03 BF 01 03 01 00 84 3F 3E 03 FD 02 C1 01 04 01 32 82 3F 3E 03 F8 02 C2 01 04 01 32 93 3F 3E 03 F3 02 C4 01 05 01 32 F0 3F 3E 03 EF 02 C6 01 05 01 31 14 3F 3E 03 EA 02 C9 01 06 01 32 A5 3F 3E 03 E6 02 CB 01 06 01 33 D1 3F 3E 03 E1 02 CD 01 06 01 32 EE 3F 3E 03 DD 02 CF 01 07 01 37 FA 3F 3E 03 D7 02 D1 01 07 01 31 78 3F 3E 03 D3 02 D3 01 08 01 34 B4 3F 3E 03 CF 02 D4 01 0B 01 37 6C 3F 3E 03 CB 02 D5 01 10 01 3D E4 3F 3E 03 C6 02 D6 01 14 01 3E 7D 3F 3E 03 C1 02 D8 01 18 01 38 22 3F 3E 03 BC 02 D9 01 1C 01 31 33 3F 3E 03 B8 02 DA 01 20 01 84 F8 3F 3E 03 B2 02 DB 01 22 01 21 C5 3F 3E 03 AC 02 DC 01 23 01 22 9F 3F 3E 03 A7 02 DD 01 23 01 22 18 3F 3E 03 A2 02 DE 01 24 01 20 39 3F 3E 03 9D 02 DE 01 24 01 25 36 3F 3E 03 98 02 E0 01 24 01 28 8B 3F 3E 03 93 02 E1 01 25 01 29 9C 3F 3E 03 8E 02 E1 01 25 01 2A 11 3F 3E 03 88 02 E1 01 25 01 2A 77 3F 3E 03 83 02 E2 01 26 01 2B B8 3F 3E 03 7D 02 E3 01 26 01 3F 9A 3F 3E 03 77 02 E4 01 26 01 3E 44 3F 3E 03 72 02 E4 01 26 01 3E 11 3F 3E 03 6C 02 E5 01 26 01 3C 53 3F 3E 03 66 02 E6 01 27 01 3D 2D 3F 3E 03 60 02 E7 01 27 01 3C B7 3F 3E 03 5A 02 E7 01 27 01 39 ED 3F 3E 03 54 02 E8 01 27 01 3B D7 3F 3E 03 4E 02 E9 01 27 01 38 11 3F 3E 03 49 02 E9 01 27 01 39 A6 3F 3E 03 43 02 E9 01 27 01 39 0C 3F 3E 03 3D 02 EB 01 28 01 37 5A 3F 3E 03 38 02 EB 01 28 01 37 0F 3F 3E 03 32 02 EC 01 28 01 36 D1 3F 3E 03 2C 02 EC 01 28 01 35 6F 3F 3E 03 26 02 ED 01 28 01 34 39 3F 3E 03 20 02 ED 01 28 01 06 5C 3F 3E 03 1A 02 EE 01 28 01 31 41 3F 3E 03 15 02 EF 01 28 01 30 42 3F 3E 03 0E 02 EF 01 28 01 33 A9 3F 3E 03 08 02 F0 01 28 01 34 1B 3F 3E 03 02 02 F0 01 28 01 34 B1 3F 3E 03 FC 01 F1 01 28 01 64 93 3F 3E 03 F6 01 F1 01 29 01 65 A9 3F 3E 03 F0 01 F2 01 29 01 65 8B 3F 3E 03 EA 01 F3 01 29 01 66 4D 3F 3E 03 E4 01 F3 01 29 01 67 63 3F 3E 03 DE 01 F4 01 29 01 63 4D 3F 3E 03 D8 01 F4 01 29 01 63 2B 3F 3E 03 D1 01 F4 01 29 01 63 B2 3F 3E 03 CB 01 F5 01 29 01 60 74 3F 3E 03 C4 01 F5 01 29 01 60 8B 3F 3E 03 BE 01 F6 01 29 01 6B 55 3F 3E 03 B7 01 F7 01 29 01 6A 30 3F 3E 03 B1 01 F8 01 2A 01 69 B2 3F 3E 03 AB 01 F8 01 2A 01 6B 88 3F 3E 03 A5 01 F9 01 2A 01 6B 5A 3F 3E 03 9E 01 F9 01 2B 01 6E 41 3F 3E 03 98 01 F9 01 2C 01 6C 17 3F 3E 03 92 01 FA 01 2D 01 6D 69 3F 3E 03 8B 01 FA 01 2E 01 6F 90 3F 3E 03 85 01 FB 01 30 01 66 E2 3F 3E 03 7E 01 FB 01 31 01 72 F9 3F 3E 03 77 01 FB 01 33 01 73 00 3F 3E 03 71 01 FB 01 36 01 70 36 3F 3E 03 6A 01 FB 01 39 01 76 2D 3F 3E 03 64 01 FA 01 C9 FE 72 BF 3F 3E 03 5E 01 F9 01 CB FE 76 C1 3F 3E 03 58 01 F7 01 CC FE 76 7F 3F 3E 03 52 01 F6 01 CC FE 77 29 3F 3E 03 4C 01 F5 01 CD FE 75 43 3F 3E 03 46 01 F4 01 CD FE 74 15 3F 3E 03 40 01 F3 01 CC FE 74 97 3F 3E 03 3A 01 F2 01 CC FE 7E F1 3F 3E 03 34 01 F1 01 CB FE 7D AB 3F 3E 03 2E 01 F0 01 CA FE 7F FD 3F 3E 03 28 01 F0 01 C9 FE 7F 6B 3F 3E 03 21 01 EF 01 C8 FE 79 B6 3F 3E 03 1C 01 EF 01 C8 FE 7D 5B 3F 3E 03 15 01 EE 01 C7 FE 79 CE 3F 3E 03 0F 01 EE 01 C7 FE 7B F4 3F 3E 03 09 01 EE 01 C7 FE 7B 92 3F 3E 03 02 01 ED 01 C7 FE 7A AD 3F 3E 03 FC 00 ED 01 C7 FE 52 B3 3F 3E 03 F7 00 EC 01 C7 FE 52 34 3F 3E 03 F0 00 EC 01 C7 FE 53 83 3F 3E 03 EA 00 EB 01 C7 FE 50 CD 3F 3E 03 E3 00 EB 01 C7 FE 50 54 3F 3E 03 DD 00 EA 01 C7 FE 55 76 3F 3E 03 D6 00 EA 01 C7 FE 54 0D 3F 3E 03 D0 00 E9 01 C7 FE 54 2F 3F 3E 03 C9 00 E8 01 C7 FE 57 DA 3F 3E 03 C3 00 E8 01 C7 FE 57 70 3F 3E 03 BD 00 E8 01 C7 FE 5D 6E 3F 3E 03 B6 00 E7 01 C7 FE 5F 01 3F 3E 03 B0 00 E7 01 C7 FE 5F 67 3F 3E 03 AA 00 E6 01 C7 FE 5C A1 3F 3E 03 A4 00 E6 01 C7 FE 5D 8F 3F 3E 03 9C 00 E5 01 C7 FE 59 73 3F 3E 03 96 00 E4 01 C7 FE 58 25 3F 3E 03 8F 00 E4 01 C7 FE 5A 2C 3F 3E 03 89 00 E3 01 C7 FE 5B 3E 3F 3E 03 82 00 E3 01 C7 FE 5A 45 3F 3E 03 7C 00 E2 01 C7 FE 4E 67 3F 3E 03 75 00 E1 01 C7 FE 4E BA 3F 3E 03 6E 00 E0 01 C7 FE 4C AD 3F 3E 03 68 00 E0 01 C7 FE 4C CB 3F 3E 03 61 00 DF 01 C8 FE 45 B6 3F 3E 03 5B 00 DE 01 C8 FE 41 10 3F 3E 03 54 00 DE 01 C8 FE 41 EF 3F 3E 03 4D 00 DD 01 C8 FE 43 A2 3F 3E 03 47 00 DC 01 C8 FE 42 F4 3F 3E 03 40 00 DC 01 C8 FE 43 43 3F 3E 03 3A 00 DB 01 C8 FE 49 AD 3F 3E 03 33 00 DA 01 C8 FE 48 C8 3F 3E 03 2D 00 D9 01 C8 FE 4B 32 3F 3E 03 26 00 D9 01 C8 FE 4A 49 3F 3E 03 20 00 D8 01 C8 FE 79 D0 3F 3E 03 19 00 D8 01 C8 FE 4E BA 3F 3E 03 13 00 D7 01 C8 FE 4D 04 3F 3E 03 0D 00 D7 01 C9 FE 4F 2A 3F 3E 03 07 00 D6 01 C9 FE 4E 7C 3F 3E 03 00 00 D5 01 C9 FE 4F 8F 3F 3E 03 FB FF D5 01 C9 FE 4E 10 3F 3E 03 F5 FF D4 01 C9 FE 4E C2 3F 3E 03 EF FF D3 01 C9 FE 4D 8C 3F 3E 03 E9 FF D3 01 C9 FE 4D EA 3F 3E 03 E3 FF D2 01 C9 FE 4C BC 3F 3E 03 DD FF D2 01 C9 FE 48 62 3F 3E 03 D7 FF D1 01 CA FE 48 7C 3F 3E 03 D2 FF D1 01 CA FE 48 29 3F 3E 03 CD FF D0 01 CA FE 4B BA 3F 3E 03 C7 FF CF 01 CA FE 4C C4 3F 3E 03 C2 FF CF 01 CA FE 4C 91 3F 3E 03 BC FF CE 01 CA FE 47 73 3F 3E 03 B7 FF CD 01 CA FE 46 4C 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B1 FF CD 01 CA FE 46 2A 3F 3E 03 B1 FF CC 01 CA FE 47 D6 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B3 FF CC 01 CA FE 46 34 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CC 01 CA FE 47 E5 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B2 FF CD 01 CA FE 46 19 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F 3E 03 B3 FF CD 01 CA FE 47 C8 3F ";
    //private byte[] testdata = null;
    //byte[] data = new byte[11];
    //public int c = 0;
    //static byte[] Main5(string test)
    //{
    //    if (string.IsNullOrEmpty(test))
    //    {
    //        return new byte[0];
    //    }
    //    //将" "删除，即用""替代
    //    string s = test.Replace(" ", "");
    //    int count = s.Length / 2;
    //    //int count = str.Length / 2;
    //    var result = new byte[count];
    //    for (int i = 0; i < count; i++)
    //    {
    //        var tempBytes = Byte.Parse(s.Substring(2 * i, 2), System.Globalization.NumberStyles.HexNumber);
    //        result[i] = tempBytes;
    //    }
    //    return result;
    //}

    private void Update()
    {


        if ((sp != null && sp.IsOpen))
        {
            //从数组中抽取数据帧，每次10个为一个新数组
            for (int i = 0; i < MyBytes.Length - 10; i++)
            {
                byte[] newDate = new byte[11];
                Array.Copy(MyBytes, i, newDate, 0, 11);
                //检验3次，第4次提取数据，共四次
                for (int j = 0; j < 4; j++)
                {
                    Fun1(newDate);
                }
            }
            SendMsg(DataSaver.Instance.s);
        }


            //if ((sp != null && sp.IsOpen))
            //{
            //    if(c<8360)
            //    {
            //        Array.Copy(testdata, c, data, 0, 11);
            //        c = c + 3;
            //    }
            //    //从数组中抽取数据帧，每次10个为一个新数组
            //    for (int i = 0; i < data.Length - 10; i++)
            //    {
            //        byte[] newDate = new byte[11];
            //        Array.Copy(data, i, newDate, 0, 11);
            //        //检验3次，第4次提取数据，共四次
            //        for (int j = 0; j < 4; j++)
            //        {
            //            Fun1(newDate);
            //        }
            //    }
            //}
        }

    //private void OnGUI()
    //{
    //    if (GUILayout.Button("接受数据"))
    //    {
    //        for (int i = 0; i < MyBytes.Length - 9; i++)
    //        {
    //            byte[] newDate = new byte[10];
    //            Array.Copy(MyBytes, i, newDate, 0, 10);
    //            //检验3次，第4次提取数据，共四次
    //            for (int j = 0; j < 4; j++)
    //            {
    //                Fun1(newDate);
    //                //if(!isRight)
    //                //{
    //                //    isRight = true;
    //                //    j = 4;
    //                //}
    //            }
    //        }
    //    }
    //}


    private float x, y, yaw,dataNum;

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
                if (recv[10] == 0x3f)
                    frameState = stateCRC;
                //else
                //    isRight = false;
                break;
            //CRCmodbus校验(第7、8位）
            case stateCRC:
                byte[] bytes = new byte[10];
                Array.Copy(recv, 0, bytes, 0, 10);
                if (Crc.IsCrcOK(bytes))
                {
                    DataSaver.Instance.data = recv;
                    frameState = analysis;
                }

                //else
                //    isRight = false;
                break;
            //提取数据
            case analysis:
                //cars[0].transform.localPosition = new Vector3(recv[1], cars[0].transform.localPosition.y, recv[3]);
                if(recv[3]>=128)
                {
                    recv[2] = (byte)~recv[2];
                    recv[3] = (byte)~recv[3];
                    x = -(recv[2]+1 + recv[3] * (256)) / 100f;
                }
                else
                {
                    x = (recv[2] + recv[3] * (256)) / 100f;
                }

                if (recv[5] >= 128)
                {
                    recv[4] = (byte)~recv[4];
                    recv[5] = (byte)~recv[5];
                    y = -(recv[4]+1 + recv[5] * (256)) / 100f;
                }
                else
                {
                    y = (recv[4] + recv[5] * (256)) / 100f;
                }

                if(recv[7] >= 128)
                {
                    recv[6] = (byte)~recv[6];
                    recv[7] = (byte)~recv[7];
                    yaw = -(recv[6] + 1 + recv[7] * (256)) / 100f;
                }
                else
                {
                    yaw = (recv[6]  + recv[7] * (256)) / 100f;
                }

                dataNum = recv[1];

                DataSaver.Instance.x = x;
                DataSaver.Instance.y = y;
                DataSaver.Instance.yaw = yaw / 3.14f * 180;
                DataSaver.Instance.dataNum = Convert.ToInt32(dataNum);

                //targetTransform.localPosition = new Vector3(x, 0.1f, y);

                //Debug.Log(recv[5] + recv[6]);
                //Debug.Log("x坐标：" + x);
                //Debug.Log("y坐标：" + y);
                //Debug.Log("俯仰角：" + yaw);
                frameState = stateHead;
                break;
        }
    }


    public void SendData(byte[] data)
    {
        if (sp != null)
        {
            //text.text = data[0].ToString();
            sp.Write(data, 0, data.Length);
        }

    }

    private void SendMsg(string str)
    {
        string msg = str;
        byte[] message = Converter.StringToBytes(msg);
        SendData(message);
    }


    //private void OnGUI()
    //{
    //    string test = "4e010203040506074f";
    //    if (GUILayout.Button("Send Test"))
    //    {
    //        SendMsg(DataSaver.Instance.s);
    //    }
    //}


}