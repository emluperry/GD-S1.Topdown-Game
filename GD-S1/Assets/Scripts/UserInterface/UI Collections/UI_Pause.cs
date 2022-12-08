using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Pause : UI_Abstract
{
    [SerializeField] private Button_UIOnClick m_ContinueButton;
    [SerializeField] private Button_UIOnClickUI m_GlossaryButton;
    [SerializeField] private Button_UIOnClickUI m_SettingsButton;
    [SerializeField] private Button_UIOnClick m_SaveButton;
    [SerializeField] private Button_UIOnClick m_QuitToTitleButton;
    [SerializeField] private Button_UIOnClick m_QuitGameButton;

    public Action onUnpause;

    private void Awake()
    {
        m_ContinueButton.OnClicked += Unpause;
        m_GlossaryButton.OnClicked += LoadUI;
        m_SettingsButton.OnClicked += LoadUI;
        //m_SaveButton.OnClicked += SaveGame;
        m_QuitToTitleButton.OnClicked += QuitToTitle;
        m_QuitGameButton.OnClicked += Quit;
    }

    private void OnDestroy()
    {
        m_ContinueButton.OnClicked -= Unpause;
        m_GlossaryButton.OnClicked -= LoadUI;
        m_SettingsButton.OnClicked -= LoadUI;
        //m_SaveButton.OnClicked -= SaveGame;
        m_QuitToTitleButton.OnClicked -= QuitToTitle;
        m_QuitGameButton.OnClicked -= Quit;
    }

    private void Unpause()
    {
        onUnpause?.Invoke();
    }
}
