using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE_TYPE
{
    STARTUP,
    START_MENU,
    LOADING,
    LEVEL_SELECT,
    LEVEL,
    NEXT_LEVEL,
    QUIT_GAME
}

public class Scene_Manager : MonoBehaviour
{
    [SerializeField] private UI_Manager m_UIManager;
    [SerializeField] private GameObject m_LoadScreen;
    private GameManager m_CurrentGameManager;

    private SCENE_TYPE m_CurrentScene = SCENE_TYPE.STARTUP;

    private void Awake()
    {
        m_UIManager.LoadSceneOnButtonClicked += LoadScene;
        m_UIManager.LevelIndexToLoad += LoadLevel;

        if(m_CurrentScene == SCENE_TYPE.STARTUP)
        {
            LoadScene(SCENE_TYPE.START_MENU);
        }
    }

    private void OnDestroy()
    {
        m_UIManager.LoadSceneOnButtonClicked -= LoadScene;
        m_UIManager.LevelIndexToLoad -= LoadLevel;
    }

    public void LoadScene(SCENE_TYPE scene)
    {
        if(scene == SCENE_TYPE.QUIT_GAME)
        {
            QuitApplication();
            return;
        }
        else if(scene == SCENE_TYPE.LEVEL)
        {
            Debug.LogError("Load levels with LoadLevel");
        }

        Load(scene.ToString());
        m_CurrentScene = scene;
    }

    private void LoadLevel(int levelIndex)
    {
        Load("level_" + levelIndex);
        m_CurrentScene = SCENE_TYPE.LEVEL;
    }

    private void Load(string sceneName)
    {
        if(m_CurrentGameManager)
        {
            m_CurrentGameManager.onPauseWorld -= m_UIManager.PauseGame;
        }
        AsyncOperation deloadOp = SceneManager.UnloadSceneAsync(m_CurrentScene.ToString(), UnloadSceneOptions.None);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadOp.completed += (loadOp) =>
        {
            Scene LoadedScene = SceneManager.GetSceneByName(sceneName);
            GameObject[] objects = LoadedScene.GetRootGameObjects();

            foreach (GameObject obj in objects)
            {
                UI_Abstract UI_Obj = obj.GetComponent<UI_Abstract>();
                if (UI_Obj)
                {
                    m_UIManager.StartListeningForUI(UI_Obj);
                }

                if(obj.GetComponent<GameManager>())
                {
                    m_CurrentGameManager = obj.GetComponent<GameManager>();
                    m_CurrentGameManager.onPauseWorld += m_UIManager.PauseGame;
                }
            }
        };

        StartCoroutine(LoadingScreen(loadOp));
    }

    private IEnumerator LoadingScreen(AsyncOperation loading)
    {
        UI_LoadScreen LoadObject = Instantiate(m_LoadScreen, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_LoadScreen>();

        while(!loading.isDone)
        {
            yield return new WaitForEndOfFrame();
            LoadObject.UpdatePercent(loading.progress * 50);
        }

        Destroy(LoadObject.gameObject);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
