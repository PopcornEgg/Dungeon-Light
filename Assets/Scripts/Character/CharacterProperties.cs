using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CharacterProperties
{
    List<CommonProperty> lsProperties = new List<CommonProperty>();

    public float Get(PropertyTypeEx ptypeex )
    {
        float sum = 0;
        foreach(CommonProperty _cp in lsProperties)
        {
            if (_cp.IsDirty)
                _cp.Refresh();
            sum += _cp.propertyEx[(int)ptypeex];
        }
        return sum;
    }

    public void AddProperty(CommonProperty _cp)
    {
        lsProperties.Add(_cp);
    }
}
public class CommonProperty
{
    //脏标记
    bool isDirty = true;
    public bool IsDirty
    {
        get { return isDirty; }
        set { isDirty = value; }
    }

    public float[] propertyEx = new float[(int)PropertyTypeEx.MAX];
    
    //public float MAXHP { get { if (IsDirty) Refresh();  return propertyEx[(int)PropertyTypeEx.MAXHP]; }  }
    //public float MAXMP { get { if (IsDirty) Refresh(); return propertyEx[(int)PropertyTypeEx.MAXMP]; } }
    //public float MAXSP { get { if (IsDirty) Refresh(); return propertyEx[(int)PropertyTypeEx.MAXSP]; }  }
    //public float AD { get { if (IsDirty) Refresh(); return propertyEx[(int)PropertyTypeEx.AD]; }  }
    //public float ADD { get { if (IsDirty) Refresh(); return propertyEx[(int)PropertyTypeEx.ADD]; }  }
    //public float AP { get { if (IsDirty) Refresh(); return propertyEx[(int)PropertyTypeEx.AP]; } }
    //public float APD { get { if (IsDirty) Refresh(); return propertyEx[(int)PropertyTypeEx.APD]; }  }
    //public float MOVESPEED { get { if (IsDirty) Refresh(); return propertyEx[(int)PropertyTypeEx.MOVESPEED]; }  }

    public virtual void Refresh() { }
    public void Clear()
    {
        for (int j = 0; j < propertyEx.Length; j++)
        {
            propertyEx[j] = 0;
        }
    }
}
public class PlayerLevelProperty : CommonProperty
{
    Player player;
    public PlayerLevelProperty(Player _player)
    {
        player = _player;
    }
    public override void Refresh()
    {
        if (IsDirty)
        {
            Clear();
            PlayerLvTab currPlayerLvTab = player.CurrPlayerLvTab;
            propertyEx[(int)PropertyTypeEx.MAXHP] = currPlayerLvTab.maxhp;
            propertyEx[(int)PropertyTypeEx.MAXMP] = currPlayerLvTab.maxmp;
            propertyEx[(int)PropertyTypeEx.AD] = currPlayerLvTab.ad;
            propertyEx[(int)PropertyTypeEx.AP] = currPlayerLvTab.ap;
            propertyEx[(int)PropertyTypeEx.ADD] = currPlayerLvTab.add;
            propertyEx[(int)PropertyTypeEx.APD] = currPlayerLvTab.apd;
            propertyEx[(int)PropertyTypeEx.MOVESPEED] = currPlayerLvTab.movespeed;
            IsDirty = false;
        }
    }
}
public class MonsterBaseProperty : CommonProperty
{
    Monster monster;
    public MonsterBaseProperty(Monster _monster)
    {
        monster = _monster;
    }
    public override void Refresh()
    {
        if (IsDirty)
        {
            Clear();
            MonsterTab monsterTab = monster.monsterTab;
            for (int j = 0; j < (int)PropertyTypeEx.MAX; j++)
            {
                propertyEx[j] += monsterTab.propertyEx[j];
            }
            IsDirty = false;
        }
    }
}
public class PlayerEquipProperty : CommonProperty
{
    Player player;
    public PlayerEquipProperty(Player _player)
    {
        player = _player;
    }
    public override void Refresh()
    {
        if (IsDirty)
        {
            Clear();
            BaseItem[] bodyItems = player.bodyEuiqpItems;
            for (int i = 0; i < bodyItems.Length; i++)
            {
                if(bodyItems[i] != null)
                {
                    for (int j = 0; j < (int)PropertyTypeEx.MAX; j++)
                    {
                        propertyEx[j] += bodyItems[i].TabData.propertyEx[j];
                    }
                }
            }
            IsDirty = false;
        }
    }
}
