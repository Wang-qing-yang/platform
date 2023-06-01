using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///
///</summary>
public class SceneSize : MonoBehaviour
{
    public InputField length;
    public InputField width;
    public InputField height;
    public Transform Scene;

    private void Start()
    {
        length.onEndEdit.AddListener(SetLength);
        width.onEndEdit.AddListener(SetWidth);
        height.onEndEdit.AddListener(SetHight);
    }

    private void SetLength(string str)
    {
        //Debug.Log(Scene.localScale);
        if(float.Parse(str)>0&&float.Parse(str)<=30)
        {
            DataSaver.Instance.SceneSize = new Vector3(float.Parse(str) / 10, DataSaver.Instance.SceneSize.y, DataSaver.Instance.SceneSize.z);
            Scene.localScale = DataSaver.Instance.SceneSize;
        }
    }

    private void SetWidth(string str)
    {
        if (float.Parse(str) > 0 && float.Parse(str) <= 30)
        {
            DataSaver.Instance.SceneSize = new Vector3(DataSaver.Instance.SceneSize.x, DataSaver.Instance.SceneSize.y, float.Parse(str) / 10);
            Scene.localScale = DataSaver.Instance.SceneSize;
        }
    }

    private void SetHight(string str)
    {
        if (float.Parse(str) > 0 && float.Parse(str) <= 5)
        {
            DataSaver.Instance.SceneSize = new Vector3(DataSaver.Instance.SceneSize.x, float.Parse(str), DataSaver.Instance.SceneSize.z);
            Scene.localScale = DataSaver.Instance.SceneSize;
        }
    }



}
