using UnityEngine;
using UnityEngine.EventSystems;

public class UIFocusInit : MonoBehaviour
{
    void OnEnable()
    {
        try
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
        catch {}
    }

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}
