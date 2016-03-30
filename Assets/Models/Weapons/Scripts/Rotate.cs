using UnityEngine;
using System.Collections;


public class Rotate : MonoBehaviour {
	public float variable = 10;
	public bool activate = false;
    void Update() {

        Shader sd = Resources.Load<Shader>("Shaders/Outline");
        MeshRenderer md = transform.FindChild(gameObject.name).GetComponent<MeshRenderer>();
        md.material.shader = sd;

        if (activate)
		{
        	transform.Rotate(Vector3.right * variable);
		}
    }
}