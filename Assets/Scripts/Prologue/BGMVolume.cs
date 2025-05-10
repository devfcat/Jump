using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMVolume : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = this.transform.GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSource.volume = SoundManager.Instance.BgmVolume;
    }
}
