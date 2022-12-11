using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    private AudioSource m_AudioSource;

    [SerializeField] private AudioClip[] m_AudioArray;

    protected virtual void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    protected virtual void PlaySound(AudioClip[] audioArray)
    {
        if (audioArray.Length <= 0)
            return;

        m_AudioSource.clip = audioArray[Random.Range(0, audioArray.Length - 1)];
        m_AudioSource.Play();
    }

    public void PlaySound()
    {
        PlaySound(m_AudioArray);
    }
}
