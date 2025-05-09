using UnityEngine;
using UnityEngine.EventSystems;

public class UIFocusInit : MonoBehaviour
{
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}
