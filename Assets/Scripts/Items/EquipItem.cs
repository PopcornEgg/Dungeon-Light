using UnityEngine;
using System.Collections;

public class QualityProperty {

    public PropertyTypeEx type = PropertyTypeEx.MAX;
    public uint value = 0;
}
public class EquipItem : BaseItem
{
    //装备位置
    public ItemEquipType itemEquipType;

    //基本属性
    public float[] propertyRandom = new float[(int)PropertyTypeEx.MAX];

    //品质属性
    public QualityProperty[] qualityPropertys = new QualityProperty[Config.QualityPropertyCount];

    public override int GetCount()
    {
        return 1;
    }

    override public void InitItemEx()
    {
    }
}
