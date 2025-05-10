using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

// 씬 이름과 상태 이름이 동일하도록 적을 것
public enum eState
{
    Main = 0,
    Prologue = 1,
    OutSide = 2,
    InSide = 3,
    Ending = 4,
}

public enum gameState
{
    Default = 0,
    Setting,
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    [Header("상태 및 정보")]
    public eState m_State; // 현재 씬 상태
    public gameState g_State; // 현재 일시정지인가 게임 중인가
    public bool isWorking; // 씬 변경 중인지 또는 로딩중인지

    /// <summary>
    /// 저장된 내 맵과 위치
    /// </summary>
    public string myMap;
    public int myPoint;

    public int isGetEnd; // 엔딩을 해금한 적이 있는가

    public bool isPopupOn; // 팝업이 열려있는가
    public GameObject m_Popup;

    [Header("설정 패널들")]
    public GameObject Panel_MainMenu;
    public GameObject Panel_Setting;
    // public GameObject Loading;

    [Header("페이드 패널")]
    public GameObject Panel_FadeIn;
    public GameObject Panel_FadeOut;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Init();
    }

    public void Init()
    {
        Application.targetFrameRate = 60;

        isWorking = false;
        isPopupOn = false;
        SetState(eState.Main);
        SetGameState(gameState.Default);

        Load();
    }

    /// <summary>
    /// 씬 처음 시작일 경우 isOn = true
    /// 다음 씬으로 넘어가려고 하는 경우 isOn = false
    /// </summary>
    /// <param name="isOn"></param>
    public IEnumerator Fade(bool isOn = false)
    {
        isWorking = true;

        // 씬 처음 시작일 경우
        if (isOn)
        {
            Panel_FadeIn.SetActive(false);
            Panel_FadeOut.SetActive(true);
        }
        else // 다음 씬으로 넘어가려고 하는 경우
        {
            Panel_FadeIn.SetActive(true);
            Panel_FadeOut.SetActive(false);
        }

        yield return new WaitForSeconds(3f);

        isWorking = false;
    }

    /// <summary>
    /// 잠깐 어두워졌다가 다시 밝아지는 화면 효과
    /// StartCoroutine(GameManager.Instance.Curtain()); 로 사용.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Curtain()
    {
        isWorking = true;

        Panel_FadeIn.SetActive(true);
        Panel_FadeOut.SetActive(false);

        yield return new WaitForSeconds(2f);

        Panel_FadeIn.SetActive(false);
        Panel_FadeOut.SetActive(true);

        isWorking = false;
    }

    /// <summary>
    /// 세이브 포인트를 삭제한다
    /// </summary>
    public void Reset()
    {
        PlayerPrefs.DeleteKey("myPoint");
        myPoint = PlayerPrefs.GetInt("myPoint");
    }

    public void Load()
    {
        string saved_map = PlayerPrefs.GetString("Map");
        Debug.Log("로드된 스테이지: " + saved_map);
        myMap = saved_map;

        myPoint = PlayerPrefs.GetInt("myPoint");
        Debug.Log("로드된 플레이어 위치: " + myPoint);

        isGetEnd = PlayerPrefs.GetInt("isGetEnd");
    }

    public void Save(int save_point=0)
    {
        string saved_map = m_State.ToString();
        PlayerPrefs.SetString("Map", saved_map);
        Debug.Log("저장된 스테이지: " + saved_map);

        if (m_State == eState.Ending) // 만약 엔딩이라면면
        {
            PlayerPrefs.SetInt("isGetEnd", 1);
            Debug.Log("엔딩 해금됨");
        }

        PlayerPrefs.SetInt("myPoint", save_point);
        Debug.Log("저장된 플레이어 위치: " + save_point);

        Load();
    }

    public void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("저장된 정보 삭제");
        }
#endif
        /*
        if (isWorking)
        {
            Loading.SetActive(true);
        }
        else
        {
            if (Loading.activeSelf)
            {
                Loading.SetActive(false);
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPopupOn)
            {
                Control_Popup(false);
            }
            else
            {
                SoundManager.Instance.PlaySFX(SFX.UI);
                Control_Setting();
            }
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (m_State == eState.OutSide || m_State == eState.InSide)
            {
                SoundManager.Instance.PlaySFX(SFX.UI);
                GameManager.Instance.SetState(m_State);
            }
        }
    }

    // 팝업을 관리하는 메서드
    public void Control_Popup(bool on, GameObject b = null)
    {
        if(m_Popup != null && m_Popup.activeSelf) // 이전에 켜진 팝업이 있는데 다른 팝업을 켤 경우 끔
        {
            m_Popup.SetActive(false);
        }

        isPopupOn = on;
        if (on)
        {
            // 팝업 소리 재생
            m_Popup = b;
            m_Popup.SetActive(true);
        }
        else if (m_Popup != null)
        {
            if(m_Popup.activeSelf)
            {
                // 팝업 소리 재생
            }
            m_Popup.SetActive(false);
        }
    }

    // 설정창을 끄거나 키는 기능
    public void Control_Setting()
    {
        if (isPopupOn)
        {
            return;
        }

        if (g_State == gameState.Default)
        {
            SetGameState(gameState.Setting);
        }
        else
        {
            SetGameState(gameState.Default);
        }
    }

    // 설정창 또는 일반 스테이지 등을 구분하는 상태 머신
    public void SetGameState(gameState state)
    {
        g_State = state;

        if (g_State == gameState.Setting)
        {
            Panel_Setting.SetActive(true);
        }
        else
        {
            if (Panel_Setting.activeSelf)
            {
                Panel_Setting.SetActive(false);
            }
        }

        try
        {
            if (m_State == eState.Main) // 메인 씬에서 설정 메뉴가 켜지면 메인 메뉴는 꺼지게 작업
            {
                Panel_MainMenu.SetActive(!Panel_Setting.activeSelf);
            }
        }
        catch {}
    }
    
    // 상태 머신 함수 (스테이지로 넘길 때는 stage 인자에 int값을 넣을 것)
    // Stage 값은 1~stage_Max까지 있음 (0일때는 플레이한 적이 없는 것)
    public void SetState(eState state)
    {
        if (state == eState.Main && m_State == eState.Main)
        {
            return;
        }

        // 상태 관리 메서드가 작동중인가
        // 프롤로그 스킵은 예외
        if (isWorking)
        {
            Debug.Log("씬 변경중");
            return;
        }
        else
        {
            isWorking = true;
        }

        m_State = state;
        
        switch(m_State)
        {
            case eState.Main:
                StartCoroutine(Change_Scene("Main"));
                break;
            case eState.Prologue:
                StartCoroutine(Change_Scene("Prologue"));
                break;
            case eState.Ending:
                StartCoroutine(Change_Scene("Ending"));
                break;
            case eState.OutSide:
                StartCoroutine(Change_Scene("OutSide"));
                break;
            case eState.InSide:
                StartCoroutine(Change_Scene("InSide"));
                break;
            default:
                SetState(eState.Main);
                break;  
        }
    }

    /// <summary>
    /// 일정 시간 뒤 씬 변경과 일부 변수를 초기화함
    /// </summary>
    /// <param name="scenename">변경할 씬의 이름</param>
    /// <returns></returns>
    IEnumerator Change_Scene(string scenename)
    {
        // Control_Setting();
        Panel_Setting.SetActive(false);
        g_State = gameState.Default;

        Control_Popup(false);
        SetGameState(gameState.Default);

        SoundManager.Instance.BgmControl(BgmStatus.Pause);
        SoundManager.Instance.PlaySFX(SFX.SceneChange);

        yield return StartCoroutine(Fade()); // 창 어둡게

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scenename);

        while (!asyncOperation.isDone)
        {
            isWorking = true;
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;
        StartCoroutine(Fade(true));
        isWorking = false;

        SoundManager.Instance.BgmControl(BgmStatus.Play);
    }
}