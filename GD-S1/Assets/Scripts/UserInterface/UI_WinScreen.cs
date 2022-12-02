using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WinScreen : UI_Abstract
{
    [SerializeField] private UI_OnClickButton m_NextLevelButton;
    [SerializeField] private UI_OnClickButton m_QuitToTitleButton;

    private void Awake()
    {
        m_NextLevelButton.OnClicked += LoadNextLevel;
        m_QuitToTitleButton.OnClicked += QuitToTitle;
    }

    private void LoadNextLevel()
    {
        LoadScene?.Invoke(SCENE_TYPE.NEXT_LEVEL);
    }

    private void QuitToTitle()
    {
        LoadScene?.Invoke(SCENE_TYPE.QUIT_GAME);
    }
}
