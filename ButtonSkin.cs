using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSkin : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Hook These")]
    public Image lightImage;     // your light background image
    public Image darkImage;      // your dark background image

    [Header("Settings")]
    public bool startLight = true;

    void Start()
    {
        // default state
        ShowLight(startLight);
    }

    void ShowLight(bool show)
    {
        if (lightImage != null) lightImage.enabled = show;
        if (darkImage != null) darkImage.enabled = !show;
    }

    // Hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowLight(false); // show dark on hover
    }

    // Unhover
    public void OnPointerExit(PointerEventData eventData)
    {
        ShowLight(true); // back to light when not hovered
    }

    // Click
    public void OnPointerClick(PointerEventData eventData)
    {
        // flash dark on click
        ShowLight(false);
    }
}
