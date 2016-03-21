using UnityEngine;
using System.Collections;

public class EquipDrop : MonoBehaviour {

    public static Vector3[] dropPositions;
    static EquipDrop()
    {
        float dis = 1.0f;
        dropPositions = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(-dis, 0, dis),
            new Vector3(0, 0, dis),
            new Vector3(dis, 0, dis),
            new Vector3(dis, 0, 0),
            new Vector3(dis, 0, -dis),
            new Vector3(0, 0, -dis),
            new Vector3(-dis, 0, -dis),
            new Vector3(-dis, 0, 0),
        };
//         for (int i = 0; i < 5; i++)
//         {
//             for (int j = 0; j < 5; j++)
//             {
//                 dropPositions[i * 5 + j] = Vector3.zero + new Vector3(i * dis, 0.0f, j * dis) - new Vector3(dis*2, 0, dis * 2);
//             }
//         }
    }
    public GameObject[] dropList;
	// Use this for initialization
	void Start () {
        //Died();
        
    }
	
	// Update is called once per frame
	void Update () {
        

    }

   // Can't destroy Transform component of 'Cylinder'. If you want to destroy the game object, please call 'Destroy' on the game object instead. Destroying the transform component is not allowed.

    public void Die() {

        //if (dropList == null)
        //    return;

        //Debug.Log("in Died");
        //Destroy(this);
        //DestroyImmediate(gameObject);

        //UnityEngine.Object obj = Resources.Load("sword");
        //Instantiate(obj);
        //Instantiate<GameObject>("sword");

        int dropNum = Random.Range(1, dropPositions.Length + 1);
        for (int i = 0; i < dropNum; i++)
        {
            GameObject obj = Instantiate<GameObject>(dropList[Random.Range(0, dropList.Length)]);
            obj.transform.position = transform.position + dropPositions[i];
            obj.AddComponent<SelfRation>();
            //Instantiate(dropList[Random.Range(0, dropList.Length)], transform.position, transform.rotation);
        }
    }

    public GameObject testObj;
    public void DiedII()
    {

        //if (dropList == null)
        //    return;

        //Debug.Log("in Died");
        //Destroy(this);
        //DestroyImmediate(gameObject);

        //UnityEngine.Object obj = Resources.Load("sword");
        //Instantiate(obj);
        //Instantiate<GameObject>("sword");


        //画一个园
        /*
        float dis = 2.0f;
        Vector3 newPos = transform.position ;
        newPos.x += dis;
        int dropMaxCount = 8;
        int currDropCount = UnityEngine.Random.Range(1, dropMaxCount + 1);
        float aangle = 360.0f / (float)currDropCount;
        for (int i = 0; i < currDropCount; i++) {

           // Quaternion.LookRotation(transform.position);
           // Instantiate(dropList[Random.Range(0, dropList.Length)], transform.position, Quaternion.zero);
            GameObject obj = Instantiate<GameObject>(testObj);

            obj.transform.position = newPos;
            obj.transform.RotateAround(transform.position, Vector3.up, aangle * i);
            //obj.pos
        }
        */

        for (int i = 0; i < dropPositions.Length; i++) {

           // Quaternion.LookRotation(transform.position);
           // Instantiate(dropList[Random.Range(0, dropList.Length)], transform.position, Quaternion.zero);
            GameObject obj = Instantiate<GameObject>(testObj);

            obj.transform.position = transform.position + dropPositions[i];
            //obj.transform.RotateAround(transform.position, Vector3.up, aangle * i);
            //obj.pos
        }

    }

}
