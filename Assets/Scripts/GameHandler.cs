using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour
{

	void Start ()
    {
        Invoke("AutoSave", Config.AutoSaveTime);
	}
	
    void OnApplicationQuit()
    {
        //OnApplicationQuit
        AutoSave();
    }

    void AutoSave()
    {
        StaticManager.sPlayer.SaveAll();
        Invoke("AutoSave", Config.AutoSaveTime);
    }
}
