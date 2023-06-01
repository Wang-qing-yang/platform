using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using  RosMessageTypes.Nav;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class UnitySubscriberPath : MonoBehaviour
{
    public GameObject axisCube;
    private void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<PathMsg>("/move_base_node/NavfnRos/plan", GetPath);
    }

    private void GetPath(PathMsg mapPath)
    {


        var c = mapPath.poses[mapPath.poses.Length - 1];
        Debug.Log(c.pose.ToString());

        var tempObj = Instantiate(axisCube);
        tempObj.transform.parent = transform;

        var pos = c.pose.position.From<FLU>();
        // Unity's* **(x,y,z)** *is equivalent to the ROS* **(z,-x,y)** *coordinate
        tempObj.transform.localPosition = new Vector3((float)pos.x, (float)pos.y, (float)pos.z);
        var orientation = c.pose.orientation.From<FLU>();

        tempObj.transform.localRotation = new Quaternion((float)orientation.x, (float)orientation.y, (float)orientation.z, (float)orientation.w);

    }
}
