using UnityEngine;
using System;
using System.Collections;

public class QualityProperty
{
    public const int QualityPropertyCount = 5;

    public PropertyTypeEx type = PropertyTypeEx.MAX;
    public uint value = 0;
}

[Serializable]
public class EquipItem : BaseItem
{
    //品质属性
    public QualityProperty[] qualityPropertys = new QualityProperty[QualityProperty.QualityPropertyCount];

    override public void InitItemEx()
    {
        //生成品质属性
    }
}
