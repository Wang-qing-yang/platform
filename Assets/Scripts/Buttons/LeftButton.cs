using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
///
///</summary>
public class LeftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        DataSaver.Instance.w = -0.1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DataSaver.Instance.w = 0;
    }

}
