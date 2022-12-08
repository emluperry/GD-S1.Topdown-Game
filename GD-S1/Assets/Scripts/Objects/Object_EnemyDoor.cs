using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_EnemyDoor : Object_Door
{
    [SerializeField] private Enemy_Spawner[] m_Spawners;
    private int m_ClearedSpawners = 0;

    protected override void Awake()
    {
        base.Awake();

        foreach (Enemy_Spawner spawnerObj in m_Spawners)
        {
            spawnerObj.OnAllEnemiesKilled += UpdateDoorState;
        }
    }

    private void OnDestroy()
    {
        foreach (Enemy_Spawner spawnerObj in m_Spawners)
        {
            spawnerObj.OnAllEnemiesKilled -= UpdateDoorState;
        }
    }

    private void UpdateDoorState()
    {
        m_ClearedSpawners++;

        if(m_ClearedSpawners >= m_Spawners.Length)
        {
            ChangeState(false);
        }
    }
}
