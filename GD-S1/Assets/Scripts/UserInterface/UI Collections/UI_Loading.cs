using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Loading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_LoadText;
    private CanvasGroup m_CanvasGroup;

    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    public IEnumerator FadeIn(float FadeInOutTime)
    {
        float Increment = 1 / FadeInOutTime;
        m_CanvasGroup.blocksRaycasts = true;
        m_CanvasGroup.alpha = 0;
        while (m_CanvasGroup.alpha < 1)
        {
            m_CanvasGroup.alpha += Increment;
            yield return new WaitForFixedUpdate();
        }
        m_CanvasGroup.alpha = 1;
    }

    public IEnumerator UpdateLoadText(AsyncOperation loadOperation)
    {
        m_LoadText.text = "0%";
        while(!loadOperation.isDone)
        {
            m_LoadText.text = (loadOperation.progress * 100).ToString() + "%";
            yield return new WaitForFixedUpdate();
        }
        m_LoadText.text = "100%";
    }

    public IEnumerator FadeOut(float FadeInOutTime)
    {
        float Increment = 1 / FadeInOutTime;
        m_CanvasGroup.alpha = 1;
        while (m_CanvasGroup.alpha < 1)
        {
            m_CanvasGroup.alpha -= Increment;
            yield return new WaitForFixedUpdate();
        }
        m_CanvasGroup.alpha = 0;
        m_CanvasGroup.blocksRaycasts = false;
    }
}
