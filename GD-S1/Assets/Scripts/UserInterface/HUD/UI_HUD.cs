using Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UI_HUD : UI_Abstract
{
    [SerializeField] private UI_SegmentBar m_Healthbar;
    [SerializeField] private UI_SegmentBar m_Magicbar;

    [SerializeField] private UI_SegmentBar m_Timerbar;
    [SerializeField] private Image m_FlameDecor;
    [SerializeField] private Image[] m_AbilityIcons;
    private int m_CurrentAbilityIcon = 1;
    [SerializeField] private Image[] m_AbilityPips;
    [SerializeField] private Color[] m_AbilityColours;

    [SerializeField] private TextMeshProUGUI m_CoinValueText;
    private int m_CoinNum = 0;
    [SerializeField] private TextMeshProUGUI m_KeyValueText;
    private int m_KeyNum = 0;
    [SerializeField] private Image m_BossKey;

    public void UpdateBar(SEGMENT_TYPE type, float value)
    {
        switch(type)
        {
            case SEGMENT_TYPE.HEALTH:
                m_Healthbar.UpdateValue(value);
                break;

            case SEGMENT_TYPE.MAGIC:
                m_Magicbar.UpdateValue(value);
                break;

            case SEGMENT_TYPE.ABILITY_TIMER:
                m_Timerbar.UpdateValue(value);
                break;
        }
    }

    public void UpdateValue(COLLECTABLE_TYPE type, int increment)
    {
        switch(type)
        {
            case COLLECTABLE_TYPE.COIN:
                m_CoinNum += increment;
                m_CoinValueText.text = m_CoinNum.ToString();
                break;

            case COLLECTABLE_TYPE.KEY:
                m_KeyNum += increment;
                m_KeyValueText.text = m_KeyNum.ToString();
                break;

            case COLLECTABLE_TYPE.BOSS_KEY:
                if (increment == 0)
                    m_BossKey.enabled = false;
                else
                    m_BossKey.enabled = true;
                break;
        }
    }

    public void UpdateAffinityType(AFFINITY_TYPE type)
    {
        m_AbilityIcons[m_CurrentAbilityIcon].enabled = false;
        m_CurrentAbilityIcon = (int)type;

        m_AbilityIcons[m_CurrentAbilityIcon].enabled = true;
        m_Timerbar.GetComponent<Image>().color = m_AbilityColours[m_CurrentAbilityIcon];

        int next = m_CurrentAbilityIcon + 1;
        if (next > 3)
            next = 1;
        m_AbilityPips[0].color = m_AbilityColours[next];

        next++;
        if (next > 3)
            next = 1;
        m_AbilityPips[1].color = m_AbilityColours[next];
    }

    public void SetAffinityActive(bool wasSet)
    {
        m_FlameDecor.enabled = wasSet;
    }
}
