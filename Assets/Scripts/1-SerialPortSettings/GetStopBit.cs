using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

/// <summary>
///
///</summary>
public class GetStopBit : MonoBehaviour
{
    private Dropdown stop_Dropdown;

    private void Awake()
    {
        stop_Dropdown = GetComponent<Dropdown>();
    }

    private void Start()
    {
        stop_Dropdown.onValueChanged.AddListener(Get);
    }

    private void Get(int value)
    {
        switch (value)
        {
            case 0:
                DataSaver.Instance.stopbits = StopBits.One;
                break;
            case 1:
                DataSaver.Instance.stopbits = StopBits.OnePointFive;
                break;
            case 2:
                DataSaver.Instance.stopbits = StopBits.Two;
                break;
            case 3:
                DataSaver.Instance.stopbits = StopBits.None;
                break;
        }

    }


}

