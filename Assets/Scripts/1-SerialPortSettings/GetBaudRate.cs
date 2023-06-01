using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
///</summary>
public class GetBaudRate : MonoBehaviour
{
    private InputField BaudRateInput;

    private void Awake()
    {
        BaudRateInput = GetComponent<InputField>();
    }

    private void Start()
    {
        BaudRateInput.onEndEdit.AddListener(Get);
    }

    private void Get(string str)
    {
        GameObject.Find("DataSaver").GetComponent<DataSaver>().baudRate=int.Parse(str);
    }


}

