using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using System;

public class UnityPublisherPoseStampedMsg : MonoBehaviour
{
    ROSConnection ros;

    private string topicName = "/move_base_simple/goal";

    RaycastHit hitInfo;
    public Vector3 goal;
    public int num = 0;

    public bool isGoal = false;

    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseStampedMsg>(topicName);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo) && hitInfo.transform.CompareTag("Floor"))
            {
                goal = hitInfo.point;
                isGoal = true;
                num++;
            }
        }


        if (isGoal)
        {
            PointMsg pos = new PointMsg(
           Convert.ToDouble(goal.x),
           Convert.ToDouble(goal.z),
           Convert.ToDouble(goal.y)
           );

            if (num == 1)
            {
                QuaternionMsg qua = new

                       QuaternionMsg(
          0,
          0, 0.337,
          0.94
          );

                PoseMsg posm = new PoseMsg(
                pos,
                qua
                );

                HeaderMsg he = new HeaderMsg(
                    0,
                    new TimeMsg(),
                    "map"
                    );

                PoseStampedMsg a = new PoseStampedMsg(
                            he,
                            posm
                        );

                ros.Publish(topicName, a);
                isGoal = false;
            }
            if (num == 2)
            {
                QuaternionMsg qua = new QuaternionMsg(
      0,
      0, 0.982,
      0.187
      );

                PoseMsg posm = new PoseMsg(
                pos,
                qua
                );

                HeaderMsg he = new HeaderMsg(
                    0,
                    new TimeMsg(),
                    "map"
                    );

                PoseStampedMsg a = new PoseStampedMsg(
                            he,
                            posm
                        );

                ros.Publish(topicName, a);
                isGoal = false;
            }
                    if(num==3)
                    {
                        QuaternionMsg qua = new QuaternionMsg(
          0,
          0, -0.999,
          0.033
          ); PoseMsg posm = new PoseMsg(
                 pos,
                 qua
                 );

                HeaderMsg he = new HeaderMsg(
                    0,
                    new TimeMsg(),
                    "map"
                    );

                PoseStampedMsg a = new PoseStampedMsg(
                            he,
                            posm
                        );

                ros.Publish(topicName, a);
                isGoal = false;
                num = 0;

            }
               

        }
    }
}
