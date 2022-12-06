using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public enum SCENE_TYPE
{
    STARTUP,
    START_MENU,
    LOADING,
    LEVEL_SELECT,
    LEVEL,
    NEXT_LEVEL,
    RESTART_LEVEL,
    QUIT_GAME
}

public class Scene_Manager : MonoBehaviour
{
    [SerializeField] private UI_Manager m_UIManager;
    private GameManager m_CurrentGameManager;

    Coroutine m_LoadingCoroutine;

    private void Awake()
    {
        m_UIManager.LoadSceneOnButtonClicked += LoadScene;
        m_UIManager.LevelIndexToLoad += LoadLevel;
        m_UIManager.OnPauseWorld += CallPauseGame;

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }
    }

    private void OnDestroy()
    {
        m_UIManager.LoadSceneOnButtonClicked -= LoadScene;
        m_UIManager.LevelIndexToLoad -= LoadLevel;
        m_UIManager.OnPauseWorld -= CallPauseGame;
    }

    public void LoadScene(SCENE_TYPE scene)
    {
        switch (scene)
        {
            case SCENE_TYPE.QUIT_GAME:
                QuitApplication();
                break;
            case SCENE_TYPE.LEVEL:
                Debug.LogError("USE LoadLevel INSTEAD OF LOADSCENE");
                break;
            case SCENE_TYPE.NEXT_LEVEL:
                int BuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (BuildIndex >= SceneManager.sceneCountInBuildSettings)
                    BuildIndex = 1;
                Load(SceneManager.GetSceneByBuildIndex(BuildIndex).name);
                break;
            case SCENE_TYPE.RESTART_LEVEL:
                Load(SceneManager.GetActiveScene().name);
                break;
            default:
                Load(scene.ToString());
                break;
        }

        Load(scene.ToString());
    }

    private void LoadLevel(int levelIndex)
    {
        Load("level_" + levelIndex);
    }

    private void Load(string sceneName)
    {
        m_LoadingCoroutine = StartCoroutine(m_UIManager.LoadScreenFadeIn());

        if(m_CurrentGameManager)
        {
            m_CurrentGameManager.OnPauseWorld -= m_UIManager.PauseGame;
            m_CurrentGameManager.OnPlayerKilled -= m_UIManager.GameOver;
            m_CurrentGameManager.OnLevelComplete -= m_UIManager.GameWin;
        }
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene(), UnloadSceneOptions.None);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        StartCoroutine(m_UIManager.UpdateLoadScreen(loadOp));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);

        GameObject[] objects = scene.GetRootGameObjects();

        foreach (GameObject obj in objects)
        {
            UI_Abstract UI_Obj = obj.GetComponent<UI_Abstract>();
            if (UI_Obj)
            {
                m_UIManager.StartListeningForUI(UI_Obj);
            }
        }

        m_CurrentGameManager = FindObjectOfType<GameManager>();
        if (m_CurrentGameManager)
        {
            m_CurrentGameManager.OnPauseWorld += m_UIManager.PauseGame;
            m_CurrentGameManager.OnPlayerKilled += m_UIManager.GameOver;
            m_CurrentGameManager.OnLevelComplete += m_UIManager.GameWin;
        }

        if(m_LoadingCoroutine != null)
            StopCoroutine(m_LoadingCoroutine);

        m_LoadingCoroutine = StartCoroutine(m_UIManager.LoadScreenFadeOut());
    }

    private void CallPauseGame(bool paused)
    {
        if (m_CurrentGameManager)
            m_CurrentGameManager.TogglePauseGameObjects(paused);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
