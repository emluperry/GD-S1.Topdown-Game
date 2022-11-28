using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool m_IsPaused = false;

    [Header("UI Elements")]
    [SerializeField] private UI_Healthbar m_Healthbar;
    [SerializeField] private UI_PauseButtonHandler m_PauseMenu;

    [Header("Core Elements")]
    [SerializeField] private GameObject m_Player;
    private Entity_Health m_PlayerHealth;
    private Player_Movement m_PlayerMov;
    [SerializeField] private Weapon_Movement m_PlayerWeapon;

    [SerializeField] private Enemy_Spawner m_Spawner;

    private float m_pauseDelay = 0f;
    [SerializeField] private float m_maxButtonCooldown = 1f;


    void Awake()
    {
        m_PlayerHealth = m_Player.GetComponent<Entity_Health>();
        m_PlayerMov = m_Player.GetComponent<Player_Movement>();

        m_Spawner.SetPlayerObject(m_PlayerMov);

        m_PlayerHealth.DamageTaken += m_Healthbar.UpdateHealth;

        m_PauseMenu.gameObject.SetActive(false);
        m_PauseMenu.onContinue += TogglePauseGameObjects;
    }

    private void OnDestroy()
    {
        m_PlayerHealth.DamageTaken -= m_Healthbar.UpdateHealth;

        m_PauseMenu.onContinue += TogglePauseGameObjects;
    }

    private void Update()
    {
        if(m_pauseDelay < m_maxButtonCooldown)
        {
            m_pauseDelay += Time.deltaTime;
        }
        else if (Input.GetAxis("Pause") > 0)
        {
            TogglePauseGameObjects();
            m_pauseDelay = 0f;
        }
    }

    private void TogglePauseGameObjects()
    {
        m_IsPaused = !m_IsPaused;

        m_PauseMenu.gameObject.SetActive(m_IsPaused);

        m_PlayerMov.m_IsPaused = m_IsPaused;
        m_PlayerWeapon.m_IsPaused = m_IsPaused;
        m_Spawner.SetPause(m_IsPaused);
    }
}
