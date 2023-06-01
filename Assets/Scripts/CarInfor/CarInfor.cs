using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
///显示小车坐标（x,z）与姿态（偏航角）
///</summary>
public class CarInfor : MonoBehaviour
{
    public Text text01;
    public Text text02;
    public Text text03;
    public Text dataText;
    private Text currentText;

    private void Update()
    {
        if(DataSaver.Instance.data!=null)
        dataText.text = "当前数据帧："+MyTools.Converter.byteToHexStr(DataSaver.Instance.data);
        switch(DataSaver.Instance.dataNum)
        {
            case 1:currentText = text01;
                break;
            case 2:currentText = text02;
                break;
            case 3:currentText = text03;
                break;
        }
        if (currentText != null)
        {
            currentText.text = "   编号：0" + DataSaver.Instance.dataNum + "\n" + " X坐标：" + DataSaver.Instance.x + "\n" + " Y坐标：" + DataSaver.Instance.y + "\n" + "偏航角：" + DataSaver.Instance.yaw;
        }
    }



}
