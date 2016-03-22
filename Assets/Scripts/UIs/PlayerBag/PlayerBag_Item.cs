using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerBag_Item : MonoBehaviour {

    BaseItem _baseItem;

    public BaseItem baseItem { get { return _baseItem; }
        set
        {
            _baseItem = value;
            txtCount.text = "x80";
        }
    }
    Image imgIcon;
    Text txtCount;
   
    void Awake()
    {
        imgIcon = transform.FindChild("Icon").GetComponent<Image>();
        txtCount = transform.FindChild("Count").GetComponent<Text>();
    }
}
