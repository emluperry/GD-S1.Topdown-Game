using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enums;

public class Scene_Manager : MonoBehaviour
{
    [Header("Other Managers")]
    [SerializeField] private UIManager m_UIManager;
    [SerializeField] private SettingsManager m_SettingsManager;
    private GameManager m_CurrentGameManager;

    [SerializeField] private Camera m_Camera;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }

        if (m_SettingsManager)
            m_SettingsManager.RestoreSavedSettings();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;

        m_UIManager.CallLoadScene += LoadScene;
        m_UIManager.CallLoadNextScene += LoadNextLevel;
        m_UIManager.OnGamePaused += PauseGame;
        m_UIManager.CallQuitApp += QuitApplication;
        m_UIManager.CallReloadScene += RestartLevel;
        m_UIManager.OnLoadSettings += LoadSettings;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;

        m_UIManager.CallLoadScene -= LoadScene;
        m_UIManager.CallLoadNextScene -= LoadNextLevel;
        m_UIManager.OnGamePaused -= PauseGame;
        m_UIManager.CallQuitApp -= QuitApplication;
        m_UIManager.CallReloadScene -= RestartLevel;
        m_UIManager.OnLoadSettings -= LoadSettings;
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        m_UIManager.OnLevelLoaded();

        SceneManager.SetActiveScene(scene);

        m_CurrentGameManager = FindObjectOfType<GameManager>();
        if (m_CurrentGameManager)
        {
            m_CurrentGameManager.OnStatValueChange += m_UIManager.UpdateBar;
            m_CurrentGameManager.OnCollectableValueChange += m_UIManager.UpdateValue;
            m_CurrentGameManager.OnAffinityTypeChange += m_UIManager.UpdateAffinityTypeChange;
            m_CurrentGameManager.OnAffinitySet += m_UIManager.UpdateAffinitySet;
            m_CurrentGameManager.OnPauseWorld += m_UIManager.LoadPauseMenu;
            m_CurrentGameManager.OnLevelEnd += m_UIManager.LoadWinLoseScreen;
        }

        m_UIManager.FadeOut();
    }

    private void StopListeningForEvents()
    {
        if (m_CurrentGameManager)
        {
            m_CurrentGameManager.OnStatValueChange -= m_UIManager.UpdateBar;
            m_CurrentGameManager.OnCollectableValueChange -= m_UIManager.UpdateValue;
            m_CurrentGameManager.OnPauseWorld -= m_UIManager.LoadPauseMenu;
            m_CurrentGameManager.OnLevelEnd -= m_UIManager.LoadWinLoseScreen;
        }

        m_UIManager.StopListeningForEvents(ref m_SettingsManager);
    }

    private void LoadScene(int BuildIndex)
    {
        StopListeningForEvents();

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(BuildIndex, LoadSceneMode.Additive);

        m_UIManager.FadeIn(loadOperation);
    }

    private void RestartLevel()
    {
        int BuildIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(BuildIndex);
    }

    private void LoadNextLevel()
    {
        int BuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(BuildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            BuildIndex = 1;
        }
        LoadScene(BuildIndex);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }

    private void PauseGame(bool paused)
    {
        if(!paused)
        {
            m_SettingsManager.RestoreSavedSettings();
        }

        m_CurrentGameManager.TogglePauseGameObjects(paused);
    }

    private void LoadSettings(bool SettingsExists)
    {
        if (SettingsExists)
        {
            m_UIManager.LoadSettings(m_SettingsManager);
        }
        m_SettingsManager.RestoreSettingMenuValues();
    }
}
