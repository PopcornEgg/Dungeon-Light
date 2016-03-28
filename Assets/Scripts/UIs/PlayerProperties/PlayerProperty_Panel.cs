using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PropertyEquipOptimizer
{
    public UInt64[] equipUids = new UInt64[(int)ItemEquipType.MAX];

    public PropertyEquipOptimizer()
    {
        for (int i = 0; i < equipUids.Length; i++)
            equipUids[i] = UInt64.MaxValue;
    }
    public bool IsChanged(int idx, UInt64 realUid)
    {
        if (equipUids[idx] != realUid)
        {
            equipUids[idx] = realUid;
            return true;
        }
        return false;
    }
}
public class PlayerProperty_Panel : MonoBehaviour {

    bool isInited = false;

    Player player;

    //左边
    Text Name;
    PropertyEquip_Item[] equipItems = new PropertyEquip_Item[(int)ItemEquipType.MAX];
    PropertyEquipOptimizer propertyEquipOptimizer = new PropertyEquipOptimizer();

    //右边
    Text propertyInfo;

    void Start ()
    {
        player = StaticManager.sPlayer;

        //左边
        Transform BodyEquip = transform.FindChild("BodyEquip");
        Transform Equips = BodyEquip.FindChild("Equips");
        Name = Equips.FindChild("Name").GetComponent<Text>();
        for (int i=0;i< equipItems.Length; i++)
        {
            string idx = "Item" + i.ToString();
            equipItems[i] = Equips.FindChild(idx).gameObject.AddComponent<PropertyEquip_Item>();
        }

        //右边
        Transform Information = transform.FindChild("Information");
        propertyInfo = Information.FindChild("Property").FindChild("Info").GetComponent<Text>();
       
        isInited = true;
        OnEnable();
    }
    public void OnEnable()
    {
        if (isInited)
        {
            if (!gameObject.activeSelf)
                return;
            RefreshBodyEquip();
            RefreshInformation();
        }
    }
    public void RefreshBodyEquip()
    {
        Name.text = string.Format("LV.{0}   {1}", player.Level, player.Name);
        BaseItem[] bodyItems = player.bodyEuiqpItems;
        for (int i = 0; i < bodyItems.Length; i++)
        {
            UInt64 _uid = bodyItems[i] != null ? bodyItems[i].UId : 0;
            if (propertyEquipOptimizer.IsChanged(i, _uid))
            {
                equipItems[i].baseItem = bodyItems[i];
            }
        }
        
    }
    public void RefreshInformation()
    {
        propertyInfo.text = player.GetPropertyString();
    }
}
