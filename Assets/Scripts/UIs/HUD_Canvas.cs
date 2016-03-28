using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD_Canvas : MonoBehaviour {

    Canvas canvas;
    public Slider HP_Slider;

    void Awake()
    {
        StaticManager.sHUD_Canvas = this;
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void SetHP_Slider(float per)
    {
        HP_Slider.value = per;
    }

    public void SetUp( )
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
}
