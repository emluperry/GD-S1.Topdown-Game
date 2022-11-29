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
    [SerializeField] private Scene_Manager m_SceneManager;

    [Header("Core Elements")]
    [SerializeField] private GameObject m_Player;
    private Entity_Health m_PlayerHealth;
    private Player_Movement m_PlayerMov;
    [SerializeField] private Weapon_Movement m_PlayerWeapon;

    [SerializeField] private Enemy_Spawner m_Spawner;

    void Awake()
    {
        m_PlayerHealth = m_Player.GetComponent<Entity_Health>();
        m_PlayerMov = m_Player.GetComponent<Player_Movement>();

        m_Spawner.SetPlayerObject(m_PlayerMov);

        m_PlayerHealth.DamageTaken += m_Healthbar.UpdateHealth;

        m_UIManager.onPauseWorld += TogglePauseGameObjects;
        m_UIManager.LoadSceneOnButtonClicked += LoadScene;
    }

    private void OnDestroy()
    {
        m_PlayerHealth.DamageTaken -= m_Healthbar.UpdateHealth;

        m_UIManager.onPauseWorld += TogglePauseGameObjects;
        m_UIManager.LoadSceneOnButtonClicked -= LoadScene;
    }

    private void TogglePauseGameObjects(bool paused)
    {
        m_PlayerMov.m_IsPaused = paused;
        m_PlayerWeapon.m_IsPaused = paused;
        m_Spawner.SetPause(paused);
    }

    private void LoadScene(SCENE_TYPE scene)
    {
        //call in scene manager
    }
}
