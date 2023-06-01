using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class UnityPublisherAccel : MonoBehaviour
{
    ROSConnection ros;

    //public string topicName = "accel";
    public string topicName = "accel";

    // Publish the cube's position and rotation every N seconds
    public float publishMessageFrequency = 0.1f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<AccelMsg>(topicName);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {

            AccelMsg a = new AccelMsg(
                new Vector3Msg(DataSaver.Instance.v, 0, 0),
                new Vector3Msg(0, 0, DataSaver.Instance.w)
                );

            // Finally send the message to server_endpoint.py running in ROS
            ros.Publish(topicName, a);

            timeElapsed = 0;
        }
    }
}
