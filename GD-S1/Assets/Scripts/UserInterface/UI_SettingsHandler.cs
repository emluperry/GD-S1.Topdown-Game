using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SettingsHandler : UI_Abstract
{
    [SerializeField] private UI_OnClickButton m_BackButton;

    private void OnEnable()
    {
        m_BackButton.OnClicked += CallBackButton;
    }

    private void OnDisable()
    {
        m_BackButton.OnClicked -= CallBackButton;
    }

    private void CallBackButton()
    {
        onBackButton?.Invoke();
    }
}
