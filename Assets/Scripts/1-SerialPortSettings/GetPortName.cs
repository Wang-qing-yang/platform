using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
///</summary>
public class GetPortName : MonoBehaviour
{
    private InputField portNameInput;

    private void Awake()
    {
        portNameInput = GetComponent<InputField>();
    }

    private void Start()
    {
        portNameInput.onEndEdit.AddListener(GetName);
    }

    private void GetName(string str)
    {
        GameObject.Find("DataSaver").GetComponent<DataSaver>().portName = str;
    }


}
