using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class test : MonoBehaviour {

    public GameObject testTag1;
    public GameObject testTag2;
    public GameObject testTag3;

    public Rigidbody rigidbody;
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
        rigidbody.MovePosition(transform.position + movement);
        //rigidbody.MovePosition(transform.position + movement);
    }

    public void OnClickTest()
    {
        // DropedItem.Drop(Vector3.zero, 2);
        testTag1.GetComponent<Image>().sprite = SpriteManager.GetIconEquip("weapon0");
        testTag2.GetComponent<Image>().sprite = SpriteManager.GetIconEquip("weapon0");
        testTag3.GetComponent<Image>().sprite = SpriteManager.GetIconEquip("weapon0");
    }
}
