﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCShop_Item : MonoBehaviour
{
    public int Idx;
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
            if(imgIcon.gameObject.activeSelf)
                Player.Self.BuyShopItem(Idx);
        });
    }

    public void Set(bool isshow, string _icon = "", int _lcount = -1)
    {
        if (isshow)
        {
            if (_lcount >= 0)
            {
                txtCount.text = string.Format("x{0}", _lcount);
                txtCount.gameObject.SetActive(true);
            }
            else
                txtCount.gameObject.SetActive(false);
            imgIcon.sprite = SpriteManager.GetIcon(_icon);
            imgIcon.gameObject.SetActive(true);
        }
        else
        {
            txtCount.gameObject.SetActive(false);
            imgIcon.gameObject.SetActive(false);
        }
    }
    public void RefreshCount(int _lcount = -1)
    {
        if (_lcount >= 0)
        {
            txtCount.text = string.Format("x{0}", _lcount);
            txtCount.gameObject.SetActive(true);
        }
        else
            txtCount.gameObject.SetActive(false);
    }
}
