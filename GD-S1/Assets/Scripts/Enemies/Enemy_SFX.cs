using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SFX : SFXHandler
{
    [SerializeField] AudioClip[] m_MovementSFX;
    [SerializeField] AudioClip[] m_AttackedSFX;

    public void PlayHurtSFX()
    {
        PlaySound(m_AttackedSFX);
    }

    public void PlayMoveSFX()
    {
        PlaySound(m_MovementSFX);
    }
}
