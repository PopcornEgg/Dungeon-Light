using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TabManager
{
    static readonly string[] preloadTabs = new string[] {

    };

    public static ItemTab itemTab = new ItemTab();
    static public void Load()
    {
        itemTab.Read();
    }
}
