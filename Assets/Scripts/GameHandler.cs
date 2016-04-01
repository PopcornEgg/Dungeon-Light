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
        Player.Self.SaveAll();
        Invoke("AutoSave", Config.AutoSaveTime);
    }
}
