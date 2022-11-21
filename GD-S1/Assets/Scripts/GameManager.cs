using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private UI_Healthbar m_Healthbar;

    [Header("Core Elements")]
    [SerializeField] private Entity_Health m_Player;


    void Start()
    {
        m_Player.DamageTaken += m_Healthbar.UpdateHealth;
    }

    private void OnDestroy()
    {
        m_Player.DamageTaken -= m_Healthbar.UpdateHealth;
    }
}
