using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
     
    }

    private void Get(PointCloud2Msg pointCloud)
    {
        Debug.Log("1");
    }
}