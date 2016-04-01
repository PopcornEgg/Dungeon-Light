using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerBag_Item : MonoBehaviour
{
    public int Idx;
    PlayerBag_Panel playerBagPanel;
    BaseItem _baseItem;

    public BaseItem baseItem
    {
        get { return _baseItem; }
        set
        {
            _baseItem = value;
            bool isshow = (_baseItem != null);
            if (isshow)
            {
                txtCount.text = string.Format("x{0}", _baseItem.GetCount());
                imgIcon.sprite = SpriteManager.GetIconEquip(_baseItem.TabData.icon);
            }
            txtCount.gameObject.SetActive(isshow);
            imgIcon.gameObject.SetActive(isshow);
        }
    }
    Image imgIcon;
    Text txtCount;

    void Awake()
    {
        playerBagPanel = Second_Canvas.playerBagPanel;
        imgIcon = transform.FindChild("Icon").GetComponent<Image>();
        txtCount = transform.FindChild("Count").GetComponent<Text>();
    }

    void Start()
    {
        //windows点击鼠标右键
        Button btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(delegate () {
            Player.Self.UseBagItem(Idx);
        });
    }
}
