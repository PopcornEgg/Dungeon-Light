using UnityEngine;
using System;
using System.Collections;

public class QualityProperty {

    public PropertyTypeEx type = PropertyTypeEx.MAX;
    public uint value = 0;
}

[Serializable]
public class EquipItem : BaseItem
{
    //品质属性
    public QualityProperty[] qualityPropertys = new QualityProperty[Config.QualityPropertyCount];

    public override int GetCount()
    {
        return 1;
    }

    override public void InitItemEx()
    {
        //生成品质属性
    }
}
