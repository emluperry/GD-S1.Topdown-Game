using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelSelect : UI_SimpleScreen
{
    [SerializeField] private Button_UIOnClickLevel[] m_LevelButtons;

    protected override void Awake()
    {
        base.Awake();

        m_LevelButtons = GetComponentsInChildren<Button_UIOnClickLevel>();
        foreach(Button_UIOnClickLevel button in m_LevelButtons)
        {
            button.OnClicked += LoadLevel;
        }
    }

    protected override void OnDestroy()
    {
        foreach (Button_UIOnClickLevel button in m_LevelButtons)
        {
            button.OnClicked -= LoadLevel;
        }

        base.OnDestroy();
    }
}
