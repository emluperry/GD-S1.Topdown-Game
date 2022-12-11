using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Healthbar : MonoBehaviour
{
    private Image m_barSegment;

    private void Awake()
    {
        m_barSegment = GetComponent<Image>();
    }

    public void UpdateHealth(float dec)
    {
        m_barSegment.fillAmount = dec;
    }
}
