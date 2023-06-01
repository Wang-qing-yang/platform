using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
///</summary>
public class test : MonoBehaviour
{
    public Button m_Button;
    public GameObject go;

    private void Start()
    {
        m_Button.onClick.AddListener(onClick);
    }

    private void onClick()
    {
        go.SetActive(!go.activeInHierarchy);
    }



}
