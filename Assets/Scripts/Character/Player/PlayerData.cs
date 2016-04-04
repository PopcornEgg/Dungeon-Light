using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class PlayerBaseDataSave
{
    [NonSerialized]
    static String playerbasedatasave = Application.persistentDataPath + "/playerbasedatasave.data";

    UInt64 uId;
    String Name;
    UInt32 level;
    UInt32 sex;
    int[] property;
    float[] bornPosition = new float[3];

    public static void Save(Player player)
    {
        PlayerBaseDataSave buffer = new PlayerBaseDataSave();
        buffer.uId = player.UID;
        buffer.Name = player.Name;
        buffer.level = player.Level;
        buffer.sex = player.Sex;
        buffer.property = player.Property;
        buffer.bornPosition[0] = player.transform.position.x;
        buffer.bornPosition[1] = player.transform.position.y;
        buffer.bornPosition[2] = player.transform.position.z;

        Utils.BinarySerialize.Serialize<PlayerBaseDataSave>(buffer, playerbasedatasave);
    }
    public static void Load(Player player)
    {
        PlayerBaseDataSave buffer = Utils.BinarySerialize.DeSerialize<PlayerBaseDataSave>(playerbasedatasave);
        if (buffer != null)
        {
            player.UID = buffer.uId;
            player.Name = buffer.Name;
            player.Level = buffer.level;
            player.Sex = buffer.sex;
            player.Property = buffer.property;
            player.bornPosition = new Vector3(buffer.bornPosition[0], buffer.bornPosition[1], buffer.bornPosition[2]); 
        }
        else
        {
            player.CreatePlayer("李连杰");
        }
    }
}
//player数据处理部分
public partial class Player : Character
{
    #region 公用*********************************************************************************
    public void SaveAll()
    {
        PlayerBaseDataSave.Save(this);
        SaveBagItems();
        SaveBodyItems();
    }
    public void LoadAll()
    {
        PlayerBaseDataSave.Load(this);
        LoadBagItems();
        LoadBodyItems();
    }
    #endregion

    #region 背包*********************************************************************************

    //"C:/Users/Administrator/AppData/LocalLow/DefaultCompany/Dungeon Light/bagitems.data"
    static String bagItemsSavePath = Application.persistentDataPath + "/bagitems.data";
    public const int bagSpace = 50;//背包容量
    public BaseItem[] bagItems = new BaseItem[bagSpace];
    public int CurrBagItemCount {
        get {
            int _count = 0;
            for (int i = 0; i < bagItems.Length; i++)
            {
                if (bagItems[i] != null)
                    _count++;
            }
            return _count;
        }
    }
    public bool IsBagFull
    {
        get
        {
            return CurrBagItemCount >= bagSpace;
        }
    }
    public bool AddBagItem(BaseItem _item)
    {
        if(_item.TabData.overlay > 0)
        {
            for (int i = 0; i < bagItems.Length; i++)
            {
                if (bagItems[i] != null && bagItems[i].TabId == _item.TabId && bagItems[i].OverlayCount < _item.TabData.overlay)
                {
                    bagItems[i].OverlayCount++;
                    return true;
                }
            }
        }
        for (int i = 0; i < bagItems.Length; i++)
        {
            if (bagItems[i] == null)
            {
                bagItems[i] = _item;
                return true;
            }
        }
        return false;
    }
    public void DelBagItem(int idx)
    {
        if (idx < 0 || idx >= bagItems.Length)
            return;

        bagItems[idx] = null;
    }
    public void UseBagItem(int idx)
    {
        BaseItem _item = bagItems[idx];
        if (_item == null)
            return;

        switch (_item.Type)
        {
            case ItemType.EQUIP:
                {
                    UseBagEquip((EquipItem)_item, idx);
                    break;
                }
            case ItemType.MEDICINE:
                {
                    UseBagMedicine((MedicineItem)_item, idx);
                    break;
                }
            default:
                break;
        }
    }
    public void UseBagEquip(EquipItem _item, int bagIdx)
    {
        int bodyIdx = (int)_item.EquipType;
        if (_item.EquipType == ItemEquipType.Accessory)//特殊处理
        {
            if (bodyEuiqpItems[bodyIdx - 1] == null)
            {
                bodyEuiqpItems[bodyIdx - 1] = _item;
                bagItems[bagIdx] = null;
            }
            else if (bodyEuiqpItems[bodyIdx] == null)
            {
                bodyEuiqpItems[bodyIdx] = _item;
                bagItems[bagIdx] = null;
            }
            else//默认替换第一个饰品
            {
                BaseItem buffer = bodyEuiqpItems[bodyIdx - 1];
                bodyEuiqpItems[bodyIdx - 1] = _item;
                bagItems[bagIdx] = buffer;
            }
        }
        else
        {
            if (bodyEuiqpItems[bodyIdx] == null)
            {
                bodyEuiqpItems[bodyIdx] = _item;
                bagItems[bagIdx] = null;
            }
            else
            {
                BaseItem buffer = bodyEuiqpItems[bodyIdx];
                bodyEuiqpItems[bodyIdx] = _item;
                bagItems[bagIdx] = buffer;
            }
        }
        Second_Canvas.RefreshPlayerBag();
        Second_Canvas.RefreshPlayerProperty();
        playerEquipProperty.IsDirty = true;

        ChangeWeaponModel(bodyIdx);
    }
    public void UseBagMedicine(MedicineItem _item, int bagIdx)
    {
        int bodyIdx = (int)_item.EquipType;
        HP += (uint)_item.TabData.propertyEx[(int)PropertyTypeEx.MAXHP];
        HP = HP > (uint)MAXHP ? (uint)MAXHP : HP;
        MP += (uint)_item.TabData.propertyEx[(int)PropertyTypeEx.MAXMP];
        MP = MP > (uint)MAXMP ? (uint)MAXMP : MP;
        SP += (uint)_item.TabData.propertyEx[(int)PropertyTypeEx.MAXSP];
        SP = SP > (uint)MAXSP ? (uint)MAXSP : SP;
        if (--bagItems[bagIdx].OverlayCount <= 0)
            bagItems[bagIdx] = null;
        Second_Canvas.RefreshPlayerBag();
    }
    public void SaveBagItems()
    {
        Utils.BinarySerialize.Serialize<BaseItem[]>(bagItems, bagItemsSavePath);
    }
    public void LoadBagItems()
    {
        BaseItem[] datas = Utils.BinarySerialize.DeSerialize<BaseItem[]>(bagItemsSavePath);
        if (datas != null)
        {
            bagItems = datas;
            for (int i = 0; i < bagItems.Length; i++)
            {
                if (bagItems[i] != null)
                {
                    bagItems[i].TabData = ItemTab.Get(bagItems[i].TabId);
                }
            }
        }
    }
    #endregion

    #region 身上装备*********************************************************************************
    static String bodyItemsSavePath = Application.persistentDataPath + "/bodyitems.data";
    PlayerEquipProperty playerEquipProperty;
    public BaseItem[] bodyEuiqpItems = new BaseItem[(int)ItemEquipType.MAX];
    public Transform rightWeaponNode;
    public void UseBodyItem(int idx)
    {
        //是否装备
        if (bodyEuiqpItems[idx] == null)
            return;
        
        //背包是否已满
        if (IsBagFull)
            return;

        //取下
        AddBagItem(bodyEuiqpItems[idx]);
        bodyEuiqpItems[idx] = null;

        Second_Canvas.RefreshPlayerBag();
        Second_Canvas.RefreshPlayerProperty();
        playerEquipProperty.IsDirty = true;

        ChangeWeaponModel(idx);
    }
    public void SaveBodyItems()
    {
        Utils.BinarySerialize.Serialize<BaseItem[]>(bodyEuiqpItems, bodyItemsSavePath);
    }
    public void LoadBodyItems()
    {
        BaseItem[] datas = Utils.BinarySerialize.DeSerialize<BaseItem[]>(bodyItemsSavePath);
        if (datas != null)
        {
            bodyEuiqpItems = datas;
            for (int i = 0; i < bodyEuiqpItems.Length; i++)
            {
                if (bodyEuiqpItems[i] != null)
                {
                    bodyEuiqpItems[i].TabData = ItemTab.Get(bodyEuiqpItems[i].TabId);
                }
            }
        }
    }
    public void ChangeWeaponModel(int bodyIdx)
    {
        if(bodyEuiqpItems[bodyIdx] == null)
        {
            //test
            //RWeaponPosition
            //GameObject obj = PrefabsManager.Get("Items/Weapons/Blaster Sword");
            GameObject gameobj = PrefabsManager.Instantiate(PrefabsType.Items, "Weapons/Blaster Sword");
            gameobj.transform.SetParent(rightWeaponNode, false);
        }
        else
        {

        }
    }
    #endregion

    #region 技能*********************************************************************************

    public override void AddSkillExp(UInt32 _skillid, UInt32 _exp)
    {
        HasSkills.SkillData _csd = characterSkill.hasSkills.GetSkill(_skillid);
        if (_csd != null)
        {
            _csd.AddExp(_exp);
        }
    }
    #endregion

    #region 人物属性*********************************************************************************

    public override void InitProperty()
    {
        //注册等级属性模块
        playerLevelProperty = new PlayerLevelProperty(this);
        characterProperties.AddProperty(playerLevelProperty);
        //注册装备属性模块
        playerEquipProperty = new PlayerEquipProperty(this);
        characterProperties.AddProperty(playerEquipProperty);

        //加载一些存档
        LoadAll();

        CurrPlayerLvTab = PlayerLvTab.Get(Level);

        if (HP == 0)
            ReLive();
        // StaticManager.sHUD_Canvas.SetHPInfo(HP, MAXHP);
    }
    public String GetPropertyString()
    {
        StringBuilder txt = new StringBuilder();
        txt.Append(String.Format("{0}：{1}/{2}\n", PropertyNames.Names[(int)PropertyTypeEx.MAXHP], HP, MAXHP));
        txt.Append(String.Format("{0}：{1}/{2}\n", PropertyNames.Names[(int)PropertyTypeEx.MAXMP], MP, MAXMP));
        txt.Append(String.Format("{0}：{1}/{2}\n", PropertyNames.Names[(int)PropertyTypeEx.MAXSP], SP, MAXSP));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyTypeEx.AD], AD));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyTypeEx.AP], AP));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyTypeEx.ADD], ADD));
        txt.Append(String.Format("{0}：{1}\n", PropertyNames.Names[(int)PropertyTypeEx.APD], APD));
        txt.Append(String.Format("{0}：{1}", PropertyNames.Names[(int)PropertyTypeEx.MOVESPEED], MOVESPEED));

        return txt.ToString();
    }
    public void CreatePlayer(String rolename)
    {
        CurrPlayerLvTab = PlayerLvTab.Get(Level);
        UID = Utils.GuidMaker.GenerateUInt64();
        Name = rolename;
        Sex = 1;
        Level = 1;
        HP = (uint)MAXHP;
        MP = (uint)MAXMP;
        SP = (uint)MAXSP;
        EXP = 0;
        MONEY = 0;
    }
    public void ReLive()
    {
        HP = (uint)MAXHP;
        MP = (uint)MAXMP;
        SP = (uint)MAXSP;

        transform.position = Vector3.zero;

        AIState = CharacterAnimState.Idle;
        if(anim != null)
            anim.SetTrigger("Relive");
    }
    //等级属性
    PlayerLevelProperty playerLevelProperty;
    PlayerLvTab currPlayerLvTab;
    public PlayerLvTab CurrPlayerLvTab { get { return currPlayerLvTab; } set { currPlayerLvTab = value; playerLevelProperty.IsDirty = true; } }
    public override void AddExp( UInt32 _exp)
    {
        //最大等级了
        if (CurrPlayerLvTab == null)
            return;
        EXP += _exp;
        uint oldLv = Level;
        while (EXP >= CurrPlayerLvTab.exp)
        {
            Level++;
            EXP -= (uint)CurrPlayerLvTab.exp;
            CurrPlayerLvTab = PlayerLvTab.Get(Level);
        }
        //是否升级
        if (Level > oldLv)
        {
            HP = (uint)MAXHP;
            MP = (uint)MAXMP;
            SP = (uint)MAXSP;
        }
    }
    #endregion
}
