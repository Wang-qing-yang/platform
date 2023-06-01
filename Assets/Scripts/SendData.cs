using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
///</summary>
public class SendData : MonoBehaviour
{
    public Text cmdText;

    private void Update()
    {
        cmdText.text = "    编号：0" +DataSaver.Instance.cmdNum+"\n"+ "线速度：" + DataSaver.Instance.v+"\n"+
                                    "角速度：" + DataSaver.Instance.w+ "\n"+ "\n"+ "当前命令帧：" + DataSaver.Instance.s;
    }


}
