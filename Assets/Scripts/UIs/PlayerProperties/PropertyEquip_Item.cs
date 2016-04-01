using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PropertyEquip_Item : MonoBehaviour
{
    public int Idx;
    PlayerProperty_Panel playerPropertyPanel;
    BaseItem _baseItem;

    public BaseItem baseItem
    {
        get { return _baseItem; }
        set
        {
            _baseItem = value;
            bool isshow = _baseItem != null;
            if (isshow)
            {
                imgIcon.sprite = SpriteManager.GetIconEquip(_baseItem.TabData.icon);
            }
            imgIcon.gameObject.SetActive(isshow);
        }
    }
    Image imgIcon;

    void Awake()
    {
        playerPropertyPanel = Second_Canvas.playerPropertyPanel;
        imgIcon = transform.FindChild("Icon").GetComponent<Image>();
    }
    void Start()
    {
        //windows点击鼠标右键
        Button btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(delegate ()
        {
            Player.Self.UseBodyItem(Idx);
        });
    }
}
