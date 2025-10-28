using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Text targetText; // Assign your UI Text here

    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color pressedColor = Color.red;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        // If no text assigned, try to find one automatically
        if (targetText == null)
            targetText = GetComponentInChildren<Text>();

        if (targetText != null)
            targetText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
            targetText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
            targetText.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
            targetText.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable)
            targetText.color = hoverColor;
    }
}
