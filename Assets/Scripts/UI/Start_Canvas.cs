using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class Start_Canvas : MonoBehaviour {

    public LoadScene_Canvas loadScene_Canvas;

    Canvas canvas;
    // Use this for initialization
    void Start ()
    {
        canvas = GetComponent<Canvas>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickStart()
    {
        if (loadScene_Canvas != null)
        {
            canvas.enabled = false;
            Invoke("LoadScene", 0.001f);
        }
        else
            Debug.Log("loadScene_Canvas == null");
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    void LoadScene()
    {
        loadScene_Canvas.StartLoadScene("Level 01");
    }
}
