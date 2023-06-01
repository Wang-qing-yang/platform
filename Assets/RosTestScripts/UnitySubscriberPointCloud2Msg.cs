using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;

public class UnitySubscriberPointCloud2Msg : MonoBehaviour
{
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<PointCloud2Msg>("sensor_msgs/PointCloud2", Get);
    }

    private void Get(PointCloud2Msg pointCloud)
    {
        Debug.Log("1");
    }
}
