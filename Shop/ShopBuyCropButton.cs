using UnityEngine;

public class ShopBuyCropButton : MonoBehaviour
{
    [Header("What crop does this button buy?")]
    public CropDefinition crop;     // PotatoCrop
    public HotbarUI hotbar;         // reference to your HotbarUI

    public void OnBuyButtonClicked()
    {
        if (crop == null)
        {
            Debug.LogWarning("ShopBuyCropButton: No CropDefinition assigned!");
            return;
        }

        if (hotbar == null)
        {
            Debug.LogWarning("ShopBuyCropButton: No HotbarUI assigned!");
            return;
        }

        // 1) Spend pearls
        if (!CurrencyManager.Instance.SpendPearls(crop.buyPrice))
        {
            Debug.Log("Not enough pearls to buy " + crop.displayName);
            return;
        }

        // 2) Add seed to hotbar (stack if same crop already there)
        bool added = hotbar.AddCrop(crop, 1);
        if (!added)
        {
            Debug.Log("Hotbar full, could not add " + crop.displayName);
            // (Optional) refund pearls if you want:
            // CurrencyManager.Instance.AddPearls(crop.buyPrice);
            return;
        }

        Debug.Log("Bought 1x " + crop.displayName);

        // 3) Tutorial: move from "BuyCrops" -> "PlantCrop"
        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnBoughtCrops();
        }
    }
}
