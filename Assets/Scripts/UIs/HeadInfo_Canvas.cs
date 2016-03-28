using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

//怪物头顶信息
public class UIMonsterHeadInfo
{
    public UInt64 uid;
    public GameObject gobj;
    public Slider hpslider;
    public Text Name;
    public Text Level;
    public Character owner;
}

//掉落道具头顶信息
public class UIItemHeadInfo
{
    public UInt64 uid;
    public GameObject gobj;
    public Text Name;
    public DropedItem owner;
}

//玩家头顶信息
public class UIPlayerHeadInfo
{
    public UInt64 uid;
    public GameObject gobj;
    public Slider hpslider;
    public Text Name;
    public Text Level;
    public Character owner;
}
public class HeadInfo_Canvas : MonoBehaviour
{
    public GameObject playerHeadinfoTemp;
    public GameObject monsterHeadinfoTemp;
    public GameObject itemHeadinfoTemp;

    UIPlayerHeadInfo uhiplayer = new UIPlayerHeadInfo();
    Dictionary<UInt64, UIMonsterHeadInfo> dicMonsterHeadInfo = new Dictionary<UInt64, UIMonsterHeadInfo>();
    Dictionary<UInt64, UIItemHeadInfo> dicItemHeadInfo = new Dictionary<UInt64, UIItemHeadInfo>();
    void Awake()
    {
        StaticManager.sHeadInfo_Canvas = this;
    }
	void Start () {

        uhiplayer.owner = StaticManager.sPlayer;
        uhiplayer.uid = uhiplayer.owner.UID;
        uhiplayer.gobj = Instantiate<GameObject>(playerHeadinfoTemp);
        uhiplayer.gobj.SetActive(true);
        uhiplayer.gobj.transform.SetParent(this.transform, false);
        uhiplayer.hpslider = uhiplayer.gobj.transform.FindChild("Slider").GetComponent<Slider>();
        uhiplayer.Name = uhiplayer.gobj.transform.FindChild("Name").GetComponent<Text>();
        uhiplayer.Level = uhiplayer.gobj.transform.FindChild("Level").GetComponent<Text>();
    }
	
	void Update () {

        ShowMonsterHeadInfo();
        ShowPlayerHeadInfo();
        ShowItemHeadInfo();
    }

    void ShowMonsterHeadInfo()
    {
        foreach(UIMonsterHeadInfo uhi in dicMonsterHeadInfo.Values)
        {
            //得到头顶的世界坐标
            Vector3 position = new Vector3(uhi.owner.transform.position.x, uhi.owner.transform.position.y + uhi.owner.modelHeight, uhi.owner.transform.position.z);
            //根据头顶的3D坐标换算成它在2D屏幕中的坐标
            position = Camera.main.WorldToScreenPoint(position);
            //显示
            uhi.gobj.transform.position = new Vector3(position.x, position.y, 0);

            //显示数值
            uhi.hpslider.value = (float)uhi.owner.HP / (float)uhi.owner.MAXHP;
            uhi.Name.text = uhi.owner.Name;
            uhi.Level.text = uhi.owner.Level.ToString();
        }
    }
    void ShowPlayerHeadInfo()
    {
        //得到头顶的世界坐标
        Vector3 position = new Vector3(uhiplayer.owner.transform.position.x, 
            uhiplayer.owner.transform.position.y + uhiplayer.owner.modelHeight, 
            uhiplayer.owner.transform.position.z);
        //根据头顶的3D坐标换算成它在2D屏幕中的坐标
        position = Camera.main.WorldToScreenPoint(position);
        //显示
        uhiplayer.gobj.transform.position = new Vector3(position.x, position.y, 0);

        //显示数值
        uhiplayer.hpslider.value = (float)uhiplayer.owner.HP / (float)uhiplayer.owner.MAXHP;
        uhiplayer.Name.text = uhiplayer.owner.Name;
        uhiplayer.Level.text = uhiplayer.owner.Level.ToString();
    }

    void ShowItemHeadInfo()
    {
        foreach (UIItemHeadInfo uhi in dicItemHeadInfo.Values)
        {
            //得到头顶的世界坐标
            Vector3 position = new Vector3(uhi.owner.transform.position.x, uhi.owner.transform.position.y + 1.0f, uhi.owner.transform.position.z);
            //根据头顶的3D坐标换算成它在2D屏幕中的坐标
            position = Camera.main.WorldToScreenPoint(position);
            //显示
            uhi.gobj.transform.position = new Vector3(position.x, position.y, 0);

            //显示数值
            uhi.Name.text = uhi.owner.itemData.Name;
        }
    }

    public void AddMonsterHeadInfo(Character _o)
    {
        UIMonsterHeadInfo uhi = new UIMonsterHeadInfo();
        uhi.uid = _o.UID;
        uhi.owner = _o;
        uhi.gobj = Instantiate<GameObject>(monsterHeadinfoTemp);
        uhi.gobj.SetActive(true);
        uhi.gobj.transform.SetParent(this.transform, false);
        uhi.hpslider = uhi.gobj.transform.FindChild("Slider").GetComponent<Slider>();
        uhi.Name = uhi.gobj.transform.FindChild("Name").GetComponent<Text>();
        uhi.Level = uhi.gobj.transform.FindChild("Level").GetComponent<Text>();
        dicMonsterHeadInfo.Add(uhi.uid, uhi);
    }

    public void DelMonsterHeadInfo(UInt64 uid)
    {
        UIMonsterHeadInfo uhi;
        if(dicMonsterHeadInfo.TryGetValue(uid, out uhi))
        {
            DestroyImmediate(uhi.gobj);
            dicMonsterHeadInfo.Remove(uid);
        }
    }

    public void AddItemHeadInfo(DropedItem _o)
    {
        UIItemHeadInfo uhi = new UIItemHeadInfo();
        uhi.uid = _o.itemData.UId;
        uhi.owner = _o;
        uhi.gobj = Instantiate<GameObject>(itemHeadinfoTemp);
        uhi.gobj.SetActive(true);
        uhi.gobj.transform.SetParent(this.transform, false);
        uhi.Name = uhi.gobj.transform.FindChild("Name").GetComponent<Text>();
        dicItemHeadInfo.Add(uhi.uid, uhi);
    }
    public void DelItemHeadInfo(UInt64 uid)
    {
        UIItemHeadInfo uhi;
        if (dicItemHeadInfo.TryGetValue(uid, out uhi))
        {
            DestroyImmediate(uhi.gobj);
            dicItemHeadInfo.Remove(uid);
        }
    }
}
