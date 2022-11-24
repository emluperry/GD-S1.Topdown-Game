using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject m_EnemyPrefabType;
    [SerializeField] private int m_NumEnemies;

    private Enemy_Movement[] m_Enemies;

    void Awake()
    {
        m_Enemies = new Enemy_Movement[m_NumEnemies];
        for(int i = 0; i < m_NumEnemies; i++)
        {
            m_Enemies[i] = Instantiate(m_EnemyPrefabType, transform.position, Quaternion.identity, transform).GetComponent<Enemy_Movement>();
        }
    }

    public void SetPause(bool paused)
    {
        foreach(Enemy_Movement enemy in m_Enemies)
        {
            enemy.m_IsPaused = paused;
        }
    }
}
