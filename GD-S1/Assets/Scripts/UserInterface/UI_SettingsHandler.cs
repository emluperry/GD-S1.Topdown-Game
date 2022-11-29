using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SettingsHandler : MonoBehaviour
{
    [SerializeField] private UI_OnClickButton m_BackButton;

    public Action onBackButton;

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
