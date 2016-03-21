using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemTab : BaseTab<uint>
{
    //表索引ID
    public uint tabId;
    //类型
    public ItemType type = ItemType.NULL;
    //名字
    public String name;
    //售价
    public int price;
    //装备等级 / 需求等级
    public int level;
    //品质
    public int quality;

    public override void Read()
    {

    }
}
