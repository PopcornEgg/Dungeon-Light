using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float smoothing = 5f;
    public Vector3 offsetPosition = new Vector3(0, 4.81f, -6.81f);
    Character target;
    void Awake()
    {
        StaticManager.sCameraFollow = this;
    }
    void Start ()
    {
        target = StaticManager.sPlayer;
    }


    void FixedUpdate ()
    {
        Vector3 targetCamPos = target.transform.position + offsetPosition;
        transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
