using UnityEngine;
using System.Collections;

public class MyETCInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMoving(Vector2 v)
    {
        StaticManager.sPlayer.SetMoveDir(v);
    }
    public void OnMoveEnd()
    {
        StaticManager.sPlayer.ClearMoveDir();
    }

    public void OnAttack()
    {
        StaticManager.sPlayer.Attack();
    }
}
