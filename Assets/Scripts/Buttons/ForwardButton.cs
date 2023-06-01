using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
///
///</summary>
public class ForwardButton : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    //private Button button;

    //private void Awake()
    //{
    //    button = GetComponent<Button>();
    //}

    //private void Start()
    //{

    //}

    //private void Update()
    //{
    //    button.onClick.AddListener(GetV);
    //}

    //private void GetV()
    //{
    //    DataSaver.Instance.v = 0f;
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        DataSaver.Instance.v = 0.1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DataSaver.Instance.v = 0;
    }

}
