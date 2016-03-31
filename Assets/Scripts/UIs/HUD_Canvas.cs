using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD_Canvas : MonoBehaviour
{

    Canvas canvas;
    Slider HP_Slider;
    Text currHP;
    Slider MP_Slider;
    Text currMP;
    Slider Exp_Slider;
    Text currEXP;

    void Awake()
    {
        StaticManager.sHUD_Canvas = this;
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();

        Transform MainInfo = transform.FindChild("MainInfo");
        HP_Slider = MainInfo.FindChild("HP_Slider").GetComponent<Slider>();
        currHP = HP_Slider.transform.FindChild("Text").GetComponent<Text>();
        MP_Slider = MainInfo.FindChild("MP_Slider").GetComponent<Slider>();
        currMP = MP_Slider.transform.FindChild("Text").GetComponent<Text>();
        Exp_Slider = MainInfo.FindChild("Exp_Slider").GetComponent<Slider>();
        currEXP = Exp_Slider.transform.FindChild("Text").GetComponent<Text>();
    }
    void FixedUpdate()
    {
        Player player = StaticManager.sPlayer;
        if (player == null)
            return;

        currHP.text = string.Format("{0}/{1}", player.HP, (int)player.MAXHP);
        HP_Slider.value = player.HP / player.MAXHP;
        currMP.text = string.Format("{0}/{1}", player.MP, (int)player.MAXMP);
        MP_Slider.value = (float)player.MP / player.MAXMP;
        currEXP.text = string.Format("{0}/{1}", player.EXP, player.CurrPlayerLvTab.exp);
        Exp_Slider.value = (float)player.EXP / player.MAXHP;
    }

    public void SetUp()
    {
        if (StaticManager.sPlayer.AIState == CharacterAnimState.Die)
            return;

        SetUp_Panel _setUpPanel = StaticManager.sSecond_Canvas.setUpPanel;
        if (_setUpPanel != null)
        {
            if (_setUpPanel.gameObject.activeSelf)
                _setUpPanel.gameObject.SetActive(false);
            else
                _setUpPanel.gameObject.SetActive(true);

            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
    }
    public void DieSetUp()
    {
        SetUp_Panel _setUpPanel = StaticManager.sSecond_Canvas.setUpPanel;
        if (_setUpPanel != null)
        {
            if (_setUpPanel.gameObject.activeSelf)
                _setUpPanel.gameObject.SetActive(false);
            else
                _setUpPanel.gameObject.SetActive(true);

            Animator _canvasAni = _setUpPanel.GetComponent<Animator>();
            if (_canvasAni != null)
            {
                _canvasAni.SetTrigger("Die");
            }
        }
    }

    public void OnClick_PlayerBag()
    {
        PlayerBag_Panel _playerBagPanel = StaticManager.sSecond_Canvas.playerBagPanel;
        if (_playerBagPanel != null)
        {
            _playerBagPanel.gameObject.SetActive(_playerBagPanel.gameObject.activeSelf ? false : true);
        }
    }
    public void OnClick_PlayerProperty()
    {
        PlayerProperty_Panel _propertyPanel = StaticManager.sSecond_Canvas.playerPropertyPanel;
        if (_propertyPanel != null)
        {
            _propertyPanel.gameObject.SetActive(_propertyPanel.gameObject.activeSelf ? false : true);
        }
    }
    public void OnClick_PlayerRandomJump()
    {
        SceneTab _sceneTab = StaticManager.sSceneManager.currScene._sceneTab;
        int idx = UnityEngine.Random.Range(0, _sceneTab.levelTab.Count);
        LevelTab.Data data = _sceneTab.levelTab.lsTabs[idx];
        StaticManager.sPlayer.transform.position = new Vector3(data.x, data.y, data.z);
    }
}