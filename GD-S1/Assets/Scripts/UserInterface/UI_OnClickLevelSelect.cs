using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UI_OnClickLevelSelect : UI_OnClickButton
{
    [SerializeField] private int m_LevelNum;

    new public event Action<int> OnClicked;

    public override void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(m_LevelNum);
    }
}
