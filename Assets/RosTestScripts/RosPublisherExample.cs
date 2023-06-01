using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.UnityRoboticsDemo;
using RosMessageTypes.Geometry;

/// <summary>
/// 
/// </summary>
public class RosPublisherExample : MonoBehaviour
{
    ROSConnection ros;
    //public string topicName = "pos_rot";
    public string topicName = "accel";

    // The game object 
    public GameObject cube;
    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<AccelMsg>(topicName);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            //cube.transform.rotation = Random.rotation;

            //PosRotMsg cubePos = new PosRotMsg(
            //    cube.transform.position.x,
            //    cube.transform.position.y,
            //    cube.transform.position.z,
            //    cube.transform.rotation.x,
            //    cube.transform.rotation.y,
            //    cube.transform.rotation.z,
            //    cube.transform.rotation.w
            //);

            AccelMsg a = new AccelMsg(
                new Vector3Msg(0.1,0.2,0.3),
                new Vector3Msg(0.4, 0.5, 0.6)
                ) ;

            // Finally send the message to server_endpoint.py running in ROS
            ros.Publish(topicName, a);

            timeElapsed = 0;
        }
    }
}
