using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StartMenu : UI_Abstract
{
    [SerializeField] private UI_OnClickButton m_StartButton;
    [SerializeField] private UI_OnClickButton m_SettingsButton;
    [SerializeField] private UI_OnClickButton m_QuitButton;

    private void Awake()
    {
        m_StartButton.OnClicked += StartGame;
        m_SettingsButton.OnClicked += LoadSettingsUI;
        m_QuitButton.OnClicked += QuitGame;
    }

    private void OnDestroy()
    {
        m_StartButton.OnClicked -= StartGame;
        m_SettingsButton.OnClicked -= LoadSettingsUI;
        m_QuitButton.OnClicked -= QuitGame;
    }

    private void StartGame()
    {
        LoadSceneOnButtonClicked?.Invoke(SCENE_TYPE.LEVEL_SELECT);
    }

    private void LoadSettingsUI()
    {
        LoadUIOnButtonClicked?.Invoke(UI_SCREENS.SETTINGS);
    }

    private void QuitGame()
    {
        LoadSceneOnButtonClicked?.Invoke(SCENE_TYPE.QUIT_GAME);
    }
}
