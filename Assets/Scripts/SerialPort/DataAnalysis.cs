using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CRC;


/// <summary>
///数据帧解析
///</summary>
public class DataAnalysis : MonoBehaviour
{
    public int frameState = 0;
    private const int stateHead = 0;
    private const int stateTail = 1;
    private const int stateCRC = 2;
    private const int analysis = 3;
    private byte[] date = { 0x31,0x32,0x31,0x3e,0x32,0x33,0x34,0x35,0x36,0xe4,0x09,0xd8,0x3f };

    private void Update()
    {
        //从数组中抽取数据帧，每次10个为一个新数组
        for (int i = 0; i < date.Length-9; i++)
        {
            byte[] newDate = new byte[10];
            Array.Copy(date, i, newDate, 0, 10);
            //检验3次，第4次提取数据，共四次
            for (int j = 0; j < 4; j++)
            {
                Fun1(newDate);
            }
        }
    }
/// <summary>
/// 判断是否为正确数据帧，并读出数据
/// </summary>
/// <param name="recv">待检测数据（10字节）</param>
    private void Fun1(byte[] recv)
    {
        //判断帧头？=0x3e
        switch(frameState)
        {
            case stateHead:
                if (recv[0] == 0x3e)
                    frameState = stateTail;
                break;
        //判断帧尾？=0x3f
              case stateTail:
                if(recv[9]==0x3f)
                    frameState = stateCRC;
                break;
        //CRCmodbus校验(第7、8位）
            case stateCRC:
                byte[] bytes = new byte[8];
                Array.Copy(recv, 1, bytes, 0, 8);
                if (Crc.IsCrcOK(bytes))
                    frameState = analysis;
                break;
        //提取数据
            case analysis:
                Debug.Log("x坐标：" + recv[1] + recv[2]);
                Debug.Log("y坐标：" + recv[3] + recv[4]);
                Debug.Log("俯仰角：" + recv[5] + recv[6]);
                frameState = stateHead;
                break;
        }
    }




    
   

}
