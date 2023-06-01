using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
//using RosColor = RosMessageTypes.UnityRoboticsDemo.UnityColorMsg;
using PosRot = RosMessageTypes.UnityRoboticsDemo.PosRotMsg;

public class RosSubscriberExample : MonoBehaviour
{
    public GameObject cube;

    void Start()
    {
        //ROSConnection.GetOrCreateInstance().Subscribe<RosColor>("color", ColorChange);
        ROSConnection.GetOrCreateInstance().Subscribe<PosRot>("color", GetpPosRot);
    }

    //void ColorChange(RosColor colorMessage)
    //{
    //    cube.GetComponent<Renderer>().material.color = new Color32((byte)colorMessage.r, (byte)colorMessage.g, (byte)colorMessage.b, (byte)colorMessage.a);
    //}

    void GetpPosRot(PosRot posrotMessage)
    {
        cube.transform.position = new Vector3(posrotMessage.pos_x, posrotMessage.pos_z, posrotMessage.pos_y);
        cube.transform.rotation = new Quaternion(posrotMessage.rot_x, posrotMessage.rot_y, posrotMessage.rot_z, posrotMessage.rot_w);
        cube.transform.eulerAngles = new Vector3(cube.transform.eulerAngles.x, cube.transform.eulerAngles.z, cube.transform.eulerAngles.y);

    }
}
