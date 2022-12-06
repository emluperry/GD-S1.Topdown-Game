using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject m_EnemyPrefabType;
    [SerializeField] private int m_NumEnemies;

    private Enemy_Handler[] m_Enemies;
    private Player_Movement m_Player;

    void Start()
    {
        m_Enemies = new Enemy_Handler[m_NumEnemies];
        for(int i = 0; i < m_NumEnemies; i++)
        {
            m_Enemies[i] = Instantiate(m_EnemyPrefabType, transform.position, Quaternion.identity, transform).GetComponent<Enemy_Handler>();
            m_Enemies[i].Initialise(m_Player);
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
}
