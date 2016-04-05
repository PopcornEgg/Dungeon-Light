using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

//头顶信息基类
public class BaseHeadInfo
{
    public UInt64 uid;
    public GameObject gobj;
    public Text Name;
    public int headinfoTempIdx = 0;

    public virtual void Init() { }
}
//玩家头顶信息
public class UIPlayerHeadInfo : BaseHeadInfo
{
    public Slider hpslider;
    public Text Level;
    public Character owner;
    public UIPlayerHeadInfo()
    {
        headinfoTempIdx = 0;
    }
    public override void Init() {
        hpslider = gobj.transform.FindChild("Slider").GetComponent<Slider>();
        Name = gobj.transform.FindChild("Name").GetComponent<Text>();
        Level = gobj.transform.FindChild("Level").GetComponent<Text>();
    }
}
//怪物头顶信息
public class UIMonsterHeadInfo : BaseHeadInfo
{
    public Slider hpslider;
    public Text Level;
    public Character owner;
    public UIMonsterHeadInfo()
    {
        headinfoTempIdx = 1;
    }
    public override void Init() {
        hpslider = gobj.transform.FindChild("Slider").GetComponent<Slider>();
        Name = gobj.transform.FindChild("Name").GetComponent<Text>();
        Level = gobj.transform.FindChild("Level").GetComponent<Text>();
    }
}
//掉落道具头顶信息
public class UIItemHeadInfo : BaseHeadInfo
{
    public DropedItem owner;
    public UIItemHeadInfo()
    {
        headinfoTempIdx = 2;
    }
    public override void Init() {
        Name = gobj.transform.FindChild("Name").GetComponent<Text>();
    }
}
//NPC头顶名字
public class UINPCHeadInfo : BaseHeadInfo
{
    public Character owner;
    public UINPCHeadInfo()
    {
        headinfoTempIdx = 3;
    }
    public override void Init()
    {
        Name = gobj.transform.FindChild("Name").GetComponent<Text>();
    }
}

public class HeadInfo_Canvas : MonoBehaviour
{
    //public GameObject playerHeadinfoTemp;
    //public GameObject monsterHeadinfoTemp;
    //public GameObject itemHeadinfoTemp;

    public GameObject[] headinfoTemps;

    static List<BaseHeadInfo> lsWaittingInited = new List<BaseHeadInfo>();
    static UIPlayerHeadInfo uhiplayer;
    static Dictionary<UInt64, UIMonsterHeadInfo> dicMonsterHeadInfo = new Dictionary<UInt64, UIMonsterHeadInfo>();
    static Dictionary<UInt64, UIItemHeadInfo> dicItemHeadInfo = new Dictionary<UInt64, UIItemHeadInfo>();
    static Dictionary<UInt64, UINPCHeadInfo> dicNPCHeadInfo = new Dictionary<UInt64, UINPCHeadInfo>();

    void Update ()
    {
        if(lsWaittingInited.Count > 0)
        {
            foreach (BaseHeadInfo _headInfo in lsWaittingInited)
            {
                _headInfo.gobj = Instantiate<GameObject>(headinfoTemps[_headInfo.headinfoTempIdx]);
                _headInfo.gobj.transform.SetParent(this.transform, false);
                _headInfo.Init();
                _headInfo.gobj.SetActive(true);
            }
            lsWaittingInited.Clear();
        }

        ShowMonsterHeadInfo();
        ShowPlayerHeadInfo();
        ShowItemHeadInfo();
        ShowNPCHeadInfo();
    }
    void ShowPlayerHeadInfo()
    {
        if (uhiplayer == null)
            return;

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
    void ShowItemHeadInfo()
    {
        foreach (UIItemHeadInfo uhi in dicItemHeadInfo.Values)
        {
            //得到头顶的世界坐标
            Vector3 position = new Vector3(uhi.owner.transform.position.x, 
                uhi.owner.transform.position.y + uhi.owner.headInfoHeight, 
                uhi.owner.transform.position.z);
            //根据头顶的3D坐标换算成它在2D屏幕中的坐标
            position = Camera.main.WorldToScreenPoint(position);
            //显示
            uhi.gobj.transform.position = new Vector3(position.x, position.y, 0);

            //显示数值
            uhi.Name.text = uhi.owner.itemData.Name;
        }
    }
    void ShowNPCHeadInfo()
    {
        foreach (UINPCHeadInfo uhi in dicNPCHeadInfo.Values)
        {
            //得到头顶的世界坐标
            Vector3 position = new Vector3(uhi.owner.transform.position.x, uhi.owner.transform.position.y + uhi.owner.modelHeight, uhi.owner.transform.position.z);
            //根据头顶的3D坐标换算成它在2D屏幕中的坐标
            position = Camera.main.WorldToScreenPoint(position);
            //显示
            uhi.gobj.transform.position = new Vector3(position.x, position.y, 0);

            uhi.Name.text = uhi.owner.Name;
        }
    }
    public static void AddPlayerHeadInfo(Character _o)
    {
        uhiplayer = new UIPlayerHeadInfo();
        uhiplayer.uid = _o.UID;
        uhiplayer.owner = _o;
        lsWaittingInited.Add(uhiplayer);
    }
    public static void AddMonsterHeadInfo(Character _o)
    {
        UIMonsterHeadInfo uhi = new UIMonsterHeadInfo();
        uhi.uid = _o.UID;
        uhi.owner = _o;
        dicMonsterHeadInfo.Add(uhi.uid, uhi);
        lsWaittingInited.Add(uhi);
    }

    public static void DelMonsterHeadInfo(UInt64 uid)
    {
        UIMonsterHeadInfo uhi;
        if(dicMonsterHeadInfo.TryGetValue(uid, out uhi))
        {
            DestroyImmediate(uhi.gobj);
            dicMonsterHeadInfo.Remove(uid);
        }
    }
    public static void AddItemHeadInfo(DropedItem _o)
    {
        UIItemHeadInfo uhi = new UIItemHeadInfo();
        uhi.uid = _o.itemData.UId;
        uhi.owner = _o;
        dicItemHeadInfo.Add(uhi.uid, uhi);
        lsWaittingInited.Add(uhi);
    }
    public static void DelItemHeadInfo(UInt64 uid)
    {
        UIItemHeadInfo uhi;
        if (dicItemHeadInfo.TryGetValue(uid, out uhi))
        {
            DestroyImmediate(uhi.gobj);
            dicItemHeadInfo.Remove(uid);
        }
    }
    public static void AddNPCHeadInfo(Character _o)
    {
        UINPCHeadInfo uhi = new UINPCHeadInfo();
        uhi.uid = _o.UID;
        uhi.owner = _o;
        dicNPCHeadInfo.Add(uhi.uid, uhi);
        lsWaittingInited.Add(uhi);
    }
    public static void DelNPCHeadInfo(UInt64 uid)
    {
        UINPCHeadInfo uhi;
        if (dicNPCHeadInfo.TryGetValue(uid, out uhi))
        {
            DestroyImmediate(uhi.gobj);
            dicNPCHeadInfo.Remove(uid);
        }
    }
}
