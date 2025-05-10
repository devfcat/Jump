using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurner : MonoBehaviour
{
    public int next_page;

    public void Turn()
    {
        Ending_Manager.Instance.Play_Animation(next_page);
    }
}
