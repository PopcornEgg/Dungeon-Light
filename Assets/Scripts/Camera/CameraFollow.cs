using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    static public CameraFollow Self = null;
    public float smoothing = 5f;
    public Vector3 offsetPosition = new Vector3(0, 4.81f, -6.81f);
    Character target;
    void Awake()
    {
    }
    void Start ()
    {
        Self = this;
    }
    void FixedUpdate ()
    {
        if (target != null)
        {
            Vector3 targetCamPos = target.transform.position + offsetPosition;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
        else target = Player.Self;
    }
}
