using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PropertyEquip_Item : MonoBehaviour
{
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
        playerPropertyPanel = StaticManager.sSecond_Canvas.playerPropertyPanel;
        imgIcon = transform.FindChild("Icon").GetComponent<Image>();
    }
}
