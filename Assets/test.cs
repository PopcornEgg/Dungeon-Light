using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

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
        DropedItem.Drop(Vector3.zero, 0);

        TabReader tr = new TabReader("Tables/test", true);
    }
}
