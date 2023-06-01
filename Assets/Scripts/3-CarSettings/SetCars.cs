using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 选择无人车，并设置其初始状态（x坐标，y坐标，yaw偏航角）,尺寸大小（length长，width宽，height高）,小车编号（num)
/// </summary>
public class SetCars : MonoBehaviour
{
    public Camera ca;
    private Ray ra;
    private RaycastHit hit;

    //初始状态
    public InputField x_InputField;//x坐标
    public InputField y_InputField;//y坐标
    public InputField yaw_InputField;//yaw偏航角

    //尺寸大小
    public InputField length_InputField;//长
    public InputField width_InputField;//宽
    public InputField height_InputField;//高

    public InputField num_InputField;//无人车编号

    public Button delete;

    private void Start()
    {
        //添加监听
        x_InputField.onEndEdit.AddListener(GetX);
        y_InputField.onEndEdit.AddListener(GetY);
        yaw_InputField.onEndEdit.AddListener(GetYaw);

        length_InputField.onEndEdit.AddListener(GetLength);
        width_InputField.onEndEdit.AddListener(GetWidth);
        height_InputField.onEndEdit.AddListener(GetHeight);

        num_InputField.onEndEdit.AddListener(GetNum);

        delete.onClick.AddListener(DeletaObject);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            ra = ca.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ra, out hit))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    DataSaver.Instance.MouseObjects = hit.transform.parent;

                    x_InputField.text = hit.transform.parent.localPosition.x.ToString("0.00");
                    y_InputField.text = hit.transform.parent.localPosition.z.ToString("0.00");
                    yaw_InputField.text = hit.transform.parent.localEulerAngles.y.ToString("0.00");

                    length_InputField.text = hit.transform.parent.localScale.z.ToString("0.00");
                    width_InputField.text = hit.transform.parent.localScale.x.ToString("0.00");
                    height_InputField.text = hit.transform.parent.localScale.y.ToString("0.00");

                    num_InputField.text = hit.transform.parent.tag;


                }
            }
        }
    }

    /// <summary>
    /// 设置无人车x坐标
    /// </summary>
    /// <param name="str"></param>
    private void GetX(string str)
    {
        DataSaver.Instance.MouseObjects.localPosition = new Vector3(float.Parse(str),
            DataSaver.Instance.MouseObjects.localPosition.y, DataSaver.Instance.MouseObjects.localPosition.z);
    }
    /// <summary>
    /// 设置无人车y坐标
    /// </summary>
    /// <param name="str"></param>
    private void GetY(string str)
    {
        DataSaver.Instance.MouseObjects.localPosition = new Vector3(DataSaver.Instance.MouseObjects.localPosition.x,
            DataSaver.Instance.MouseObjects.localPosition.y, float.Parse(str));
    }
    /// <summary>
    /// 设置无人车偏航角yaw
    /// </summary>
    /// <param name="str"></param>
    private void GetYaw(string str)
    {
        DataSaver.Instance.MouseObjects.localEulerAngles = new Vector3(DataSaver.Instance.MouseObjects.localEulerAngles.x, float.Parse(str),
            DataSaver.Instance.MouseObjects.localEulerAngles.x);
    }

    /// <summary>
    /// 设置无人车宽度width
    /// </summary>
    /// <param name="str"></param>
    private void GetWidth(string str)
    {
        DataSaver.Instance.MouseObjects.localScale = new Vector3(float.Parse(str),
            DataSaver.Instance.MouseObjects.localScale.y, DataSaver.Instance.MouseObjects.localScale.z);
    }
    /// <summary>
    /// 设置无人车长度length
    /// </summary>
    /// <param name="str"></param>
    private void GetLength(string str)
    {
        DataSaver.Instance.MouseObjects.localScale = new Vector3(DataSaver.Instance.MouseObjects.localScale.x,
           DataSaver.Instance.MouseObjects.localScale.y, float.Parse(str));
    }
    /// <summary>
    /// 设置无人车高度height
    /// </summary>
    /// <param name="str"></param>
    private void GetHeight(string str)
    {
        DataSaver.Instance.MouseObjects.localScale = new Vector3(DataSaver.Instance.MouseObjects.localScale.x, float.Parse(str)
            , DataSaver.Instance.MouseObjects.localScale.z);
    }

    /// <summary>
    /// 设置无人车高度编号num
    /// </summary>
    /// <param name="str"></param>
    private void GetNum(string str)
    {
        if (int.Parse(str) > 0 && int.Parse(str) < 6)
            DataSaver.Instance.MouseObjects.tag = str;
        else
            return;
    }

    /// <summary>
    /// 删除无人车
    /// </summary>
    private void DeletaObject()
    {
        if (DataSaver.Instance.MouseObjects != null)
            Object.Destroy(DataSaver.Instance.MouseObjects.gameObject);
    }

}
