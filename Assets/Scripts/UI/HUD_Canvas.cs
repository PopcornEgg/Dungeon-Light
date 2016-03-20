using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD_Canvas : MonoBehaviour {

    Canvas canvas;
    public Slider HP_Slider;

    public Transform setUpCanvasTransform;

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
        if (StaticManager.sPlayer.isDead)
            return;
        
        Canvas _canvas = setUpCanvasTransform.GetComponent<Canvas>();
        if(_canvas != null)
        {
            _canvas.enabled = !_canvas.enabled;
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
    }
    public void DieSetUp()
    {
        Canvas _canvas = setUpCanvasTransform.GetComponent<Canvas>();
        if (_canvas != null)
        {
            _canvas.enabled = !_canvas.enabled;

            Animator _canvasAni = _canvas.GetComponent<Animator>();
            if (_canvasAni != null)
            {
                _canvasAni.SetTrigger("Die");
            }
        }
    }
}
