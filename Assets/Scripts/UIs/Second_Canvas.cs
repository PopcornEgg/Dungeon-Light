using UnityEngine;
using System.Collections;

public class Second_Canvas : MonoBehaviour {

    SetUp_Panel _setUpPanel;
    public SetUp_Panel setUpPanel { get { return _setUpPanel; } set { _setUpPanel = value; } }

    PlayerBag_Panel _playerBagPanel;
    public PlayerBag_Panel playerBagPanel { get { return _playerBagPanel; } set { _playerBagPanel = value; } }
    
    void Awake()
    {
        StaticManager.sSecond_Canvas = this;
    }
    void Start ()
    {
        setUpPanel = transform.FindChild("SetUp_Panel").GetComponent<SetUp_Panel>();
        playerBagPanel = transform.FindChild("PlayerBag_Panel").GetComponent<PlayerBag_Panel>();
    }
}
