﻿using UnityEngine;
using System.Collections;

public enum ItemType
{
    MONEY = 0,
    MEDICINE,
    EQUIP,

    MAX,
}

//装备使用的位置
public enum ItemEquipType
{
    Head = 0,
    Body,
    Hand,
    Leg,
    Accessory = Leg + 2,//饰品两个

    MAX,
}