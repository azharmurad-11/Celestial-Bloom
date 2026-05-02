using UnityEngine;

[CreateAssetMenu(menuName = "CelestialBloom/Crop", fileName = "NewCrop")]
public class CropDefinition : ScriptableObject
{
    [Header("ID & Name")]
    public string id;            // MUST be unique: "potato", "beetroot"
    public string displayName;

    [Header("Prices")]
    public int buyPrice = 30;    // seed cost
    public int sellPrice = 35;   // harvested crop sell price

    [Header("Sprites")]
    public Sprite seedSprite;    // used in shop + hotbar
    public Sprite cropSprite;    // used in selling UI or inventory
}
