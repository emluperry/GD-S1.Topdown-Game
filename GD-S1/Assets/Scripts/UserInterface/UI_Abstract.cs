using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Abstract : MonoBehaviour
{
    public Action<SCENE_TYPE> LoadScene;
    public Action<int> LoadLevelByIndex;
    public Action<UI_SCREENS> LoadUI;
    public Action onBackButton;

    [Header("OPTIONAL common buttons")]
    [SerializeField] private UI_OnClickButton m_BackButton;

    private void OnEnable()
    {
        if(m_BackButton)
            m_BackButton.OnClicked += CallBackButton;
    }

    private void OnDisable()
    {
        if (m_BackButton)
            m_BackButton.OnClicked -= CallBackButton;
    }

    private void CallBackButton()
    {
        onBackButton?.Invoke();
    }
}
