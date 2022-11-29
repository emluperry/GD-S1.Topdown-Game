using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE_TYPE
{
    START,
    LEVEL_SELECT,
    LEVEL,
    QUIT_GAME
}

public class Scene_Manager : MonoBehaviour
{
    public void LoadScene(SCENE_TYPE scene)
    {
        if(scene == SCENE_TYPE.QUIT_GAME)
        {
            QuitApplication();
        }
        else
        {
            //SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Single);
        }
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
