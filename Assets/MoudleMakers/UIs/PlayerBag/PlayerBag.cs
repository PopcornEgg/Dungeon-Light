using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerBag : MonoBehaviour
{
    public int bagSpace = 100;//背包容量
    public GameObject itemObj;

    RectTransform content;//内容
    GridLayoutGroup grid;
    ScrollRect scrollRect;

    void Start()
    {
        TabManager.Load();

        if (itemObj == null)
            return;

        Transform tfMain = transform.FindChild("Main");

        scrollRect = tfMain.GetComponent<ScrollRect>();
        content = scrollRect.content;
        grid = content.GetComponent<GridLayoutGroup>();

        //设置item高宽
        RectTransform rtfViewport = tfMain.FindChild("Viewport").GetComponent<RectTransform>();
        float itemWH = (rtfViewport.rect.width - grid.spacing.x * (float)(grid.constraintCount - 1) - grid.padding.left - grid.padding.right) / grid.constraintCount;
        grid.cellSize = new Vector2(itemWH, itemWH);

        //设置content高度
        int rowCount = bagSpace / grid.constraintCount;
        float contentWH = itemWH * rowCount + grid.spacing.y * (float)(rowCount - 1) + grid.padding.top + grid.padding.bottom;
        content.sizeDelta = new Vector2(0, contentWH);

        for (int i = 0; i < bagSpace; i++)
        {
            GameObject cobj = Instantiate<GameObject>(itemObj);
            cobj.SetActive(true);
            cobj.transform.SetParent(content, false);
        }
        itemObj.SetActive(false);
    }
}
