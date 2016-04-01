using UnityEngine;
using System.Collections;

public class SetUp_Panel : MonoBehaviour {

	// Update is called once per frame
	void Update () {
	
	}

    public void OnGoOn()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void OnReset()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
        Player.Self.ReLive();
        //Application.LoadLevel(Application.loadedLevel);
    }

    public void OnExit()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
        Application.LoadLevel("Start");
    }
}
