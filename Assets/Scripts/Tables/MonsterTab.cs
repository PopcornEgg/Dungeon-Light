using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//public class ItemTab : BaseTab<uint>
public class MonsterTab
{
    static System.Random random = new System.Random();

    #region ***属性数据***
    //表索引ID
    public uint tabid;
    //类型
    public CharacterType type;
    //名字
    public String name;
    //售价
    public MonsterType mtype;
    //装备等级/需求等级
    public int level;
    //掉落列表id
    public int[] droplist;
    //掉落概率（综合1万）
    public int[] droprate;
    #endregion

    #region*** 外观数据***
    //掉落外观
    public string model;
    //掉落高度
    public float height;
    //掉落缩放
    public float scale;
    #endregion

    //静态
    public static Dictionary<uint, MonsterTab> dicTabs = new Dictionary<uint, MonsterTab>();

    public static void Read()
    {
        TabReader tr = new TabReader("Tables/monstertab", true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            MonsterTab _item = new MonsterTab();
            //属性数据
            _item.tabid = tr.GetItemUInt32(i, "tabid");
            _item.type = (CharacterType)tr.GetItemUInt32(i, "type");
            _item.name = tr.GetString(i, "name");
            _item.mtype = (MonsterType)tr.GetItemUInt32(i, "mtype");
            _item.level = tr.GetItemInt32(i, "level");
            
            //读取列表
            string[] sp;
            sp = tr.GetString(i, "droplist").Split('|');
            _item.droplist = new int[sp.Length];
            for (int j = 0; j < sp.Length; j++)
                _item.droplist[j] = Convert.ToInt32(sp[j]);

            sp = tr.GetString(i, "droprate").Split('|');
            _item.droprate = new int[sp.Length];
            int add = 0;
            for (int j = 0; j < sp.Length; j++)
            {
                add += Convert.ToInt32(sp[j]);
                _item.droprate[j] = add;
            }

            //外观数据
            _item.model = tr.GetString(i, "model");
            _item.height = tr.GetItemFloat(i, "height");
            _item.scale = tr.GetItemFloat(i, "scale");

            dicTabs.Add(_item.tabid, _item);
        }
    }

    public static MonsterTab Get(uint tabid)
    {
        MonsterTab _item;
        if (dicTabs.TryGetValue(tabid, out _item))
            return _item;
        return null;
    }

    public int GetDropListIdx()
    {
        int rate = random.Next(0, 10000);
        for(int idx=0;idx < droprate.Length; ++idx)
        {
            if (droprate[idx] > rate)
                return droplist[idx];
        }
        return -1;
    }
}
