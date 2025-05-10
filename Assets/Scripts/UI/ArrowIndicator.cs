using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowIndicator : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject arrow;

    private bool hasInitialized = false; // 사용자가 조작했는지 여부

    void Start()
    {
        arrow = this.transform.GetChild(0).gameObject;
    }

    void OnEnable()
    {
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
        {
            arrow.SetActive(true);
        }

        // 1프레임 뒤부터 사운드 재생 허용
        StartCoroutine(EnableSoundNextFrame());
    }

    void OnDisable()
    {
        arrow.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        arrow.SetActive(true);

        if (!hasInitialized)
            return;

        SoundManager.Instance.PlaySFX(SFX.UI);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        arrow.SetActive(false);
    }

    System.Collections.IEnumerator EnableSoundNextFrame()
    {
        yield return null;
        hasInitialized = true;
    }
}
