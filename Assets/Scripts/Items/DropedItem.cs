using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropedItem : MonoBehaviour
{
    public static int dropedItemLayer = LayerMask.GetMask("DropedItem");

    float rotationSpeed = 100.0f;
    public BaseItem itemData;
    void Start()
    {
    }

    void Update()
    {
        if(itemData == null)
        {
//             Debug.LogError("BaseItem itemData == null");
//             Destroy(gameObject);
//             return;
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 10.0f, dropedItemLayer))
            {
                Debug.DrawLine(ray.origin, hitInfo.point);//划出射线，只有在scene视图中才能看到
                GameObject gameObj = hitInfo.collider.gameObject;
                Debug.Log("click object name is " + gameObj.name);
                if (gameObj.tag == "Player")//当射线碰撞目标为boot类型的物品 ，执行拾取操作
                {
                    Debug.Log("pick up!");
                }
            }
        }

        //自旋转
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }


    //**********************************************
    //掉落逻辑
    static Vector3[] dropPositions;
    static Dictionary<string, GameObject> dicDropItems = new Dictionary<string, GameObject>();
    static DropedItem()
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
    public static void Drop(Vector3 spawnPos, uint tid)
    {
        string[] dropList = new string[] {
            "Prefabs/DropItems/Broadsword",
        };
        int dropNum = Random.Range(1, dropPositions.Length + 1);
        for (int i = 0; i < dropList.Length; i++)
        {
            GameObject obj = null;
            if (!dicDropItems.TryGetValue(dropList[i], out obj))
            {
                obj = Resources.Load<GameObject>(dropList[i]);
                dicDropItems.Add(dropList[i], obj);

                //Instantiate(dropList[Random.Range(0, dropList.Length)], transform.position, transform.rotation);
            }

            GameObject gameobj = GameObject.Instantiate<GameObject>(obj);
            gameobj.transform.position = spawnPos + dropPositions[i];
            gameobj.AddComponent<DropedItem>();
            float lscale = 0.5f;
            gameobj.transform.localScale = new Vector3(lscale, lscale, lscale);
        }
    }

    public void DropII()
    {
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
    }
}
