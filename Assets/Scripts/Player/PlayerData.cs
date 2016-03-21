using UnityEngine;
using System.Collections;

//player数据处理部分
public partial class Player : Character
{
    //背包
    public static int bagSpace = 50;//背包容量
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
    public void AddBagItem(BaseItem _item)
    {
        for(int i=0;i< bagItems.Length; i++)
        {
            if (bagItems[i] == null)
                bagItems[i] = _item;
        }
    }
    public void DelBagItem(int idx)
    {
        if (idx < 0 || idx >= bagItems.Length)
            return;

        bagItems[idx] = null;
    }


    //身上装备
    public EquipItem[] bodyEuiqpItems = new EquipItem[(int)ItemEquipType.MAX];
}
