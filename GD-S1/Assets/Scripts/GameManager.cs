using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool m_IsGamePaused = false;

    [Header("UI Elements")]
    [SerializeField] private UI_Healthbar m_Healthbar;
    [SerializeField] private UI_PauseButtonHandler m_PauseMenu;

    [Header("Core Elements")]
    [SerializeField] private Entity_Health m_Player;
    [SerializeField] private Enemy_Spawner m_Spawner;


    void Start()
    {
        m_Player.DamageTaken += m_Healthbar.UpdateHealth;

        m_PauseMenu.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        m_Player.DamageTaken -= m_Healthbar.UpdateHealth;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            m_PauseMenu.gameObject.SetActive(!m_PauseMenu.gameObject.activeSelf);
        }
    }
}
