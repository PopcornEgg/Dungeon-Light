using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class test : MonoBehaviour {

    public GameObject testTag1;
    public GameObject testTag2;
    public GameObject testTag3;
    // Use this for initialization
    void Start () {

        //string fileName = Application.persistentDataPath + "/" + "test.txt"; 
        //fileName = Application.dataPath + "/" + "test.txt";

       
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickTest()
    {
        // DropedItem.Drop(Vector3.zero, 2);
        testTag1.GetComponent<Image>().sprite = SpriteManager.GetIconEquip("weapon0");
        testTag2.GetComponent<Image>().sprite = SpriteManager.GetIconEquip("weapon0");
        testTag3.GetComponent<Image>().sprite = SpriteManager.GetIconEquip("weapon0");
    }
}
