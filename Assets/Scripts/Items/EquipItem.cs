using UnityEngine;
using System.Collections;

public class QualityProperty {

    public PropertyType type = PropertyType.MAX;
    public uint value = 0;
}
public class EquipItem : BaseItem
{
    //装备位置
    public ItemEquipType itemEquipType;

    //基本属性
    public int[] baseProperty = new int[(int)PropertyType.MAX];

    //品质属性
    public QualityProperty[] qualityPropertys = new QualityProperty[Config.QualityPropertyCount];

    public EquipItem()
    {
    }

    override public void InitItemEx(ItemTab _tab)
    {

    }
}
