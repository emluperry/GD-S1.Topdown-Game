using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI & Scene Management")]
    [SerializeField] private UI_Healthbar m_Healthbar;
    [SerializeField] private UI_Manager m_UIManager;

    [Header("Core Elements")]
    [SerializeField] private GameObject m_Player;
    private Entity_Health m_PlayerHealth;
    private Player_Movement m_PlayerMov;
    [SerializeField] private Weapon_Movement m_PlayerWeapon;

    [SerializeField] private Enemy_Spawner m_Spawner;

    [Header("Transition Variables")]
    [SerializeField] private float m_maxButtonCooldown = 1f;
    private float m_pauseDelay = 0f;
    private bool m_IsPaused = false;

    public Action<bool> OnPauseWorld;

    void Awake()
    {
        m_PlayerHealth = m_Player.GetComponent<Entity_Health>();
        m_PlayerMov = m_Player.GetComponent<Player_Movement>();

        m_Spawner.SetPlayerObject(m_PlayerMov);

        m_PlayerHealth.DamageTaken += m_Healthbar.UpdateHealth;
    }

    private void OnDestroy()
    {
        m_PlayerHealth.DamageTaken -= m_Healthbar.UpdateHealth;
    }

    private void Update()
    {
        if (m_pauseDelay < m_maxButtonCooldown)
        {
            m_pauseDelay += Time.deltaTime;
        }
        else if (Input.GetAxis("Pause") > 0)
        {
            TogglePauseGameObjects(!m_IsPaused);
            OnPauseWorld?.Invoke(m_IsPaused);
        }
    }

    public void TogglePauseGameObjects(bool paused)
    {
        m_IsPaused = paused;
        m_PlayerMov.m_IsPaused = paused;
        m_PlayerWeapon.m_IsPaused = paused;
        m_Spawner.SetPause(paused);

        m_pauseDelay = 0f;
    }
}
