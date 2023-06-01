using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

/// <summary>
///
///</summary>
public class GetParity : MonoBehaviour
{
    private Dropdown parity_Dropdown;

    private void Awake()
    {
        parity_Dropdown = GetComponent<Dropdown>();
    }

    private void Start()
    {
        parity_Dropdown.onValueChanged.AddListener(Get);
    }

    private void Get(int value)
    {
        switch (value)
        {
            case 0:
                DataSaver.Instance.parity= Parity.None;
                break;
            case 1:
                DataSaver.Instance.parity = Parity.Odd;
                break;
            case 2:
                DataSaver.Instance.parity = Parity.Even;
                break;
        }

    }


}

