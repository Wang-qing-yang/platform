using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///添加相应类型（正方体，球体，圆柱体）的障碍物
///</summary>
public class AddObstacles : MonoBehaviour
{
    //GameObject a = GameObject.CreatePrimitive(PrimitiveType.Cube);
    public Dropdown obstacleType;//类型
    private Button add;
    private PrimitiveType obstacleName=PrimitiveType.Cube;
    public GameObject obstacles;//障碍物父物体

    private void Awake()
    {
        add = GetComponent<Button>();
    }

    private void Start()
    {
        obstacleType.onValueChanged.AddListener(GetType);
        add.onClick.AddListener(Add);
    }

    /// <summary>
    /// 获得障碍物类型
    /// </summary>
    /// <param name="value"></param>
    private void GetType(int value)
    {
        switch (value)
        {
            case 0:
                obstacleName = PrimitiveType.Cube;
                break;
            case 1:
                obstacleName = PrimitiveType.Sphere;
                break;
            case 2:
                obstacleName = PrimitiveType.Cylinder;
                break;
        }
    }

    /// <summary>
    /// 添加障碍物并且tag
    /// </summary>
    private void Add()
    {
        GameObject a =GameObject.CreatePrimitive(obstacleName);
        a.tag = "Obstacle";
        a.transform.SetParent(obstacles.transform);
    }


}
