using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTN_Setting : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.Control_Setting();
    }
}
