using UnityEngine;
using System.Collections;

public class DebugDrawCircle : MonoBehaviour
{

    public float m_Radius = 1; // 圆环的半径
    public float m_Theta = 0.1f; // 值越低圆环越平滑
    public Color m_Color = Color.yellow; // 线框颜色

    void Update()
    {
        if (m_Theta < 0.0001f) m_Theta = 0.0001f;

        // 绘制圆环
        Vector3 beginPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
        {
            float x = m_Radius * Mathf.Cos(theta);
            float z = m_Radius * Mathf.Sin(theta);
            Vector3 endPoint = new Vector3(x + transform.position.x, transform.position.y + 0.5f, z + transform.position.z);
            if (theta == 0)
            {
                firstPoint = endPoint;
            }
            else
            {
                Debug.DrawLine(beginPoint, endPoint, m_Color);
            }
            beginPoint = endPoint;
        }

        // 绘制最后一条线段
        Debug.DrawLine(beginPoint, firstPoint, m_Color);
    }

    public void SetRadius(float v)
    {
        m_Radius = v;
    }
}
