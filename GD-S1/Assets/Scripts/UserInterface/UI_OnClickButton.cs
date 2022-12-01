using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_OnClickButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action OnClicked;

    private Image m_Image;

    [SerializeField] private Color m_HoverColour = Color.white;

    private void Start()
    {
        m_Image = GetComponent<Image>();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Image.color = m_HoverColour;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Image.color = Color.white;
    }
}
