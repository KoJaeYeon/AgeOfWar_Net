using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{    
    AudioSource m_audioSource;
    private void Awake()
    {
        if (Instance != this) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        m_audioSource = GetComponent<AudioSource>();
    }
}
