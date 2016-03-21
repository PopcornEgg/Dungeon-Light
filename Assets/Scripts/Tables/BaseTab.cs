using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BaseTab<T>
{
    public int Name;

    public Dictionary<T, BaseTab<T>> dicTabs = new Dictionary<T, BaseTab<T>>();

    public List<BaseTab<T>> lsTabs = new List<BaseTab<T>>();

    public virtual void Read()
    {

    }

    public void Add(BaseTab<T> _item, T idx)
    {
        if(_item != null)
        {
            dicTabs.Add(idx, _item);
            lsTabs.Add(_item);
        }
    }
    public BaseTab<T> GetByDic(T idx)
    {
        BaseTab<T> _item;
        if (dicTabs.TryGetValue(idx, out _item))
            return _item;
        return null;
    }
    public BaseTab<T> GetByList(int idx)
    {
        if (idx >= 0 && idx < lsTabs.Count)
            return lsTabs[idx];
        return null;
    }
}
