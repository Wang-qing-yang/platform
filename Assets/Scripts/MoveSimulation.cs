using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MoveSimulation : MonoBehaviour
{
    //四辆无人车
    private NavMeshAgent agent1;
    private NavMeshAgent agent2;
    private NavMeshAgent agent3;
    private NavMeshAgent agent4;

    //编队
    public Toggle myToggle;
    public Toggle myToggle1;
    public Toggle myToggle2;

    public int formationMode = 0;

    public LineRenderer lr;
    RaycastHit hitInfo;
    private GameObject[] cars;
    private bool isNave;
    private bool isMove;

    public Text mText;
    public Toggle pathToggle;

    private string str;
    private Vector3[] path;//路径
    private bool once;


    public Toggle moveToggle;
    public float moveSpeed = 1;
    public float rotateSpeed = 40;
    private float hor, ver;



    // Use this for initialization

    void Start()
    {
        InitLine();
        cars = GameObject.FindGameObjectsWithTag("Player");
        pathToggle.onValueChanged.AddListener((bool isOn) => { OnPathToggleClick(pathToggle, isOn); });
        moveToggle.onValueChanged.AddListener((bool isOn) => { OnMoveToggleClick(moveToggle, isOn); });

        myToggle.onValueChanged.AddListener((bool isOn) => { OnToggleClick1(myToggle, isOn); });
        myToggle1.onValueChanged.AddListener((bool isOn) => { OnToggleClick2(myToggle, isOn); });
        myToggle2.onValueChanged.AddListener((bool isOn) => { OnToggleClick3(myToggle, isOn); });
    }

    /// <summary>
    /// 初始化LineRenderer
    /// </summary>
    private void InitLine()
    {
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.numCapVertices = 90;
        lr.numCornerVertices = 90;
        lr.material = new Material(Shader.Find("Sprites/Default"));
    }

    // Update is called once per frame
    void Update()
    {
        //编队规划
        switch (formationMode)
        {
            case 1:
                agent2 = cars[1].GetComponentInParent<NavMeshAgent>();
                agent3 = cars[2].GetComponentInParent<NavMeshAgent>();

                agent2.SetDestination(cars[0].transform.parent.position + new Vector3(-1f, 0f, 0f));
                agent3.SetDestination(cars[0].transform.parent.position + new Vector3(-2f, 0f, 0f));
                break;

            case 2:
                agent2 = cars[1].GetComponentInParent<NavMeshAgent>();
                agent3 = cars[2].GetComponentInParent<NavMeshAgent>();

                agent2.SetDestination(cars[0].transform.parent.position + new Vector3(0f, 0f, -1f));
                agent3.SetDestination(cars[0].transform.parent.position + new Vector3(0f, 0f, 1f));
                break;

            case 3:
                agent2 = cars[1].GetComponentInParent<NavMeshAgent>();
                agent3 = cars[2].GetComponentInParent<NavMeshAgent>();

                //agent2.SetDestination(cars[0].transform.parent.position + new Vector3(-0.5f, 0f, -0.5f));
                //agent3.SetDestination(cars[0].transform.parent.position + new Vector3(-0.5f, 0f, 0.5f));
                agent2.SetDestination(cars[0].transform.parent.position + new Vector3(-1f, 0f, -1f));
                agent3.SetDestination(cars[0].transform.parent.position + new Vector3(-1f, 0f, 1f));
                break;

            default:
                break;
        }

        if (isNave || isMove)
        {
            //自由移动
            if (isMove)
            {
                hor = Input.GetAxis("Horizontal");
                ver = Input.GetAxis("Vertical");
                cars[0].transform.parent.Translate(Vector3.forward * ver * Time.deltaTime * moveSpeed);
                cars[0].transform.parent.Rotate(transform.up * hor * Time.deltaTime * rotateSpeed);
            }

            //路径规划
            if (isNave)
            {
                agent1 = cars[0].GetComponentInParent<NavMeshAgent>();

                //距离小于0.5时，不绘制轨迹
                if (Mathf.Abs(agent1.remainingDistance) < 0.5f)
                {
                    lr.positionCount = 0;
                    lr.gameObject.SetActive(false);
                }

                if (lr.gameObject.activeInHierarchy)
                {
                    Vector3[] _path = agent1.path.corners;

                    //储存路径
                    if (!once)
                    {
                        path = _path;
                        once = true;
                    }
                    //lr.SetVertexCount(_path.Length);//设置线段数
                    lr.positionCount = _path.Length;

                    //画出轨迹
                    for (int i = 0; i < _path.Length; i++)
                    {
                        //Debug.Log(i + "= " + _path[i]);
                        lr.SetPosition(i, _path[i]);//设置线段顶点坐标
                    }

                    //记录拐点
                    for (int i = 0; i < path.Length; i++)
                    {
                        str += "端点" + i + "(" + path[i].x.ToString("0.00") + " , " + path[i].z.ToString("0.00") + ")" + "\n";
                    }
                    mText.text = str;
                    str = "";

                }


                if (Input.GetMouseButtonDown(1))
                {

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hitInfo) && hitInfo.transform.CompareTag("Floor"))
                    {
                        agent1.SetDestination(hitInfo.point);
                        //agent.SetDestination(new Vector3(hitInfo.point.x,0f,hitInfo.point.z));
                        lr.gameObject.SetActive(true);
                        once = false;
                    }
                }
            }
            else
            {
                mText.text = "";
                lr.positionCount = 0;
                lr.gameObject.SetActive(false);
            }
        }
        else
        {
            //cars[0].transform.parent.localPosition = new Vector3(-DataSaver.Instance.y, 0, DataSaver.Instance.x);
            //cars[0].transform.parent.localEulerAngles = new Vector3(0, -DataSaver.Instance.yaw, 0);
            cars[0].transform.parent.localPosition = new Vector3(-DataSaver.Instance.y, 0, DataSaver.Instance.x);
            //cars[0].transform.parent.localEulerAngles = new Vector3(0, -DataSaver.Instance.yaw, 0);
            cars[0].transform.parent.localEulerAngles = new Vector3(0, DataSaver.Instance.yaw, 0);
        }





       
    }

    private void OnPathToggleClick(Toggle toggle, bool isOn)
    {
        isNave = isOn;
        //for (int i = 0; i < cars.Length; i++)
        //{
        //    cars[i].GetComponentInParent<NavMeshAgent>().enabled = !cars[i].GetComponentInParent<NavMeshAgent>().enabled;
        //}

        cars[0].GetComponentInParent<NavMeshAgent>().enabled = !cars[0].GetComponentInParent<NavMeshAgent>().enabled;
    }

    private void OnMoveToggleClick(Toggle toggle, bool isOn)
    {
        isMove = isOn;
    }

    private void OnToggleClick1(Toggle toggle, bool isOn)
    {
        formationMode = isOn ? 1 : 0;
    }

    private void OnToggleClick2(Toggle toggle, bool isOn)
    {
        formationMode = isOn ? 2 : 0;
    }

    private void OnToggleClick3(Toggle toggle, bool isOn)
    {
        formationMode = isOn ? 3 : 0;
    }


}