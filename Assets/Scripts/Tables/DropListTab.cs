using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//public class ItemTab : BaseTab<uint>
public class DropListTab
{
    #region ***属性数据***
    //表索引ID
    public uint tabid;
    //掉落列表
    public int[] droplist;
    //掉落数量
    public int[] dropcount;
    #endregion

    //静态
    public static Dictionary<uint, DropListTab> dicTabs = new Dictionary<uint, DropListTab>();

    public static void Read()
    {
        TabReader tr = new TabReader("Tables/droplisttab", true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            DropListTab _item = new DropListTab();
            //属性数据
            _item.tabid = tr.GetItemUInt32(i, "tabid");
            
            //读取列表
            string[] sp;
            sp = tr.GetString(i, "droplist").Split('|');
            _item.droplist = new int[sp.Length];
            for (int j = 0; j < sp.Length; j++)
                _item.droplist[j] = Convert.ToInt32(sp[j]);

            sp = tr.GetString(i, "dropcount").Split('|');
            _item.dropcount = new int[sp.Length];
            for (int j = 0; j < sp.Length; j++)
                _item.dropcount[j] = Convert.ToInt32(sp[j]);

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
