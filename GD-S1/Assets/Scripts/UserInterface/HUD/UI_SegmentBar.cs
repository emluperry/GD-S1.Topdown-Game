using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SegmentBar : MonoBehaviour
{
    private Image m_barSegment;

    private void Awake()
    {
        m_barSegment = GetComponent<Image>();
    }

    public void UpdateValue(float dec)
    {
        m_barSegment.fillAmount = dec;
    }
}
