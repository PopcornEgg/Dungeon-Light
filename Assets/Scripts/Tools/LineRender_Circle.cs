using UnityEngine;
using System.Collections;

public class LineRender_Circle : MonoBehaviour {

    public float m_Radius = 1; // 圆环的半径
    public Color m_Color = Color.yellow; // 线框颜色
    public int pointCount = 60;

    public LineRenderer lRenderer = null;
    void Start()
    {
        lRenderer = gameObject.AddComponent<LineRenderer>();
        lRenderer.enabled = true;
        lRenderer.SetVertexCount(pointCount + 1);
        lRenderer.SetWidth(0.03f, 0.03f);
    }
    void Update()
    {
        float curTheta = 0.0f;
        float addTheta = 2 * Mathf.PI / (float)pointCount;
        for (int idx = 0; idx < pointCount; idx++)
        {
            float x = m_Radius * Mathf.Cos(curTheta);
            float z = m_Radius * Mathf.Sin(curTheta);
            Vector3 pt = new Vector3(x + transform.position.x, transform.position.y, z + transform.position.z);
            lRenderer.SetPosition(idx, pt);
            curTheta += addTheta;
        }
        lRenderer.SetPosition(pointCount,
            new Vector3(m_Radius * Mathf.Cos(0) + transform.position.x, transform.position.y, m_Radius * Mathf.Sin(0) + transform.position.z));
    }

    public void SetRadius(float v)
    {
        m_Radius = v;
    }
}
