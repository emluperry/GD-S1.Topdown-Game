using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelSelect : UI_Abstract
{
    [SerializeField] private UI_OnClickLevelSelect[] m_LevelButtons;

    private void Awake()
    {
        for (int i = 0; i < m_LevelButtons.Length; i++)
        {
            m_LevelButtons[i].OnClicked += CallLoadLevel;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < m_LevelButtons.Length; i++)
        {
            m_LevelButtons[i].OnClicked -= CallLoadLevel;
        }
    }

    private void CallLoadLevel(int levelNum)
    {
        LoadLevelByIndex?.Invoke(levelNum);
    }
}
