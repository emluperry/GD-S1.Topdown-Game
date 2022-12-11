using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Glossary : UI_SimpleScreen
{
    [SerializeField] private Button_UIOnClickUI m_TutorialButton;
    [SerializeField] private Button_UIOnClickUI m_BestiaryButton;

    protected override void Awake()
    {
        base.Awake();

        m_TutorialButton.OnClicked += LoadUI;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        m_TutorialButton.OnClicked -= LoadUI;
    }
}
