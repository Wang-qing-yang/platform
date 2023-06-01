using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///场景漫游（第三人称视角、自由视角、全屏设置）
///</summary>
public class SwitchVisionMode : MonoBehaviour
{
    //UI组件设置
    public Toggle myToggle;
    public Toggle myToggle1;
    //视角模式设置 0：固定视角/1：第三人称视角/2：自由视角
    [Range(0, 2)]
    public int visionMode=0;
    //Main Camera组件
    public Camera myCamera;

    private void Start()
    {
        //UI添加监听事件
        myToggle.onValueChanged.AddListener((bool isOn) => { OnToggleClick(myToggle, isOn); });
        myToggle1.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(myToggle, isOn); });
    }

    private void Update()
    {
        //视角模式设置 0：固定视角/1：第三人称视角/2：自由视角
        switch (visionMode)
        {
            case 0:
                break;
            case 1:
                FixedCamera();
                break;
            case 2:
                ThirdPersonCamera();
                break ;
        }
    }

    /// <summary>
    /// 固定视角
    /// </summary>
    private void FixedCamera()
    {
        myCamera.transform.position = new Vector3(13.5f, 8f, 1.2f);
        myCamera.transform.eulerAngles = new Vector3(45, -90, 0);
        myCamera.fieldOfView = 40;
    }


    /// <summary>
    /// 第三人称视角
    /// </summary>
    /// 
    public float sensitivityMouse = 2f;//鼠标灵敏度
    public float sensitivetyKeyBoard = 0.1f;//键盘灵敏度
    public float sensitivetyMouseWheel = 10f;//滚轮灵敏度
    private void ThirdPersonCamera()
        {
                //滚轮实现镜头缩进和拉远
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    myCamera.fieldOfView = myCamera.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * sensitivetyMouseWheel;
                }
        //按着鼠标右键实现视角转动
        if (Input.GetMouseButton(1))
        {
            myCamera.transform.Rotate(-Input.GetAxis("Mouse Y") * sensitivityMouse, Input.GetAxis("Mouse X") * sensitivityMouse, 0);
        }

        //键盘按钮↑/w和↓/s实现视角前后移动
        if (Input.GetAxis("Vertical") != 0)
        {
            myCamera.transform.Translate(0, 0, Input.GetAxis("Vertical") * sensitivetyKeyBoard);

        }

            //键盘按钮←/a和→/d实现视角水平移动
            if (Input.GetAxis("Horizontal") != 0)
        {
            myCamera.transform.Translate(Input.GetAxis("Horizontal") * sensitivetyKeyBoard, 0, 0);
        }
        //键盘按钮←/q和→/e实现视角上下移动
        if (Input.GetAxis("Jump") != 0)
        {
            myCamera.transform.Translate(0, Input.GetAxis("Jump") * sensitivetyKeyBoard, 0);
        }
            }

    /// <summary>
    /// 鼠标左键双击实现全屏切换
    /// </summary>
    private void OnGUI()
    {
        //判断鼠标双击
        if (Event.current.isMouse && Event.current.type == EventType.MouseDown && Event.current.clickCount == 2)
        {
            //判断鼠标位置，防止误触
            if (Event.current.mousePosition.x > 240 && Event.current.mousePosition.y > 200 && Event.current.mousePosition.x < 780)
            {
                if (myCamera.rect.xMin == 0.24f)
                {
                    myCamera.rect = new Rect(0, 0, 1, 1);
                }
                else
                {
                    myCamera.rect = new Rect(0.24f, -0.35f, 0.52f, 1.0f);
                }
            }
        }
    }

    private void OnToggleClick(Toggle toggle, bool isOn)
    {
        visionMode = isOn ? 1 : 0;
    }

    private void OnToggleClick1(Toggle toggle, bool isOn)
    {
        visionMode = isOn ? 2 : 0;
    }



}
