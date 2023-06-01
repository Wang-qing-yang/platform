using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeAndData : MonoBehaviour
{

    public Text timeText;
    public Text DataText;

    void Update()
    {
        timeText.text = string.Format("{0:T}", System.DateTime.Now); 
        DataText.text = string.Format("{0:D}", System.DateTime.Now) + " " + GetWeek();
    }

    /// <summary>
    /// 获取星期天数
    /// </summary>
    /// <returns>星期几</returns>
    private string GetWeek()
    {
        string week = string.Empty;
        switch ((int)DateTime.Now.DayOfWeek)
        {
            case 0:
                week = "星期日";
                break;
            case 1:
                week = "星期一";
                break;
            case 2:
                week = "星期二";
                break;
            case 3:
                week = "星期三";
                break;
            case 4:
                week = "星期四";
                break;
            case 5:
                week = "星期五";
                break;
            default:
                week = "星期六";
                break;
        }
        return week;
    }
}
