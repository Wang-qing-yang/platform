using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Nav;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;


public class UnitySubscriberOdometryMsg : MonoBehaviour
{
    public Transform car;
    void Start()
    {
        Debug.Log(1);
        ROSConnection.GetOrCreateInstance().Subscribe<OdometryMsg>("/odom", GetTransform);

    }

    private void GetTransform(OdometryMsg carTransform)
    {
        car.SetPositionAndRotation(carTransform.pose.pose.position.From<FLU>(), carTransform.pose.pose.orientation.From<FLU>());
        DataSaver.Instance.x = car.position.To<FLU>().x;
        DataSaver.Instance.y = car.position.To<FLU>().y;
        DataSaver.Instance.yaw = car.eulerAngles.y;
    }
    

}
