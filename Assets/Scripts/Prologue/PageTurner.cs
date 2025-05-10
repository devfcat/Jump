using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageTurner : MonoBehaviour
{
    public int next_page;

    public void Turn()
    {
        PrologueManager.Instance.Play_Animation(next_page);
    }
}
