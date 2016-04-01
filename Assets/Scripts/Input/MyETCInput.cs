using UnityEngine;
using System.Collections;

public class MyETCInput : MonoBehaviour {

	// Use this for initialization
	void Start () {

#if !MOBILE_INPUT
        gameObject.SetActive(false);
#endif
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void OnMoving(Vector2 v)
    {
        Player.Self.SetMoveDir(v);
    }
    public void OnMoveEnd()
    {
        Player.Self.ClearMoveDir();
    }

    public void OnAttack()
    {
        Player.Self.Attack();
    }
}
