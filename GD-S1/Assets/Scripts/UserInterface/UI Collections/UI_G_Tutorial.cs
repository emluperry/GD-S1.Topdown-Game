using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_G_Tutorial : UI_SimpleScreen
{
    [SerializeField] private Button_UIOnClick m_ControlsButton;
    [SerializeField] private Button_UIOnClick m_MagicButton;
    [SerializeField] private Button_UIOnClick m_GoalButton;

    [SerializeField] private GameObject m_ControlsText;
    [SerializeField] private GameObject m_MagicText;
    [SerializeField] private GameObject m_GoalText;

    protected override void Awake()
    {
        base.Awake();

        m_ControlsButton.OnClicked += LoadControls;
        m_MagicButton.OnClicked += LoadMagic;
        m_GoalButton.OnClicked += LoadGoal;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        m_ControlsButton.OnClicked -= LoadControls;
        m_MagicButton.OnClicked -= LoadMagic;
        m_GoalButton.OnClicked -= LoadGoal;
    }

    private void LoadControls()
    {
        m_ControlsText.SetActive(true);
        m_MagicText.SetActive(false);
        m_GoalText.SetActive(false);
    }

    private void LoadMagic()
    {
        m_ControlsText.SetActive(false);
        m_MagicText.SetActive(true);
        m_GoalText.SetActive(false);
    }

    private void LoadGoal()
    {
        m_ControlsText.SetActive(false);
        m_MagicText.SetActive(false);
        m_GoalText.SetActive(true);
    }
}
