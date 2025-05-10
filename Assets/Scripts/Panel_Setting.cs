using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_Setting : MonoBehaviour
{
    [Header("음량 조절 슬라이더")]
    public Slider BgmSlider;
    public Slider SESlider;

    [Header("설정 패널의 버튼들")]
    public GameObject btn_reGame;
    public GameObject btn_saveMain;

    private eState mState;

    void OnEnable()
    {
        Init();
        // SoundManager.Instance.PlaySFX(SFX.UI);
    }

    void OnDisable()
    {
        // SoundManager.Instance.PlaySFX(SFX.UI);
    }

    void Init()
    {
        mState = GameManager.Instance.m_State;
        if (mState == eState.OutSide || mState == eState.InSide)
        {
            btn_reGame.SetActive(true);
        }
        else
        {
            btn_reGame.SetActive(false);
        }

        if (mState == eState.Main)
        {
            btn_saveMain.SetActive(false);
        }
        else
        {
            btn_saveMain.SetActive(true);
        }

        Set_UI_Slider();
    }

    /// <summary>
    /// 볼륨 조절 슬라이더 UI 제어 (실제 소리의 볼륨은 조절하지 않음)
    /// </summary>
    public void Set_UI_Slider()
    {
        BgmSlider.value = SoundManager.Instance.BgmVolume;
        SESlider.value = SoundManager.Instance.SfxVolume;
    }

    public void OnClick_Close()
    {
        SoundManager.Instance.PlaySFX(SFX.UI);
        GameManager.Instance.Control_Setting();
    }

    public void OnClick_Main()
    {
        GameManager.Instance.SetState(eState.Main);
    }

    public void OnClick_ReLoad()
    {
        mState = GameManager.Instance.m_State;
        GameManager.Instance.SetState(mState);
    }
}
