using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//public class ItemTab : BaseTab<uint>
public class LevelTab
{
    public class Data {

        public readonly uint tabid;
        public readonly float x;
        public readonly float y;
        public readonly float z;
        public readonly uint monsterid;
        public readonly int rate;
        public readonly int space;

        public Data(int i, TabReader tr)
        {
            //属性数据
            tabid = tr.GetItemUInt32(i, "tabid");
            x = tr.GetItemFloat(i, "x");
            y = tr.GetItemFloat(i, "y");
            z = tr.GetItemFloat(i, "z");
            monsterid = tr.GetItemUInt32(i, "monsterid");
            rate = tr.GetItemInt32(i, "rate");
            space = tr.GetItemInt32(i, "space");
        }
    }
   
    public List<Data> lsTabs = new List<Data>();

    public int Count {get { return lsTabs.Count; } }
    public void Read(String filename)
    {
        TabReader tr = new TabReader("Tables/" + filename, true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            Data _item = new Data(i, tr);
            lsTabs.Add(_item);
        }
    }

    public Data Get(int tabid)
    {
        if (tabid >=0 && tabid < lsTabs.Count)
            return lsTabs[tabid];
        return null;
    }
}
public class SceneTab
{
    static System.Random random = new System.Random();

    public readonly uint tabid;
    public readonly String name;
    public readonly String idxname;
    public readonly int normal;
    public readonly int enchanted;
    public readonly int elitist;
    public readonly int boss;
    public LevelTab levelTab;

    public SceneTab(int i, TabReader tr)
    {
        //属性数据
        tabid = tr.GetItemUInt32(i, "tabid");
        name = tr.GetString(i, "name");
        idxname = tr.GetString(i, "idxname");
        normal = tr.GetItemInt32(i, "normal");
        enchanted = tr.GetItemInt32(i, "enchanted");
        elitist = tr.GetItemInt32(i, "elitist");
        boss = tr.GetItemInt32(i, "boss");
    }
    //静态
    public static Dictionary<uint, SceneTab> dicTabs = new Dictionary<uint, SceneTab>();

    public static void Read()
    {
        TabReader tr = new TabReader("Tables/scenetab", true);
        for (int i = 0; i < tr.recordCount; i++)
        {
            SceneTab _item = new SceneTab(i, tr);
            dicTabs.Add(_item.tabid, _item);
        }
        //加载leveltab
        foreach(SceneTab _stab in dicTabs.Values)
        {
            _stab.levelTab = new LevelTab();
            _stab.levelTab.Read(_stab.idxname);
        }
    }

    public static SceneTab Get(uint tabid)
    {
        SceneTab _item;
        if (dicTabs.TryGetValue(tabid, out _item))
            return _item;
        return null;
    }
}

