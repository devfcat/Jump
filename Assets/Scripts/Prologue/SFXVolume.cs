using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXVolume : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = this.transform.GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSource.volume = SoundManager.Instance.SfxVolume;
    }
}
