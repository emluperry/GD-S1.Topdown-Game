using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PauseButtonHandler : UI_Abstract
{
    [Header("Buttons")]
    [SerializeField] private UI_OnClickButton m_ResumeButton;
    [SerializeField] private UI_OnClickButton m_GlossaryButton;
    [SerializeField] private UI_OnClickButton m_SaveButton;
    [SerializeField] private UI_OnClickButton m_SettingsButton;
    [SerializeField] private UI_OnClickButton m_QuitLevelButton;
    [SerializeField] private UI_OnClickButton m_ExitGameButton;

    public Action onSaveGame;

    private void OnEnable()
    {
        m_ResumeButton.OnClicked += ResumeGame;
        m_GlossaryButton.OnClicked += LoadGlossaryUI;
        m_SaveButton.OnClicked += SaveGame;
        m_SettingsButton.OnClicked += LoadSettingsUI;
        m_QuitLevelButton.OnClicked += QuitLevel;
        m_ExitGameButton.OnClicked += QuitToTitle;
    }

    private void OnDisable()
    {
        m_ResumeButton.OnClicked -= ResumeGame;
        m_GlossaryButton.OnClicked -= LoadGlossaryUI;
        m_SaveButton.OnClicked -= SaveGame;
        m_SettingsButton.OnClicked -= LoadSettingsUI;
        m_QuitLevelButton.OnClicked -= QuitLevel;
        m_ExitGameButton.OnClicked -= QuitToTitle;
    }

    private void ResumeGame()
    {
        LoadUI?.Invoke(UI_SCREENS.NONE);
    }

    private void LoadGlossaryUI()
    {
        LoadUI?.Invoke(UI_SCREENS.GLOSSARY_SELECT);
    }

    private void LoadSettingsUI()
    {
        LoadUI?.Invoke(UI_SCREENS.SETTINGS);
    }

    private void SaveGame()
    {
        onSaveGame?.Invoke();
    }

    private void QuitLevel()
    {
        LoadScene?.Invoke(SCENE_TYPE.LEVEL_SELECT);
    }

    private void QuitToTitle()
    {
        LoadScene?.Invoke(SCENE_TYPE.QUIT_GAME);
    }
}
