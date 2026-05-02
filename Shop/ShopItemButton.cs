using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [Header("Item Data")]
    public Sprite itemIcon;   // what icon goes into the hotbar

    [Header("References")]
    public HotbarUI hotbar;   // drag the HotbarPanel here

    private void Awake()
    {
        // Auto-find hotbar if not set
        if (hotbar == null)
        {
            hotbar = FindObjectOfType<HotbarUI>();
            if (hotbar == null)
            {
                Debug.LogError("[ShopItemButton] No HotbarUI found in scene!");
            }
        }

        if (itemIcon == null)
        {
            Debug.LogWarning("[ShopItemButton] itemIcon is NOT set on " + gameObject.name);
        }
    }

    public void BuyItem()
    {
        if (hotbar == null)
        {
            Debug.LogError("[ShopItemButton] Cannot buy item, hotbar is null!");
            return;
        }

        if (itemIcon == null)
        {
            Debug.LogError("[ShopItemButton] Cannot buy item, itemIcon is null on " + gameObject.name);
            return;
        }

        int index = hotbar.selectedIndex;

        Debug.Log("[ShopItemButton] Trying to place item in selected slot index: " + index);

        // Safety checks
        if (hotbar.slots == null || hotbar.slots.Length == 0)
        {
            Debug.LogError("[ShopItemButton] Hotbar slots array is null or empty!");
            return;
        }

        if (index < 0 || index >= hotbar.slots.Length)
        {
            Debug.LogError("[ShopItemButton] selectedIndex out of range: " + index);
            return;
        }

        if (hotbar.slots[index] == null)
        {
            Debug.LogError("[ShopItemButton] Slot at index " + index + " is NULL in the array!");
            return;
        }

        // Put the item in the selected slot
        hotbar.SetSlotItem(index, itemIcon, 1);
        Debug.Log("[ShopItemButton] Bought item and placed in slot " + index);
    }
}
