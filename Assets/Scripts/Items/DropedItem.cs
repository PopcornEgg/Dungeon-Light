using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropedItem : MonoBehaviour
{
    public static int dropedItemLayer;

    float rotationSpeed = 100.0f;
    public BaseItem itemData;
    void Start()
    {
        StaticManager.sHeadInfo_Canvas.AddItemHeadInfo(this);
    }

    void Update()
    {
        //自旋转
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }
    public void OnPickuped()
    {
        if(itemData == null)
            Debug.LogError("BaseItem itemData == null");
        else
        {
            if (StaticManager.sPlayer.AddBagItem(itemData))
                StaticManager.sSecond_Canvas.RefreshPlayerBag();
        }

        //先从headinfo里移除
        StaticManager.sHeadInfo_Canvas.DelItemHeadInfo(itemData.UId);
        DestroyImmediate(this.gameObject);
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
        MonsterTab _mtab = MonsterTab.Get(tid);
        if (_mtab == null)
            return;

        int dropListIdx = _mtab.GetDropListIdx();
        if (dropListIdx < 0)
            return;

        DropListTab _dltab = DropListTab.Get((uint)dropListIdx);
        if (_dltab == null)
            return;

        for (int i = 0; i < _dltab.droplist.Length; i++)
        {
            ItemTab _itab = ItemTab.Get((uint)_dltab.droplist[i]);
            if (_itab == null)
                continue;

            GameObject obj = null;
            if (!dicDropItems.TryGetValue(_itab.dropModel, out obj))
            {
                obj = Resources.Load<GameObject>("Prefabs/DropItems/" + _itab.dropModel);
                dicDropItems.Add(_itab.dropModel, obj);

                //Instantiate(dropList[Random.Range(0, dropList.Length)], transform.position, transform.rotation);
            }

            GameObject gameobj = GameObject.Instantiate<GameObject>(obj);
            gameobj.transform.position = spawnPos + dropPositions[i] + new Vector3(0, _itab.dropHeight, 0);
            gameobj.transform.localScale = new Vector3(_itab.dropScale, _itab.dropScale, _itab.dropScale);
            gameobj.AddComponent<DropedItem>().itemData = BaseItem.newItem(_itab); 
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
