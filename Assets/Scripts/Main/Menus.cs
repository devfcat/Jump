using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Menus : MonoBehaviour
{
    private string loaded_map;

    public GameObject btn_loadStart;
    public GameObject btn_Ending;

    void Start()
    {
        GameManager.Instance.Panel_MainMenu = this.gameObject;
    }

    void OnEnable()
    {
        GameManager.Instance.Load();
        Set_UI();
    }

    void Set_UI()
    {
        loaded_map = GameManager.Instance.myMap;
        if (loaded_map == "OutSide" || loaded_map == "InSide" || loaded_map == "Ending")
        {
            btn_loadStart.SetActive(true);
        }
        else
        {
            btn_loadStart.SetActive(false);
        }

        if (GameManager.Instance.isGetEnd == 1)
        {
            btn_Ending.SetActive(true);
        }
        else
        {
            btn_Ending.SetActive(false);
        }
    }

    public void OnClick_Setting()
    {
        GameManager.Instance.Control_Setting();
        SoundManager.Instance.PlaySFX(SFX.UI);
    }

    public void OnClick_Exit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 프롤로그로 새로 시작
    /// </summary>
    public void OnClick_NewStart()
    {
        GameManager.Instance.Reset();
        GameManager.Instance.SetState(eState.Prologue);
    }

    /// <summary>
    /// 데이터를 로드하여 해당 맵으로 이어하기 처리
    /// </summary>
    public void OnClick_LoadStart()
    {
        GameManager.Instance.Load();
        GameManager.Instance.SetState((eState)Enum.Parse(typeof(eState), loaded_map));
    }

    public void OnClick_Ending()
    {
        GameManager.Instance.SetState(eState.Ending);
    }
}
