using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerBag_Panel : MonoBehaviour
{
    public GameObject itemObj;
    RectTransform content;//内容
    GridLayoutGroup grid;
    ScrollRect scrollRect;
    Player _player = null;

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
            cobj.SetActive(true);
            cobj.transform.SetParent(content, false);
        }
        itemObj.SetActive(false);
    }
}
