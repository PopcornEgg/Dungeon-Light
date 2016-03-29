using UnityEngine;
using System.Collections;


public class Rotate : MonoBehaviour {
	public float variable = 10;
	public bool activate = false;
    void Update() {
		if(activate)
		{
        	transform.Rotate(Vector3.right * variable);
		}
    }
}