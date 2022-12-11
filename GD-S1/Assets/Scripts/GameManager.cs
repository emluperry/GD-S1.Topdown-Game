using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class GameManager : MonoBehaviour
{
    [Header("Core Elements")]
    [SerializeField] private Player_Handler m_Player;
    [SerializeField] private int m_NumKeys = 0;
    [SerializeField] private bool m_HasBossKey = false;

    [SerializeField] private Enemy_Spawner[] m_Spawners;
    private int m_SpawnerCount = 0;

    [SerializeField] private Object_Container[] m_ContainerObjects;
    [SerializeField] private Object_Unlockable[] m_UnlockableObjects;
    [SerializeField] private Object_FireSwitch[] m_FireSwitches;
    [SerializeField] private Object_WaterCollection[] m_WaterCollections;

    [Header("Transition Variables")]
    [SerializeField] private float m_maxButtonCooldown = 1f;
    private float m_pauseDelay = 0f;
    private bool m_IsPaused = false;
    private bool m_IsLevelActive = true;

    public Action<SEGMENT_TYPE, float> OnStatValueChange;
    public Action<COLLECTABLE_TYPE, int> OnCollectableValueChange;
    public Action<AFFINITY_TYPE> OnAffinityTypeChange;
    public Action<bool> OnAffinitySet;

    public Action<bool> OnPauseWorld;
    public Action<bool> OnLevelEnd;

    void Awake()
    {
        foreach(Object_FireSwitch switchObj in m_FireSwitches)
        {
            switchObj.OnMagicHit += LoseMagic;
        }

        foreach(Object_WaterCollection waterCol in m_WaterCollections)
        {
            waterCol.OnMagicHit += LoseMagic;
        }

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

        m_Player.UpdateCurrentAffinity += UpdateHUDAbility;
        m_Player.SetCurrentAffinity += SetHUDAbility;
        m_Player.OnStatValueChange += UpdateHUDBar;
        m_Player.OnKilled += PlayerKilled;
    }

    private void OnDestroy()
    {
        m_Player.OnStatValueChange -= UpdateHUDBar;
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
            m_UnlockableObjects[i].OnUnlockAttempt -= UnlockAttempt;
        }

        foreach (Object_FireSwitch switchObj in m_FireSwitches)
        {
            switchObj.OnMagicHit -= LoseMagic;
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

    private void UpdateHUDBar(SEGMENT_TYPE statType, float dec)
    {
        OnStatValueChange?.Invoke(statType, dec);
    }

    private void UpdateHUDValue(COLLECTABLE_TYPE valueType, int increment)
    {
        OnCollectableValueChange?.Invoke(valueType, increment);
    }

    private void UpdateHUDAbility(AFFINITY_TYPE type)
    {
        OnAffinityTypeChange?.Invoke(type);
    }

    private void SetHUDAbility(bool wasSet)
    {
        OnAffinitySet?.Invoke(wasSet);
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
                UpdateHUDValue(collectable, value);
                m_NumKeys += value;
                break;
            case COLLECTABLE_TYPE.BOSS_KEY:
                m_HasBossKey = true;
                UpdateHUDValue(collectable, value);
                break;
            case COLLECTABLE_TYPE.COIN:
                //save elsewhere for transfer between scenes/levels
                UpdateHUDValue(collectable, value);
                break;
            case COLLECTABLE_TYPE.HEALTH:
                m_Player.RecoverStat(value, collectable);
                break;
            case COLLECTABLE_TYPE.MAGIC:
                m_Player.RecoverStat(value, collectable);
                break;
        }
    }

    private void UnlockAttempt(int UnlockableID, COLLECTABLE_TYPE type)
    {
        bool CanUnlock = false;

        switch(type)
        {
            case COLLECTABLE_TYPE.KEY:
                if (m_NumKeys >= 1)
                {
                    m_NumKeys--;
                    CanUnlock = true;
                    UpdateHUDValue(COLLECTABLE_TYPE.KEY, -1);
                }
                break;

            case COLLECTABLE_TYPE.BOSS_KEY:
                if(m_HasBossKey)
                {
                    m_HasBossKey = false;
                    CanUnlock = true;
                    UpdateHUDValue(COLLECTABLE_TYPE.BOSS_KEY, -1);
                }
                break;
        }

        m_UnlockableObjects[UnlockableID].ShouldUnlock(CanUnlock);
    }

    private void LoseMagic(int cost)
    {
        m_Player.LoseMagic(cost);
    }
}
