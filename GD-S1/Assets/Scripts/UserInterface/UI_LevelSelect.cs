using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelSelect : UI_Abstract
{
    [SerializeField] private UI_OnClickLevelSelect[] m_LevelButtons;
    [SerializeField] private UI_OnClickButton m_BackButton;

    private void Awake()
    {
        for (int i = 0; i < m_LevelButtons.Length; i++)
        {
            m_LevelButtons[i].OnClicked += CallLoadLevel;
        }
        m_BackButton.OnClicked += BackButton;
    }

    private void OnDestroy()
    {
        m_BackButton.OnClicked -= BackButton;
    }

    private void CallLoadLevel(int levelNum)
    {
        LoadLevelByIndex?.Invoke(levelNum);
    }

    private void BackButton()
    {
        onBackButton?.Invoke();
    }
}
