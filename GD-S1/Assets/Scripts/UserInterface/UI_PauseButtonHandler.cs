using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PauseButtonHandler : MonoBehaviour
{
    [SerializeField] private UI_OnClickButton m_ResumeButton;
    [SerializeField] private UI_OnClickButton m_GlossaryButton;
    [SerializeField] private UI_OnClickButton m_SaveButton;
    [SerializeField] private UI_OnClickButton m_SettingsButton;
    [SerializeField] private UI_OnClickButton m_QuitLevelButton;
    [SerializeField] private UI_OnClickButton m_ExitGameButton;

    public Action onContinue;
    public Action onQuitLevel;
    public Action onQuitGame;

    private void Awake()
    {
        m_ResumeButton.OnClicked += ResumeGame;
        m_GlossaryButton.OnClicked += LoadGlossaryUI;
        m_SaveButton.OnClicked += SaveGame;
        m_SettingsButton.OnClicked += LoadSettingsUI;
        m_QuitLevelButton.OnClicked += QuitLevel;
        m_ExitGameButton.OnClicked += QuitToTitle;
    }

    private void OnDestroy()
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
        gameObject.SetActive(false);
        onContinue?.Invoke();
    }

    private void LoadGlossaryUI()
    {
        //load glossary UI
    }

    private void LoadSettingsUI()
    {
        //load settings ui
    }

    private void SaveGame()
    {
        //save game
    }

    private void QuitLevel()
    {
        onQuitLevel?.Invoke();
    }

    private void QuitToTitle()
    {
        onQuitGame?.Invoke();
    }
}
