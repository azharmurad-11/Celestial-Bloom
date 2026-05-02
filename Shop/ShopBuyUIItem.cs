using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopBuyUIItem : MonoBehaviour
{
    public Image iconImage;         // UI image in the shop buy panel
    public TMP_Text nameText;
    public TMP_Text priceText;

    [Header("Assigned Asset")]
    public CropDefinition crop;     // drag the crop asset here

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (crop == null) return;

        iconImage.sprite = crop.seedSprite;
        nameText.text = crop.displayName;
        priceText.text = crop.buyPrice.ToString();
    }
}
