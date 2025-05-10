using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Bgm 오디오 클립 인덱스
/// </summary>
public enum BGM
{
    Main = 0,
    Prologue = 1,
    OutSide = 2,
    InSide = 3,
    Ending = 4
    
}

/// <summary>
/// Bgm 재생 상태
/// </summary>
public enum BgmStatus
{
    Play, // 재생 중
    Stop, // 재생 안함
    Pause // 일시 정지
}

/// <summary>
/// 효과음 오디오 클립 인덱스
/// </summary>
public enum SFX
{
    UI = 0,
    SceneChange = 1,
    Save = 2,
}

public class SoundManager : MonoBehaviour
{
    [Header("AudioClips")] // 인스펙터 창에서 직접 파일 추가
    [SerializeField, Tooltip("Bgm 클립 리스트")] private List<AudioClip> bgmList; 
    [SerializeField, Tooltip("Se 클립 리스트")] private List<AudioClip> sfxList; 

    private AudioSource bgmPlayer; // Bgm AudioSource, SoundManager 첫 번째 자식이어야 함 
    private AudioSource sfxPlayer; // Sfx AudioSource, SoundManager 두 번째 자식이어야 함 

    [Header("Volume")] // 볼륨 상태 디버깅용
    [ReadOnly, SerializeField, Range(0, 1), Tooltip("Bgm 볼륨")] private float Volume1 = 1f;
    [ReadOnly, SerializeField, Range(0, 1), Tooltip("Sfx 볼륨")] private float Volume2 = 1f;

    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    /// <summary>
    /// 저장된 볼륨 불러오기 메소드
    /// </summary>
    private void GetVolumePref()
    {
        if (PlayerPrefs.GetFloat("Volume") == 0)
        {
            // Debug.Log("No saved volume");
            PlayerPrefs.SetFloat("BgmVolume", 1f);
            PlayerPrefs.SetFloat("SfxVolume", 1f);
            PlayerPrefs.SetFloat("Volume", 1f);
            BgmVolume = 1f;
            SfxVolume = 1f;
        }
        else
        {
            BgmVolume = PlayerPrefs.GetFloat("BgmVolume");
            SfxVolume = PlayerPrefs.GetFloat("SfxVolume");
            bgmPlayer.volume = BgmVolume;
            sfxPlayer.volume = SfxVolume;
            Debug.Log(string.Format("BgmVolume : {0}, SfxVolume : {1}", BgmVolume, SfxVolume));
        }
    }

    /// <summary>
    /// 볼륨 설정 저장 메소드
    /// </summary>
    public void SetVolumePref()
    {
        // Debug.Log("Save volume");
        PlayerPrefs.SetFloat("BgmVolume", BgmVolume);
        PlayerPrefs.SetFloat("SfxVolume", SfxVolume);
        PlayerPrefs.SetFloat("Volume", 1f);
    }
    
    // Bgm AudioSource 볼륨 크기
    private float bgmVolume;
    public float BgmVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        {
            bgmVolume = Mathf.Clamp01(value);
            bgmPlayer.volume = bgmVolume;
            BgmStatusCheck();
            Volume1 = bgmVolume;

        }
    }
    // Sfx AudioSource 볼륨 크기
    private float sfxVolume;
    public float SfxVolume
    {
        get
        {
            return sfxVolume;
        }
        set
        {
            sfxVolume = Mathf.Clamp01(value);
            sfxPlayer.volume = sfxVolume;
            Volume2 = sfxVolume;
        }
    }

    private void Awake()
    {
        InstanceCheck();
        Init();
        GetVolumePref();
    }

    /// <summary>
    /// 인스턴스 관리 메소드
    /// </summary>
    private void InstanceCheck()
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

    // BGM 소리 조절
    public void OnValueChange_BgmVolume(Slider s)
    {
        /* 해당 씬에서 사용
        bgmS.value = SoundManager.Instance.BgmVolume;
        sfxS.value = SoundManager.Instance.SfxVolume;
        */
        BgmVolume = s.value;
        SetVolumePref();
    }

    // SE 소리 조절
    public void OnValueChange_SfxVolume(Slider s)
    {
        SfxVolume = s.value;
        SetVolumePref();
    }

    private void Init()
    {
        bgmPlayer = GetComponentsInChildren<AudioSource>()[0];
        sfxPlayer = GetComponentsInChildren<AudioSource>()[1];
    }

    /// <summary>
    /// Bgm AudioSource 상태 확인
    /// </summary>
    private void BgmStatusCheck()
    {
        if (bgmPlayer.volume == 0 && bgmPlayer.isPlaying) // 볼륨 0일 때 재생 정지
        {
            // Debug.Log("Muted");
            // BgmControl(BgmStatus.Stop);
        }
        else if (bgmPlayer.volume > 0 && !bgmPlayer.isPlaying) // 볼륨이 0이 아니게 됐을 때 재생
        {
            // Debug.Log("Unmuted");
            BgmControl(BgmStatus.Play);
        }
    }

    /// <summary>
    /// Bgm Clip 선택 후 재생 메소드
    /// </summary>
    /// <param name="bgm">재생할 Bgm 파일 인덱스</param>
    public void PlayBGM(BGM bgm)
    {
        bgmPlayer.clip = bgmList[(int)bgm];
        if (bgmPlayer.volume > 0) BgmControl(BgmStatus.Play); // 음소거 시 클립 설정 후 재생하지 않음
    }

    /// <summary>
    /// Sfx Clip 선택 후 재생 메소드
    /// </summary>
    /// <param name="sfx">재생할 Sfx 파일 인덱스</param>
    public void PlaySFX(SFX sfx)
    {
        if (sfxPlayer.volume > 0) // 음소거 시 재생하지 않음
        {
            if (sfxPlayer.isPlaying) sfxPlayer.Stop();

            sfxPlayer.clip = sfxList[(int)sfx];
            sfxPlayer.Play(); // 바로 교체 재생
        }
    }

    /// <summary>
    /// Bgm 상태 변경
    /// </summary>
    /// <param name="status">Play : 재생, Stop : 음소거, Pause : 일시정지</param>
    public void BgmControl(BgmStatus status)
    {
        switch (status)
        {
            case BgmStatus.Play:
                if (bgmPlayer.isPlaying) return;
                bgmPlayer.Play();
                break;
            case BgmStatus.Stop:
                bgmPlayer.Stop();
                break;
            case BgmStatus.Pause:
                bgmPlayer.Pause();
                break;
        }
    }
}
