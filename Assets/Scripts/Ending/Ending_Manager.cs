using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending_Manager : MonoBehaviour
{
    public List<GameObject> Panels;
    private float time;

    private bool isStart = false;

    private static Ending_Manager _instance;
    public static Ending_Manager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(Ending_Manager)) as Ending_Manager;

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
        SoundManager.Instance.PlayBGM(BGM.Ending);
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
            if (page == i) Panels[i].SetActive(true);
            else Panels[i].SetActive(false);
        } 
    }
}
