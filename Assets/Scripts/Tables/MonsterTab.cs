﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//public class ItemTab : BaseTab<uint>
public class MonsterTab
{
    static System.Random random = new System.Random();

    #region ***属性数据***
    //表索引ID
    public readonly uint tabid;
    //类型
    public readonly CharacterType type;
    //名字
    public readonly String name;
    //怪物类型
    public readonly MonsterType mtype;
    //装备等级/需求等级
    public readonly int level;
    //扫描范围
    public readonly float scanrange;
    //追踪范围
    public readonly float followrange;

    //攻防属性
    public readonly int maxhp;
    public readonly int maxmp;
    public readonly int ad;
    public readonly int ap;
    public readonly int add;
    public readonly int apd;
    public readonly int movespeed;

    //掉落列表id
    public readonly int[] droplist;
    //掉落概率（综合1万）
    public readonly int[] droprate;
    #endregion

    #region*** 外观数据***
    //掉落外观
    public readonly string model;
    //掉落高度
    public readonly float height;
    //掉落缩放
    public readonly float scale;
    #endregion

    public MonsterTab(int i, TabReader tr)
    {
        //属性数据
        tabid = tr.GetItemUInt32(i, "tabid");
        type = (CharacterType)tr.GetItemUInt32(i, "type");
        name = tr.GetString(i, "name");
        mtype = (MonsterType)tr.GetItemUInt32(i, "mtype");
        level = tr.GetItemInt32(i, "level");
        scanrange = tr.GetItemFloat(i, "scanrange");
        followrange = tr.GetItemFloat(i, "followrange");

        //攻防属性
        maxhp = tr.GetItemInt32(i, "maxhp");
        maxmp = tr.GetItemInt32(i, "maxmp");
        ad = tr.GetItemInt32(i, "ad");
        ap = tr.GetItemInt32(i, "ap");
        add = tr.GetItemInt32(i, "add");
        apd = tr.GetItemInt32(i, "apd");
        movespeed = tr.GetItemInt32(i, "movespeed");

        //读取列表
        string[] sp;
        sp = tr.GetString(i, "droplist").Split('|');
        droplist = new int[sp.Length];
        for (int j = 0; j < sp.Length; j++)
            droplist[j] = Convert.ToInt32(sp[j]);

        sp = tr.GetString(i, "droprate").Split('|');
        droprate = new int[sp.Length];
        int sum = 0;
        for (int j = 0; j < sp.Length; j++)
        {
            sum += Convert.ToInt32(sp[j]);
            droprate[j] = sum;
        }

        //外观数据
        model = tr.GetString(i, "model");
        height = tr.GetItemFloat(i, "height");
        scale = tr.GetItemFloat(i, "scale");
    }
    //静态
    public static Dictionary<uint, MonsterTab> dicTabs = new Dictionary<uint, MonsterTab>();

    public static void Read()
    {
        TabReader tr = new TabReader("Tables/monstertab", true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            MonsterTab _item = new MonsterTab(i, tr);
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
