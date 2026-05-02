using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CropSellUIItem : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text sellPriceText;

    public CropDefinition crop;

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (crop == null) return;

        iconImage.sprite = crop.cropSprite;
        nameText.text = crop.displayName;
        sellPriceText.text = crop.sellPrice.ToString();
    }
}
