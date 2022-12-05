using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoseScreen : UI_Abstract
{
    [SerializeField] private UI_OnClickButton m_RestartButton;
    [SerializeField] private UI_OnClickButton m_QuitToTitleButton;

    private void Awake()
    {
        m_RestartButton.OnClicked += RestartLevel;
        m_QuitToTitleButton.OnClicked += QuitToTitle;
    }

    private void OnDestroy()
    {
        m_RestartButton.OnClicked -= RestartLevel;
        m_QuitToTitleButton.OnClicked -= QuitToTitle;
    }

    private void RestartLevel()
    {
        LoadScene?.Invoke(SCENE_TYPE.RESTART_LEVEL);
    }

    private void QuitToTitle()
    {
        LoadScene?.Invoke(SCENE_TYPE.QUIT_GAME);
    }
}
