using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowIndicator : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject arrow;

    private bool initialized = false;

    void Start()
    {
        arrow = transform.GetChild(0).gameObject;

        // OnSelect는 Start 이후 호출될 수 있으므로, 한 프레임 뒤에 초기화
        StartCoroutine(EnableAfterFrame());
    }

    System.Collections.IEnumerator EnableAfterFrame()
    {
        yield return null; // 한 프레임 기다림
        initialized = true;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!initialized) return;
        arrow.SetActive(true);
        SoundManager.Instance.PlaySFX(SFX.UI);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!initialized) return;
        arrow.SetActive(false);
    }
}

