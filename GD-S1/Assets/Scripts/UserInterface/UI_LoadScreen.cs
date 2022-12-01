using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_LoadText;

    public void UpdatePercent(float value)
    {
        m_LoadText.text = value.ToString();
    }
}
