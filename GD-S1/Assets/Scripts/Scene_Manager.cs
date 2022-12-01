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
    QUIT_GAME
}

public class Scene_Manager : MonoBehaviour
{
    [SerializeField] private UI_Manager m_UIManager;
    [SerializeField] private GameObject m_LoadScreen;

    private SCENE_TYPE m_CurrentScene = SCENE_TYPE.STARTUP;

    private void Awake()
    {
        m_UIManager.LoadSceneOnButtonClicked += LoadScene;
        //m_UIManager.LevelIndexToLoad += FunctionA;

        if(m_CurrentScene == SCENE_TYPE.STARTUP)
        {
            LoadScene(SCENE_TYPE.START_MENU);
        }
    }

    private void OnDestroy()
    {
        m_UIManager.LoadSceneOnButtonClicked -= LoadScene;
        //m_UIManager.LevelIndexToLoad -= FunctionA;
    }

    public void LoadScene(SCENE_TYPE scene)
    {
        if(scene == SCENE_TYPE.QUIT_GAME)
        {
            QuitApplication();
        }
        else
        {
            AsyncOperation deloadOp = SceneManager.UnloadSceneAsync(m_CurrentScene.ToString(), UnloadSceneOptions.None);

            AsyncOperation loadOp = SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);
            loadOp.completed += (loadOp) =>
            {
                Scene LoadedScene = SceneManager.GetSceneByName(scene.ToString());
                GameObject[] objects = LoadedScene.GetRootGameObjects();
                foreach(GameObject obj in objects)
                {
                    UI_Abstract UI_Obj = obj.GetComponent<UI_Abstract>();
                    if(UI_Obj)
                    {
                        m_UIManager.StartListeningForUI(UI_Obj);
                        break;
                    }
                }

                m_CurrentScene = scene;
            };

            StartCoroutine(LoadingScreen(deloadOp, loadOp));
        }
    }

    private IEnumerator LoadingScreen(AsyncOperation deload, AsyncOperation loading)
    {
        UI_LoadScreen LoadObject = Instantiate(m_LoadScreen, Vector3.zero, Quaternion.identity, transform).GetComponent<UI_LoadScreen>();

        while(!(deload.isDone && loading.isDone))
        {
            yield return new WaitForEndOfFrame();
            LoadObject.UpdatePercent((deload.progress + loading.progress) * 50);
        }

        Destroy(LoadObject.gameObject);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
