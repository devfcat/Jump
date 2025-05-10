using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextFlag : MonoBehaviour
{
    public GameObject Effect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 닿으면 다음 씬으로
        if (other.name == "Player")
        {
            // 클리어 효과음 나게 추후 작성
            eState mState = GameManager.Instance.m_State;
            Effect.SetActive(true);
            if (mState == eState.OutSide)
            {
                GameManager.Instance.SetState(eState.InSide);
                GameManager.Instance.Save(5); // 다음 맵으로 갔음을 저장
                // 5번 플래그가 InSide 맵의 첫 포인트임
            }
            else if (mState == eState.InSide)
            {
                GameManager.Instance.SetState(eState.Ending);
                GameManager.Instance.Save(); // 다음 맵으로 갔음을 저장
            }
        }
    }
}
