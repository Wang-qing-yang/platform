using UnityEngine;
using UnityEngine.AI;

// Use physics raycast hit from mouse click to set agent destination

public class FindPath : MonoBehaviour
{
    NavMeshAgent m_Agent;
    RaycastHit m_HitInfo = new RaycastHit();
    public LineRenderer lr;
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        InitLine();
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


    void Update()
    {

        if (Mathf.Abs(m_Agent.remainingDistance) < 1.5f)
        {
            lr.positionCount = 0;
            lr.gameObject.SetActive(false);
        }
        if (lr.gameObject.activeInHierarchy)
        {
            Vector3[] _path = m_Agent.path.corners;//储存路径
            var path = _path;
            lr.SetVertexCount(_path.Length);//设置线段数

            for (int i = 0; i < _path.Length; i++)
            {
                Debug.Log(i + "= " + _path[i]);
                lr.SetPosition(i, _path[i]);//设置线段顶点坐标
            }
        }

        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
            {
                m_Agent.destination = m_HitInfo.point;
                //m_Agent.Stop();
                lr.gameObject.SetActive(true);
            }
        }
    }
}