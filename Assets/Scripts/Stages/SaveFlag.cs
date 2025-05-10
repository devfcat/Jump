using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 세이브 깃발마다 고유 아이디가 있으니 혼동하지 말 것
/// </summary>
public class SaveFlag : MonoBehaviour
{
    public int save_point;
    public GameObject Effect;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 닿으면 해당 깃발의 포인트를 저장한다
        if (other.name == "Player")
        {
            GameManager.Instance.Load();
            // 저장된 세이브포인트 값보다 새로 닿은 세이브 포인트가 클 경우에만 저장
            if (GameManager.Instance.myPoint < save_point)
            {
                SoundManager.Instance.PlaySFX(SFX.Save);
                GameManager.Instance.Save(save_point);
                Effect.SetActive(true);
            }
        }
    }
}
