using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadScene_Canvas : MonoBehaviour {

    AsyncOperation asyncOperation;
    Canvas canvas;

    public Slider Progress;
    public Text showTxt;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        DontDestroyOnLoad(gameObject);
    }

    void OnGUI()
    {
        //判断异步对象并且异步对象没有加载完毕，显示进度  
        if (asyncOperation != null && !asyncOperation.isDone && Progress != null)
        {
            Progress.value = asyncOperation.progress;
            //GUILayout.Label("progress:" + (float)asyncOperation.progress * 100 + "%");

            if(showTxt != null)
            {
                showTxt.text = (float)asyncOperation.progress * 100 + "%";
            }
        }
    }
    void SetLoadingProgress(float per)
    {
        if ( Progress != null)
        {
            Progress.value = per;
            //GUILayout.Label("progress:" + (float)asyncOperation.progress * 100 + "%");
        }

        if (showTxt != null)
        {
            showTxt.text = per * 100 + "%";
        }
    }

    IEnumerator loadScene(string sceneName)
    {
        int displayProgress = 0;
        int toProgress = 0;
        AsyncOperation op = Application.LoadLevelAsync(sceneName);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            toProgress = (int)op.progress * 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                SetLoadingProgress((float)displayProgress / 100.0f);
                yield return new WaitForEndOfFrame();
            }
        }

        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            SetLoadingProgress((float)displayProgress / 100.0f);
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;

        LoadCompleted();
    }

    void LoadCompleted()
    {
        print("load Complete!");
        canvas.enabled = false;
        asyncOperation = null;
    }
    public void StartLoadScene(string sceneName)
    {
        canvas.enabled = true;
        StartCoroutine("loadScene", sceneName);
    }
}
