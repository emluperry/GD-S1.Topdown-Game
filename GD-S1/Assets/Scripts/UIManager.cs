using Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Prefab Objects")]
    [SerializeField] private GameObject m_LevelSelectPrefab;
    [SerializeField] private GameObject m_SettingsPrefab;
    [SerializeField] private GameObject m_GlossaryPrefab;
    [SerializeField] private GameObject m_PausePrefab;
    [SerializeField] private GameObject m_WinPrefab;
    [SerializeField] private GameObject m_LosePrefab;

    [Header("Objects")]
    private UI_Abstract m_LevelSelect;
    private UI_Abstract m_Settings;
    private UI_Abstract m_Glossary;
    private UI_Abstract m_Pause;
    private UI_Abstract m_Win;
    private UI_Abstract m_Lose;

    [Header("Loading Variables")]
    [SerializeField] private CanvasGroup m_LoadScreenObject;
    [SerializeField] private float m_FadeInOutTime = 1f;

    private Stack<UI_Abstract> m_UIStack;
    private List<UI_Abstract> m_ActiveUIObjects;

    public Action<int> CallLoadScene;
    public Action<bool> OnGamePaused;
    public Action<bool> OnLoadSettings;
    public Action CallQuitApp;
    public Action CallLoadNextScene;
    public Action CallReloadScene;

    private void OnEnable()
    {
        m_UIStack = new Stack<UI_Abstract>();
    }

    public void OnLevelLoaded()
    {
        m_UIStack.Clear();

        m_ActiveUIObjects = new List<UI_Abstract>();
        m_ActiveUIObjects.AddRange(FindObjectsOfType<UI_Abstract>());

        foreach (UI_Abstract uiObject in m_ActiveUIObjects)
        {
            ListenForEventsIn(uiObject);

            m_UIStack.Push(uiObject);
        }
    }

    public void ListenForEventsIn(UI_Abstract uiObject)
    {
        uiObject.CallLoadScene += LoadScene;
        uiObject.CallQuitApp += QuitApplication;
        uiObject.CallLoadNextScene += LoadNextLevel;
        uiObject.CallReloadScene += RestartLevel;
        uiObject.CallLoadUI += LoadUI;
    }

    public void StopListeningForEvents(ref SettingsManager settingsManager)
    {
        if (settingsManager && m_Settings)
        {
            settingsManager.StopListeningForEvents();
        }

        if (m_Pause)
            m_Pause.GetComponent<UI_Pause>().onUnpause -= Unpause;


        if (m_ActiveUIObjects.Count <= 0)
            return;

        foreach (UI_Abstract uiObject in m_ActiveUIObjects)
        {
            uiObject.CallLoadScene -= LoadScene;
            uiObject.CallQuitApp -= QuitApplication;
            uiObject.CallLoadNextScene -= LoadNextLevel;
            uiObject.CallReloadScene -= RestartLevel;
            uiObject.CallLoadUI -= LoadUI;
        }

        m_ActiveUIObjects.Clear();
    }

    private void LoadScene(int BuildIndex) { CallLoadScene?.Invoke(BuildIndex); }
    private void QuitApplication() { CallQuitApp?.Invoke(); }
    private void LoadNextLevel() { CallLoadNextScene?.Invoke(); }
    private void RestartLevel() { CallReloadScene?.Invoke(); }

    public IEnumerator FadeIn()
    {
        float Increment = 1 / m_FadeInOutTime;
        m_LoadScreenObject.gameObject.SetActive(true);
        m_LoadScreenObject.blocksRaycasts = true;
        m_LoadScreenObject.alpha = 0;
        while (m_LoadScreenObject.alpha < 1)
        {
            m_LoadScreenObject.alpha += Increment;
            yield return new WaitForFixedUpdate();
        }
        m_LoadScreenObject.alpha = 1;
    }

    public IEnumerator FadeOut()
    {
        float Increment = 1 / m_FadeInOutTime;
        m_LoadScreenObject.alpha = 1;
        while (m_LoadScreenObject.alpha < 1)
        {
            m_LoadScreenObject.alpha -= Increment;
            yield return new WaitForFixedUpdate();
        }
        m_LoadScreenObject.alpha = 0;
        m_LoadScreenObject.blocksRaycasts = false;
        m_LoadScreenObject.gameObject.SetActive(false);
    }

    public void LoadWinLoseScreen(bool playerWon)
    {
        if (playerWon)
            LoadUI(UI_STATE.WIN);
        else
            LoadUI(UI_STATE.LOSE);
    }

    private void Unpause() { LoadPauseMenu(false); }

    public void LoadPauseMenu(bool paused)
    {
        if (paused)
        {
            LoadUI(UI_STATE.PAUSED);
        }
        else
        {
            UI_Abstract uiObject;
            while (m_UIStack.TryPeek(out uiObject) && !uiObject.GetComponent<UI_Pause>())
            {
                LoadUI(UI_STATE.BACK);
            }
            if (uiObject && uiObject.GetComponent<UI_Pause>())
                LoadUI(UI_STATE.BACK);
        }

        OnGamePaused?.Invoke(paused);
    }

    private void LoadUI(UI_STATE UIScreen)
    {
        if (m_UIStack.TryPeek(out UI_Abstract LastUI))
            LastUI.gameObject.SetActive(false);

        switch (UIScreen)
        {
            case UI_STATE.NONE:
                m_UIStack.Clear();
                break;

            case UI_STATE.PAUSED:
                LoadUI(ref m_PausePrefab, ref m_Pause);
                if(m_Pause)
                    m_Pause.GetComponent<UI_Pause>().onUnpause += Unpause;
                break;

            case UI_STATE.SETTINGS:
                LoadUI(ref m_SettingsPrefab, ref m_Settings);
                OnLoadSettings?.Invoke(m_Settings);
                break;

            case UI_STATE.GLOSSARY:
                LoadUI(ref m_GlossaryPrefab, ref m_Glossary);
                break;

            case UI_STATE.WIN:
                LoadUI(ref m_WinPrefab, ref m_Win);
                break;

            case UI_STATE.LOSE:
                LoadUI(ref m_LosePrefab, ref m_Lose);
                break;

            case UI_STATE.LEVEL_SELECT:
                LoadUI(ref m_LevelSelectPrefab, ref m_LevelSelect);
                break;

            case UI_STATE.BACK:
                m_UIStack.Pop();
                if (m_UIStack.TryPeek(out LastUI))
                    LastUI.gameObject.SetActive(true);
                break;
        }
    }

    public void LoadSettings(SettingsManager settingsManager)
    {
        settingsManager.ListenForEvents(m_Settings.GetComponent<UI_Settings>());
    }

    private void LoadUI(ref GameObject prefab, ref UI_Abstract uiObject)
    {
        if (!uiObject)
        {
            uiObject = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<UI_Abstract>();
            m_ActiveUIObjects.Add(uiObject);
            ListenForEventsIn(uiObject);
        }
        else
            uiObject.gameObject.SetActive(true);
        m_UIStack.Push(uiObject);
    }
}
