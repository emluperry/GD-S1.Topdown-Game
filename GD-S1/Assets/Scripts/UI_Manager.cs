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
    GLOSSARY_BESTIARY,
    GAME_LOSE,
    GAME_WIN
}

public class UI_Manager : MonoBehaviour
{
    [Header("UI Prefabs")]
    [SerializeField] private GameObject m_StartMenuButtonsPrefab;
    [SerializeField] private GameObject m_PauseMenuPrefab;
    [SerializeField] private GameObject m_SettingsMenuPrefab;
    [SerializeField] private GameObject m_GlossarySelectMenuPrefab;
    [SerializeField] private GameObject m_GameWinScreenPrefab;
    [SerializeField] private GameObject m_GameLoseScreenPrefab;

    private UI_Abstract m_StartMenu;
    private UI_PauseButtonHandler m_PauseMenu;
    private UI_SettingsHandler m_SettingsMenu;
    private UI_Abstract m_GlossarySelectMenu;
    private UI_WinScreen m_WinScreenMenu;
    private UI_Abstract m_LoseScreenMenu;

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

                m_PauseMenu.LoadUI += LoadUIScreen;
                m_PauseMenu.LoadScene += LoadSceneCall;
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
                    m_GlossarySelectMenu = Instantiate(m_GlossarySelectMenuPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_Abstract>();
                else
                    m_GlossarySelectMenu.gameObject.SetActive(true);
                m_GlossarySelectMenu.onBackButton += BackButton;
                break;

            case UI_SCREENS.GAME_WIN:
                if (m_WinScreenMenu == null)
                    m_WinScreenMenu = Instantiate(m_GameWinScreenPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_WinScreen>();
                else
                    m_WinScreenMenu.gameObject.SetActive(true);
                break;

            case UI_SCREENS.GAME_LOSE:
                if (m_LoseScreenMenu == null)
                    m_LoseScreenMenu = Instantiate(m_GameLoseScreenPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_Abstract>();
                else
                    m_LoseScreenMenu.gameObject.SetActive(true);
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
                m_PauseMenu.LoadUI -= LoadUIScreen;
                m_PauseMenu.LoadScene -= LoadSceneCall;
                break;
            case UI_SCREENS.SETTINGS:
                m_SettingsMenu.gameObject.SetActive(false);
                m_SettingsMenu.onBackButton -= BackButton;
                break;
            case UI_SCREENS.GLOSSARY_SELECT:
                m_GlossarySelectMenu.gameObject.SetActive(false);
                break;
            case UI_SCREENS.GAME_WIN:
                m_WinScreenMenu.gameObject.SetActive(false);
                break;
            case UI_SCREENS.GAME_LOSE:
                m_LoseScreenMenu.gameObject.SetActive(false);
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
        obj.LoadUI += LoadUIScreen;
        obj.LoadScene += LoadSceneCall;
        obj.LoadLevelByIndex += LoadLevelCall;
    }

    public void StopListeningForUI(UI_Abstract obj)
    {
        obj.LoadUI -= LoadUIScreen;
        obj.LoadScene -= LoadSceneCall;
        obj.LoadLevelByIndex -= LoadLevelCall;
    }
}
