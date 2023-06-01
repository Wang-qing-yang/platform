using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
///</summary>
public class GetDataBit : MonoBehaviour
{
    private Dropdown date_Dropdown;

    private void Awake()
    {
        date_Dropdown = GetComponent<Dropdown>();
    }

    private void Start()
    {
        date_Dropdown.onValueChanged.AddListener(Get);
    }

    private void Get(int value)
    {
        switch (value)
        {
            case 0: 
                DataSaver.Instance.databits = 8;
                break;
            case 1:
                DataSaver.Instance.databits = 7;
                break;
            case 2:
                DataSaver.Instance.databits = 6;
                break;
            case 3:
                DataSaver.Instance.databits =5;
                break;
        }

    }


}
