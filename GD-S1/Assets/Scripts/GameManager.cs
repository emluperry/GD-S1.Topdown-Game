using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI & Scene Management")]
    [SerializeField] private UI_Healthbar m_Healthbar;

    [Header("Core Elements")]
    [SerializeField] private Player_Handler m_Player;

    [SerializeField] private Enemy_Spawner m_Spawner;

    [Header("Transition Variables")]
    [SerializeField] private float m_maxButtonCooldown = 1f;
    private float m_pauseDelay = 0f;
    private bool m_IsPaused = false;

    public Action<bool> OnPauseWorld;
    public Action OnPlayerKilled;
    public Action OnLevelComplete;

    void Awake()
    {
        m_Spawner.SetPlayerObject(m_Player.GetPlayerMovementComponent());

        m_Player.OnDamageTaken += m_Healthbar.UpdateHealth;
        m_Player.OnKilled += PlayerKilled;
    }

    private void OnDestroy()
    {
        m_Player.OnDamageTaken -= m_Healthbar.UpdateHealth;
        m_Player.OnKilled -= PlayerKilled;
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
        m_Player.SetPause(paused);
        m_Spawner.SetPause(paused);

        m_pauseDelay = 0f;
    }

    private void PlayerKilled()
    {
        OnPlayerKilled?.Invoke();
        TogglePauseGameObjects(true);
    }
}
