using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_OutSide : MonoBehaviour
{
    public List<Transform> flags;
    public int loaded_point;

    public GameObject player;
    public Transform player_transform;

    void Start()
    {
        SoundManager.Instance.PlayBGM(BGM.OutSide);
        SetGameInit();
    }

    /// <summary>
    /// 플레이어 위치 로드 및 세팅
    /// </summary>
    void SetGameInit() 
    {
        player.SetActive(false);

        GameManager.Instance.Load();
        loaded_point = GameManager.Instance.myPoint;
        // 이 맵에 처음 들어오면 저장된 포인트를 가져옴
        // 저장된 포인트가 없다면 0임

        // 해당 포인트의 깃발 위치보다 조금 더 위에서 태어남
        Vector3 loaded_pos = new Vector3(flags[loaded_point].position.x, flags[loaded_point].position.y + 0.1f ,0f);
        player_transform.position = loaded_pos;

        player.SetActive(true);
    }
}
