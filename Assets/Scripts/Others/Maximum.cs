using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///全屏场景
///</summary>
public class Maximum : MonoBehaviour
{
    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }
    private void OnGUI()
    {
        //判断鼠标双击
        if (Event.current.isMouse && Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
        {
            //判断鼠标位置，防止误触
            if(Event.current.mousePosition.x>240&&Event.current.mousePosition.y>200&& Event.current.mousePosition.x<780)
            {
                if (m_Camera.rect.xMin == 0.24f)
                {
                    m_Camera.rect = new Rect(0, 0, 1, 1);
                }
                else
                {
                    m_Camera.rect = new Rect(0.24f, -0.35f, 0.52f, 1.0f);
                }
            }
        }
    }

}
