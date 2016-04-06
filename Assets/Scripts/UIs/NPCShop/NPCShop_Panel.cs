using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class NPCShop_Panel : MonoBehaviour
{
    public static int shopSpace = 50;
    bool isInited = false;
    ShopType shopType;

    public GameObject itemObj;
    RectTransform content;//内容
    GridLayoutGroup grid;
    ScrollRect scrollRect;

    NPCShop_Item[] shopItems = new NPCShop_Item[shopSpace];

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
            shopItems[i] = cobj.AddComponent<NPCShop_Item>();
            shopItems[i].Idx = i;
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
    public void Refresh()
    {
        if (!gameObject.activeSelf)
            return;

        for (int i = 0; i < shopItems.Length; i++)
        {
        }
    }
    public void Show( bool isShow, ShopType _type = ShopType.Null)
    {
        if(isShow)
            shopType = _type;
        gameObject.SetActive(isShow);
    }
}
