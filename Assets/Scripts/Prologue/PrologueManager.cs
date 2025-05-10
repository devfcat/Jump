using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueManager : MonoBehaviour
{
    public List<GameObject> Panels;
    private float time;

    private bool isStart = false;

    private static PrologueManager _instance;
    public static PrologueManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(PrologueManager)) as PrologueManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    void Start()
    {
        // 저장
        GameManager.Instance.Save();
        SoundManager.Instance.PlayBGM(BGM.Prologue);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > 3f && !isStart)
        {
            Play_Animation(1);
            isStart = true;
        }
    }

    public void Play_Animation(int page = 1)
    {
        for(int i = 0; i < Panels.Count; i++)
        {
            Panels[i].SetActive(false);
        } 

        switch (page)
        {
            case 1:
                Panels[0].SetActive(true);
                break;
            case 2:
                Panels[1].SetActive(true);
                Panels[2].SetActive(true);
                break;
            case 3:
                Panels[3].SetActive(true);
                break;
            case 4:
                // 다음 진행상황으로 넘기고 저장
                GameManager.Instance.SetState(eState.OutSide);
                GameManager.Instance.Save();
                break;
            default:
                break;
        }
    }
}
