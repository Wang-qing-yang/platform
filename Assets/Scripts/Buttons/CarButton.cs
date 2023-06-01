﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
///</summary>
public class CarButton : MonoBehaviour
{
    public GameObject go;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(Show);
    }

    private void Show()
    {
        go.SetActive(!go.activeInHierarchy);
    }


}
