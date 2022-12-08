using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : Trigger
{
    [SerializeField] private GameObject m_EnemyPrefabType;
    [SerializeField] private int m_NumEnemies;

    private Enemy_Handler[] m_Enemies;
    private Player_Movement m_Player;

    public Action OnAllEnemiesKilled;

    private void Start()
    {
        m_Enemies = new Enemy_Handler[m_NumEnemies];
        for(int i = 0; i < m_NumEnemies; i++)
        {
            m_Enemies[i] = Instantiate(m_EnemyPrefabType, transform.position, Quaternion.identity, transform).GetComponent<Enemy_Handler>();
            m_Enemies[i].OnEnemyKilled += OnEnemyKilled;
            m_Enemies[i].Initialise(m_Player);
        }
    }

    private void OnDestroy()
    {
        foreach(Enemy_Handler enemy in m_Enemies)
        {
            enemy.OnEnemyKilled -= OnEnemyKilled;
        }
    }

    public void SetPause(bool paused)
    {
        foreach(Enemy_Handler enemy in m_Enemies)
        {
            enemy.SetPaused(paused);
        }
    }

    public void SetPlayerObject(Player_Movement player)
    {
        m_Player = player;
    }

    private void OnEnemyKilled()
    {
        m_NumEnemies--;

        if (m_NumEnemies <= 0 && !m_CurrentState)
        {
            OnAllEnemiesKilled?.Invoke();
            ChangeState(true);
        }
    }
}
