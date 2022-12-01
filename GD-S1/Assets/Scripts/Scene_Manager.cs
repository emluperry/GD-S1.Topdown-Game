using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE_TYPE
{
    STARTUP,
    START_MENU,
    LEVEL_SELECT,
    LEVEL,
    QUIT_GAME
}

public class Scene_Manager : MonoBehaviour
{
    [SerializeField] private UI_Manager m_UIManager;

    private SCENE_TYPE m_CurrentScene = SCENE_TYPE.STARTUP;

    private void Awake()
    {
        m_UIManager.LoadSceneOnButtonClicked += LoadScene;

        if(m_CurrentScene == SCENE_TYPE.STARTUP)
        {
            LoadScene(SCENE_TYPE.START_MENU);
        }
    }

    private void OnDestroy()
    {
        m_UIManager.LoadSceneOnButtonClicked -= LoadScene;
    }

    public void LoadScene(SCENE_TYPE scene)
    {
        if(scene == SCENE_TYPE.QUIT_GAME)
        {
            QuitApplication();
        }
        else
        {
            //load loading scene - additive
            //unload old scene
            //load new scene
            AsyncOperation operation = SceneManager.LoadSceneAsync(scene.ToString().ToLower(), LoadSceneMode.Additive);
            operation.completed += (op) =>
            {
                Scene LoadedScene = SceneManager.GetSceneByName(scene.ToString());
                GameObject[] objects = LoadedScene.GetRootGameObjects();
                foreach(GameObject obj in objects)
                {
                    UI_Abstract UI_Obj = obj.GetComponent<UI_Abstract>();
                    if(UI_Obj)
                    {
                        m_UIManager.StartListeningForUI(UI_Obj);
                        return;
                    }
                }
            };
        }
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
