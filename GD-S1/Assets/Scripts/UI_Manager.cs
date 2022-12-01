using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UI_SCREENS
{
    NONE,
    PAUSE,
    SETTINGS,
    GLOSSARY_SELECT,
    GLOSSARY_TUTORIAL,
    GLOSSARY_BESTIARY
}

public class UI_Manager : MonoBehaviour
{
    [Header("UI Prefabs")]
    [SerializeField] private GameObject m_StartMenuButtonsPrefab;
    [SerializeField] private GameObject m_PauseMenuPrefab;
    [SerializeField] private GameObject m_SettingsMenuPrefab;
    [SerializeField] private GameObject m_GlossarySelectMenuPrefab;

    private UI_PauseButtonHandler m_PauseMenu;
    private UI_SettingsHandler m_SettingsMenu;
    private GameObject m_GlossarySelectMenu;

    private bool m_IsUIActive = false;

    private Stack<UI_SCREENS> m_UIScreenStack;

    public Action<SCENE_TYPE> LoadSceneOnButtonClicked;
    public Action<int> LevelIndexToLoad;

    private void Awake()
    {
        m_UIScreenStack = new Stack<UI_SCREENS>();
        m_UIScreenStack.Push(UI_SCREENS.NONE);
    }

    private void LoadUIScreen(UI_SCREENS screen)
    {
        HideUIScreen(m_UIScreenStack.Peek());

        switch (screen)
        {
            case UI_SCREENS.PAUSE:
                if (m_PauseMenu == null)
                    m_PauseMenu = Instantiate(m_PauseMenuPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_PauseButtonHandler>();
                else
                    m_PauseMenu.gameObject.SetActive(true);

                m_PauseMenu.LoadUIOnButtonClicked += LoadUIScreen;
                m_PauseMenu.LoadSceneOnButtonClicked += LoadSceneCall;
                break;
                
            case UI_SCREENS.SETTINGS:
                if (m_SettingsMenu == null)
                    m_SettingsMenu = Instantiate(m_SettingsMenuPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_SettingsHandler>();
                else
                    m_SettingsMenu.gameObject.SetActive(true);
                m_SettingsMenu.onBackButton += BackButton;
                break;

            case UI_SCREENS.GLOSSARY_SELECT:
                if (m_GlossarySelectMenu == null)
                    m_GlossarySelectMenu = Instantiate(m_GlossarySelectMenuPrefab, Vector3.zero, Quaternion.identity, transform);
                else
                    m_GlossarySelectMenu.gameObject.SetActive(true);
                //m_GlossarySelectMenu.onBackButton += BackButton;
                break;
        }

        m_UIScreenStack.Push(screen);
    }

    private void HideUIScreen(UI_SCREENS screen)
    {
        switch (screen)
        {
            case UI_SCREENS.PAUSE:
                m_PauseMenu.gameObject.SetActive(false);
                m_PauseMenu.LoadUIOnButtonClicked -= LoadUIScreen;
                m_PauseMenu.LoadSceneOnButtonClicked -= LoadSceneCall;
                break;
            case UI_SCREENS.SETTINGS:
                m_SettingsMenu.gameObject.SetActive(false);
                m_SettingsMenu.onBackButton -= BackButton;
                break;
            case UI_SCREENS.GLOSSARY_SELECT:
                m_GlossarySelectMenu.gameObject.SetActive(false);
                break;
        }
    }

    private void BackButton()
    {
        HideUIScreen(m_UIScreenStack.Pop());

        LoadUIScreen(m_UIScreenStack.Peek());
    }

    public void PauseGame(bool isPaused)
    {
        if (isPaused)
        {
            ClearStackToNone();
        }
        else
        {
            LoadUIScreen(UI_SCREENS.PAUSE);
        }
    }

    private void ClearStackToNone()
    {
        UI_SCREENS screen;
        if (m_UIScreenStack.TryPop(out screen))
            HideUIScreen(screen);

        while(m_UIScreenStack.TryPeek(out screen) && screen != UI_SCREENS.NONE)
        {
            m_UIScreenStack.Pop();
        }

        m_IsUIActive = false;
    }

    private void LoadSceneCall(SCENE_TYPE scene)
    {
        LoadSceneOnButtonClicked?.Invoke(scene);
    }

    private void LoadLevelCall(int index)
    {
        LevelIndexToLoad?.Invoke(index);
    }

    public void StartListeningForUI(UI_Abstract obj)
    {
        obj.LoadUIOnButtonClicked += LoadUIScreen;
        obj.LoadSceneOnButtonClicked += LoadSceneCall;
        obj.LoadLevelByIndex += LoadLevelCall;
    }

    public void StopListeningForUI(UI_Abstract obj)
    {
        obj.LoadUIOnButtonClicked -= LoadUIScreen;
        obj.LoadSceneOnButtonClicked -= LoadSceneCall;
        obj.LoadLevelByIndex -= LoadLevelCall;
    }
}
