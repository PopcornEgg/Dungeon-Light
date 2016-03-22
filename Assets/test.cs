using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class test : MonoBehaviour {

    public Image img;
	// Use this for initialization
	void Start () {

        //string fileName = Application.persistentDataPath + "/" + "test.txt"; 
        //fileName = Application.dataPath + "/" + "test.txt";

       
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DropItemTest()
    {
        DropedItem.Drop(Vector3.zero, 2);
    }
}
