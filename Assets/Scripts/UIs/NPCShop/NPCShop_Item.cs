using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCShop_Item : MonoBehaviour
{
    public int Idx;
    ShopItemData shopItemData;
    Image imgIcon;
    Text txtCount;

    void Awake()
    {
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

    public void Set(ShopItemData _sidata, bool isbuycount)
    {
        shopItemData = _sidata;
        bool isshow = (_sidata != null);
        if (isshow)
        {
            txtCount.gameObject.SetActive(isbuycount);
            if(isbuycount)
                txtCount.text = string.Format("x{0}", shopItemData.leftCount);
            imgIcon.sprite = SpriteManager.GetIcon(shopItemData.icon);
        }
        txtCount.gameObject.SetActive(isshow);
        imgIcon.gameObject.SetActive(isshow);
    }
    public void RefreshCount()
    {
        txtCount.text = string.Format("x{0}", shopItemData.leftCount);
    }
}
