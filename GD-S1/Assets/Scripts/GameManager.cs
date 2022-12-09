using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class GameManager : MonoBehaviour
{
    [Header("UI & Scene Management")]
    [SerializeField] private UI_Healthbar m_Healthbar;

    [Header("Core Elements")]
    [SerializeField] private Player_Handler m_Player;
    [SerializeField] private int m_NumKeys = 0;
    [SerializeField] private bool m_HasBossKey = false;

    [SerializeField] private Enemy_Spawner[] m_Spawners;
    private int m_SpawnerCount = 0;

    [SerializeField] private Object_Container[] m_ContainerObjects;
    [SerializeField] private Object_Unlockable[] m_UnlockableObjects;

    [Header("Transition Variables")]
    [SerializeField] private float m_maxButtonCooldown = 1f;
    private float m_pauseDelay = 0f;
    private bool m_IsPaused = false;
    private bool m_IsLevelActive = true;

    public Action<bool> OnPauseWorld;
    public Action<bool> OnLevelEnd;

    void Awake()
    {
        foreach (Enemy_Spawner spawner in m_Spawners)
        {
            spawner.SetPlayerObject(m_Player.GetPlayerMovementComponent());
            spawner.OnAllEnemiesKilled += OnSpawnerCleared;
        }
        m_SpawnerCount = m_Spawners.Length;

        foreach(Object_Container breakable in m_ContainerObjects)
        {
            breakable.OnCollectableCollected += UpdateCollectedItems;
        }

        for(int i = 0; i < m_UnlockableObjects.Length; i++)
        {
            m_UnlockableObjects[i].m_UnlockableID = i;
            m_UnlockableObjects[i].OnUnlockAttempt += UnlockAttempt;
        }

        m_Player.OnDamageTaken += m_Healthbar.UpdateHealth;
        m_Player.OnKilled += PlayerKilled;
    }

    private void OnDestroy()
    {
        m_Player.OnDamageTaken -= m_Healthbar.UpdateHealth;
        m_Player.OnKilled -= PlayerKilled;

        foreach(Enemy_Spawner spawner in m_Spawners)
        {
            spawner.OnAllEnemiesKilled -= OnSpawnerCleared;
        }

        foreach (Object_Container breakable in m_ContainerObjects)
        {
            breakable.OnCollectableCollected -= UpdateCollectedItems;
        }

        for (int i = 0; i < m_UnlockableObjects.Length; i++)
        {
            m_UnlockableObjects[i].m_UnlockableID = i;
            m_UnlockableObjects[i].OnUnlockAttempt -= UnlockAttempt;
        }
    }

    private void Update()
    {
        if (!m_IsLevelActive)
            return;

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

        foreach(Enemy_Spawner spawner in m_Spawners)
        {
            spawner.SetPause(paused);
        }

        m_pauseDelay = 0f;
    }

    private void PlayerKilled()
    {
        OnLevelEnd?.Invoke(false);
        TogglePauseGameObjects(true);
        m_IsLevelActive = false;
    }

    private void OnSpawnerCleared()
    {
        m_SpawnerCount--;

        if(m_SpawnerCount <= 0)
        {
            OnLevelEnd?.Invoke(true);
            TogglePauseGameObjects(true);
            m_IsLevelActive = false;
        }
    }

    private void UpdateCollectedItems(COLLECTABLE_TYPE collectable, int value)
    {
        switch (collectable)
        {
            case COLLECTABLE_TYPE.KEY:
                m_NumKeys += value;
                break;
        }
    }

    private void UnlockAttempt(int UnlockableID)
    {
        bool CanUnlock = false;
        if(m_NumKeys >= 1)
        {
            m_NumKeys--;
            CanUnlock = true;
        }

        m_UnlockableObjects[UnlockableID].ShouldUnlock(CanUnlock);
    }
}
