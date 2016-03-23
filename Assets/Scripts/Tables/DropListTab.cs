using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//public class ItemTab : BaseTab<uint>
public class DropListTab
{
    #region ***属性数据***
    //表索引ID
    public readonly uint tabid;
    //掉落列表
    public readonly int[] droplist;
    //掉落数量
    public readonly int[] dropcount;
    #endregion

    public DropListTab(int i, TabReader tr)
    {
        //属性数据
        tabid = tr.GetItemUInt32(i, "tabid");

        //读取列表
        string[] sp;
        sp = tr.GetString(i, "droplist").Split('|');
        droplist = new int[sp.Length];
        for (int j = 0; j < sp.Length; j++)
            droplist[j] = Convert.ToInt32(sp[j]);

        sp = tr.GetString(i, "dropcount").Split('|');
        dropcount = new int[sp.Length];
        for (int j = 0; j < sp.Length; j++)
            dropcount[j] = Convert.ToInt32(sp[j]);
    }
    //静态
    public static Dictionary<uint, DropListTab> dicTabs = new Dictionary<uint, DropListTab>();

    public static void Read()
    {
        TabReader tr = new TabReader("Tables/droplisttab", true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            DropListTab _item = new DropListTab(i, tr);
            dicTabs.Add(_item.tabid, _item);
        }
    }

    public static DropListTab Get(uint tabid)
    {
        DropListTab _item;
        if (dicTabs.TryGetValue(tabid, out _item))
            return _item;
        return null;
    }
}
