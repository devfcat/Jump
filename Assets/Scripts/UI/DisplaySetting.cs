using UnityEngine;
using TMPro;

/// <summary>
/// 창모드와 전체화면 모드 전환 기능
/// </summary>

public enum eResolution
{
    fullscreen = 0,
    big = 1,
    midium = 2,
    small = 3,
    low = 4,
}

public class DisplaySetting : MonoBehaviour
{
    private static DisplaySetting _instance;
    public static DisplaySetting Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(DisplaySetting)) as DisplaySetting;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    [SerializeField] private eResolution m_resolution;
    public TextMeshProUGUI text_ui;
    public bool SettingMod; // 설정 창 말고 기본 세팅 시

    public void Start()
    {
        Init();
    }

    public void Control_data(bool isSave = false)
    {
        if (isSave)
        {
            PlayerPrefs.SetInt("eResolution", (int)m_resolution);
        }
        else
        {
            int saved = PlayerPrefs.GetInt("eResolution");
            if (saved == 0)
            {
                m_resolution = eResolution.fullscreen;
            }
            else
            {
                m_resolution = (eResolution)saved;
            }
        }
    }

    void Init()
    {
        Control_data();
        Set_UI();
        Set_Resolution();
    }

    // 현재 창 모드에 따라 UI 세팅
    public void Set_UI()
    {
        if (SettingMod)
        {
            return;
        }

        if (m_resolution == eResolution.fullscreen)
        {
            text_ui.text = "Full";
        }
        else
        {
            switch (m_resolution)
            {
                case eResolution.big:
                    text_ui.text = "1920 * 1080 px";
                    break;
                case eResolution.midium:
                    text_ui.text = "1600 * 900 px";
                    break;
                case eResolution.small:
                    text_ui.text = "1280 * 720 px";
                    break;
                case eResolution.low:
                    text_ui.text = "800 * 450 px";
                    break;
                default:
                    text_ui.text = "Error";
                    break;
            }
        }
    }

    void Set_Resolution()
    {
        switch (m_resolution)
        {
            case eResolution.big:
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
                break;
            case eResolution.midium:
                Screen.SetResolution(1600, 900, FullScreenMode.Windowed);
                break;
            case eResolution.small:
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                break;
            case eResolution.low:
                Screen.SetResolution(800, 450, FullScreenMode.Windowed);
                break;
            default:
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                break;
        }
    }

    public void OnClick()
    {
        // SoundManager.Instance.PlaySFX(SFX.UI);
        
        int index = (int)m_resolution;
        if (index < 4)
        {
            index++;
        }
        else // index == 4이면
        {
            index = 0;
        }

        m_resolution = (eResolution)index;
        Control_data(true);
        Set_Resolution();
        Set_UI();
    }
}
