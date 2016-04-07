using UnityEngine;
using System.Collections;

public class Second_Canvas : MonoBehaviour {

    public static SetUp_Panel setUpPanel;
    public static PlayerBag_Panel playerBagPanel;
    public static PlayerProperty_Panel playerPropertyPanel;
    public static NPCShop_Panel npcShopPanel;

    void Awake()
    {
    }
    void Start ()
    {
        setUpPanel = transform.FindChild("SetUp_Panel").GetComponent<SetUp_Panel>();
        playerBagPanel = transform.FindChild("PlayerBag_Panel").GetComponent<PlayerBag_Panel>();
        playerPropertyPanel = transform.FindChild("Property_Panel").GetComponent<PlayerProperty_Panel>();
        npcShopPanel = transform.FindChild("NPCShop_Panel").GetComponent<NPCShop_Panel>();
    }

    public static void RefreshPlayerBag()
    {
        if (playerBagPanel != null)
            playerBagPanel.Refresh();
    }
    public static void RefreshPlayerProperty()
    {
        if (playerPropertyPanel != null)
        {
            playerPropertyPanel.OnEnable();
        }
    }
}
