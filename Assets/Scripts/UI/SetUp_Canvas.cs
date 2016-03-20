using UnityEngine;
using System.Collections;

public class SetUp_Canvas : MonoBehaviour {

    Canvas canvas;
    // Use this for initialization
    void Start () {

        canvas = GetComponent<Canvas>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnGoOn()
    {
        canvas.enabled = !canvas.enabled;
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void OnReset()
    {
        canvas.enabled = false;
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevel);
    }

    public void OnExit()
    {
        Time.timeScale = 1;
        Application.LoadLevel("Start");
    }
}
