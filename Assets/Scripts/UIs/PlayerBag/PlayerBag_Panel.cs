using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

//背包优化器
public class PlayerBagOptimizer
{
    public UInt64[] itemUids = new UInt64[Player.bagSpace];

    public PlayerBagOptimizer()
    {
        for (int i = 0; i < itemUids.Length; i++)
            itemUids[i] = UInt64.MaxValue;
    }
    public bool IsChanged(int idx, UInt64 realUid)
    {
        if (itemUids[idx] != realUid)
        {
            itemUids[idx] = realUid;
            return true;
        }
        return false;
    }
}
public class PlayerBag_Panel : MonoBehaviour
{
    public GameObject itemObj;
    RectTransform content;//内容
    GridLayoutGroup grid;
    ScrollRect scrollRect;
    Player _player = null;

    PlayerBagOptimizer playerBagOptimizer = new PlayerBagOptimizer();
    PlayerBag_Item[] playerBagItems = new PlayerBag_Item[Player.bagSpace];

    void Awake()
    {
        _player = StaticManager.sPlayer;
        //gameObject.SetActive(false);
    }
    void Start()
    {
        if (itemObj == null)
            return;

        scrollRect = GetComponent<ScrollRect>();
        content = scrollRect.content;
        grid = content.GetComponent<GridLayoutGroup>();

        //设置item高宽
        RectTransform rtfViewport = transform.FindChild("Viewport").GetComponent<RectTransform>();
        float itemWH = (rtfViewport.rect.width - grid.spacing.x * (float)(grid.constraintCount - 1) - grid.padding.left - grid.padding.right) / grid.constraintCount;
        grid.cellSize = new Vector2(itemWH, itemWH);

        //设置content高度
        int rowCount = Player.bagSpace / grid.constraintCount;
        float contentWH = itemWH * rowCount + grid.spacing.y * (float)(rowCount - 1) + grid.padding.top + grid.padding.bottom;
        content.sizeDelta = new Vector2(0, contentWH);

        for (int i = 0; i < Player.bagSpace; i++)
        {
            GameObject cobj = Instantiate<GameObject>(itemObj);
            playerBagItems[i] = cobj.AddComponent<PlayerBag_Item>();
            cobj.transform.SetParent(content, false);
            cobj.SetActive(true);
        }
        itemObj.SetActive(false);

        Refresh();
    }

    public void Refresh()
    {
        BaseItem[] bagItems = _player.bagItems;
        for (int i = 0; i < bagItems.Length; i++)
        {
            if (bagItems[i] != null)
            {
                if(playerBagOptimizer.IsChanged(i, bagItems[i].UId))
                {
                    playerBagItems[i].baseItem = bagItems[i];
                }
            }
        }
    }
}
