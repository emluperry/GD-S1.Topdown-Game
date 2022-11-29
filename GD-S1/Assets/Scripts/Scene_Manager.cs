using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SCENE_TYPE
{
    START,
    LEVEL_SELECT,
    LEVEL,
    QUIT_GAME
}

public class Scene_Manager : MonoBehaviour
{
    private void QuitApplication()
    {
        Application.Quit();
    }
}
