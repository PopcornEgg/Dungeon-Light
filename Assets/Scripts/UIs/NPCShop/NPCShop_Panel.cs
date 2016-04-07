using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShopItemData
{
    public uint tabId;
    public int leftCount;
}
public class NPCShop_Panel : MonoBehaviour
{
    public static int shopSpace = 50;
    public static ShopType shopType;

    public GameObject itemObj;
    RectTransform content;//内容
    GridLayoutGroup grid;
    ScrollRect scrollRect;

    NPCShop_Item[] listItems = new NPCShop_Item[shopSpace];
    bool isInited = false;


    Dictionary<uint, ShopItemData> shopItemDatas = new Dictionary<uint, ShopItemData>();

    void Awake()
    {
        //gameObject.SetActive(false);
    }
    void Start()
    {
        if (itemObj == null)
            return;

        Transform list = transform.FindChild("List");
        scrollRect = list.gameObject.GetComponent<ScrollRect>();
        content = scrollRect.content;
        grid = content.GetComponent<GridLayoutGroup>();

        //设置item高宽
        RectTransform rtfViewport = list.FindChild("Viewport").GetComponent<RectTransform>();
        float itemWH = (rtfViewport.rect.width - grid.spacing.x * (float)(grid.constraintCount - 1) - grid.padding.left - grid.padding.right) / grid.constraintCount;
        grid.cellSize = new Vector2(itemWH, itemWH);

        //设置content高度
        int rowCount = shopSpace / grid.constraintCount;
        float contentWH = itemWH * rowCount + grid.spacing.y * (float)(rowCount - 1) + grid.padding.top + grid.padding.bottom;
        content.sizeDelta = new Vector2(0, contentWH);

        for (int i = 0; i < shopSpace; i++)
        {
            GameObject cobj = Instantiate<GameObject>(itemObj);
            listItems[i] = cobj.AddComponent<NPCShop_Item>();
            listItems[i].Idx = i;
            cobj.transform.SetParent(content, false);
            cobj.SetActive(true);
        }
        itemObj.SetActive(false);

        isInited = true;
        OnEnable();
    }
    void OnEnable()
    {
        if (isInited)
            Refresh();
    }

    public int GetItemLeftCount(uint itemid, int initCount)
    {
        ShopItemData sidata;
        if (!shopItemDatas.TryGetValue(itemid, out sidata))
        {
            NPCShopTab nstab = NPCShopTab.Get((uint)shopType);
            sidata = new ShopItemData();
            sidata.leftCount = initCount;
        }
        return sidata.leftCount;
    }
    public void SubItemLeftCount(uint itemid, int _count)
    {
        ShopItemData sidata;
        if (shopItemDatas.TryGetValue(itemid, out sidata))
        {
            sidata.leftCount -= _count;
        }
    }
    public void Refresh()
    {
        if (!gameObject.activeSelf)
            return;

        NPCShopTab nstab = NPCShopTab.Get((uint)shopType);
        for (int i = 0; i < listItems.Length; i++)
        {
            if(i < nstab.idlist.Length)
            {
                ItemTab _item = ItemTab.Get(nstab.idlist[i]);
                listItems[i].Set(true, _item.icon, GetItemLeftCount(nstab.idlist[i], nstab.buycount[i]));
            }
            else
            {
                listItems[i].Set(false);
            }
        }
    }
    public void Show( bool isShow, ShopType _type = ShopType.Max)
    {
        if(isShow)
            shopType = _type;
        gameObject.SetActive(isShow);
    }
}
