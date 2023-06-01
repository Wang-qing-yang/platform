using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyTools;

/// <summary>
///生成所需的命令帧
///</summary>
public class GenerateCMD : MonoBehaviour
{
    public float linearVelocity;
    public float angularVelocity;
    public float number;
    public string cmd;

    private void Generata(float v,float w,float num)
    {
        v = v * 100;
        int v_ldata = Convert.ToInt32(v) & 0xff;
        int v_hdata = (Convert.ToInt32(v) >> 8) & 0xff;

        w = w * 100;
        int w_ldata = Convert.ToInt32(w) & 0xff;
        int w_hdata = (Convert.ToInt32(w) >> 8) & 0xff;

        //Debug.Log(ConvertHex(Convert.ToString(v_hdata)));
        cmd = "4e" + Converter.ConvertHex(Convert.ToString(num)) 
                           + Converter.ConvertHex(Convert.ToString(v_ldata)) + Converter.ConvertHex(Convert.ToString(v_hdata)) 
                           + Converter.ConvertHex(Convert.ToString(w_ldata)) + Converter.ConvertHex(Convert.ToString(w_hdata));
        //生成CRC16modbus校验
        byte[] crcTest = Converter.StringToBytes(cmd);
        crcTest = Crc.GetCRCDatas(crcTest);
        //添加帧尾
        cmd = Converter.byteToHexStr(crcTest) + "4F";
        DataSaver.Instance.s = cmd;
    }

    private void Update()
    {
        linearVelocity = DataSaver.Instance.v;
        angularVelocity = DataSaver.Instance.w;

        //linearVelocity = (float)Math.Round((double)DataSaver.Instance.moveSpeed, 2);
        //angularVelocity = (float)Math.Round((double)(DataSaver.Instance.rotateSpeed / 180f * 3.14f)*2, 2);

        //number = DataSaver.Instance.num;
        Generata(linearVelocity, angularVelocity, number);
    }


}
