using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;



[Serializable]
public class saveBinary
{
    public int[] aa = { 0, 1, 2 };
    public string nama = "别天神";
}
[Serializable]
public class TestSave
{
    public saveBinary[] tt = new saveBinary[5];
}
public class test : MonoBehaviour {

    public GameObject testTag1;
    public GameObject testTag2;
    public GameObject testTag3;

    public Rigidbody rigidBody;
    // Use this for initialization
    void Start () {

        //string fileName = Application.persistentDataPath + "/" + "test.txt"; 
        //fileName = Application.dataPath + "/" + "test.txt";

       
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 movement = new Vector3(1, 0, 0);
        movement = movement.normalized * 1 * Time.deltaTime;
        transform.position += movement;
        //rigidbody.MovePosition(transform.position + movement);
    }
    void sFixedUpdate()
    {
        Vector3 movement = new Vector3(1,0,0);
        movement = movement.normalized * 1 * Time.deltaTime;
        rigidBody.MovePosition(transform.position + movement);
        //rigidbody.MovePosition(transform.position + movement);
    }

    TestSave sb = new TestSave();

    public void OnClickTest()
    {
    }
    public void OnClickTest2()
    {
    }
}
