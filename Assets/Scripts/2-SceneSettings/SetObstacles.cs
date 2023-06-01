using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选择障碍物，并设置其初始状态（x坐标，y坐标，z坐标）,尺寸大小（length长，width宽，height高）,角度，类型
/// </summary>
public class SetObstacles : MonoBehaviour
{
    public Camera ca;
    private Ray ra;
    private RaycastHit hit;

    //位置Position
    public InputField posX;
    public InputField posY;
    public InputField posZ;

    //角度Rotation
    public InputField RotX;
    public InputField RotY;
    public InputField RotZ;

    //尺寸Scale
    public InputField ScaleX;
    public InputField ScaleY;
    public InputField ScaleZ;

    public Button delete;

    private void Start()
    {
        //添加监听事件
        posX.onEndEdit.AddListener(GetPosX);
        posY.onEndEdit.AddListener(GetposY);
        posZ.onEndEdit.AddListener(GetposZ);

        ScaleX.onEndEdit.AddListener(GetSizeX);
        ScaleY.onEndEdit.AddListener(GetSizeY);
        ScaleZ.onEndEdit.AddListener(GetSizeZ);

        RotX.onEndEdit.AddListener(GetRotX);
        RotY.onEndEdit.AddListener(GetRotY);
        RotZ.onEndEdit.AddListener(GetRotZ);

        delete.onClick.AddListener(DeletaObject);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ra = ca.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ra, out hit))
            {
                if(hit.transform.gameObject.tag=="Obstacle")
                {
                    DataSaver.Instance.MouseObjects = hit.transform;
                    posX.text = hit.transform.position.x.ToString();
                    posY.text = hit.transform.position.y.ToString();
                    posZ.text = hit.transform.position.z.ToString();

                    RotX.text = hit.transform.eulerAngles.x.ToString();
                    RotY.text = hit.transform.eulerAngles.y.ToString();
                    RotZ.text = hit.transform.eulerAngles.z.ToString();

                    ScaleX.text = hit.transform.localScale.x.ToString();
                    ScaleY.text = hit.transform.localScale.y.ToString();
                    ScaleZ.text = hit.transform.localScale.z.ToString();
                }
            }     
        }
    }

    /// <summary>
    /// 设置障碍物x坐标
    /// </summary>
    /// <param name="str"></param>
    private void GetPosX(string str)
    {
        DataSaver.Instance.MouseObjects.position = new Vector3(float.Parse(str),
            DataSaver.Instance.MouseObjects.position.y, DataSaver.Instance.MouseObjects.position.z);
    }

    /// <summary>
    /// 设置障碍物y坐标
    /// </summary>
    /// <param name="str"></param>
    private void GetposY(string str)
    {
        DataSaver.Instance.MouseObjects.position = new Vector3(DataSaver.Instance.MouseObjects.position.x,
            float.Parse(str), DataSaver.Instance.MouseObjects.position.z);
    }

    /// <summary>
    /// 设置障碍物z坐标
    /// </summary>
    /// <param name="str"></param>
    private void GetposZ(string str)
    {
        DataSaver.Instance.MouseObjects.position = new Vector3(DataSaver.Instance.MouseObjects.position.x,
            DataSaver.Instance.MouseObjects.position.y, float.Parse(str));
    }

    /// <summary>
    /// 设置障碍物尺寸x
    /// </summary>
    /// <param name="str"></param>
    private void GetSizeX(string str)
    {
        DataSaver.Instance.MouseObjects.localScale = new Vector3(float.Parse(str),
            DataSaver.Instance.MouseObjects.localScale.y, DataSaver.Instance.MouseObjects.localScale.z);
    }

    /// <summary>
    /// 设置障碍物尺寸y
    /// </summary>
    /// <param name="str"></param>
    private void GetSizeY(string str)
    {
        DataSaver.Instance.MouseObjects.localScale = new Vector3(DataSaver.Instance.MouseObjects.localScale.x,
            float.Parse(str), DataSaver.Instance.MouseObjects.localScale.z);
    }

    /// <summary>
    /// 设置障碍物尺寸z
    /// </summary>
    /// <param name="str"></param>
    private void GetSizeZ(string str)
    {
        DataSaver.Instance.MouseObjects.localScale = new Vector3(DataSaver.Instance.MouseObjects.localScale.x,
            DataSaver.Instance.MouseObjects.localScale.y, float.Parse(str));
    }

    /// <summary>
    /// 设置障碍物角度x
    /// </summary>
    /// <param name="str"></param>
    private void GetRotX(string str)
    {
        DataSaver.Instance.MouseObjects.eulerAngles = new Vector3(float.Parse(str),
    DataSaver.Instance.MouseObjects.eulerAngles.y, DataSaver.Instance.MouseObjects.eulerAngles.z);
    }

    /// <summary>
    /// 设置障碍物角度y
    /// </summary>
    /// <param name="str"></param>
    private void GetRotY(string str)
    {
        DataSaver.Instance.MouseObjects.eulerAngles = new Vector3(DataSaver.Instance.MouseObjects.eulerAngles.x,
            float.Parse(str), DataSaver.Instance.MouseObjects.eulerAngles.z);
    }

    /// <summary>
    /// 设置障碍物角度z
    /// </summary>
    /// <param name="str"></param>
    private void GetRotZ(string str)
    {
        DataSaver.Instance.MouseObjects.eulerAngles = new Vector3(DataSaver.Instance.MouseObjects.eulerAngles.x,
            DataSaver.Instance.MouseObjects.eulerAngles.y, float.Parse(str));
    }


    /// <summary>
    /// 删除障碍物
    /// </summary>
    private void DeletaObject()
    {
        if(DataSaver.Instance.MouseObjects != null)
        Object.Destroy(DataSaver.Instance.MouseObjects.gameObject);
    }

}
