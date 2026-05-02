using UnityEngine;

public class CropSellBox : MonoBehaviour
{
    [Header("References")]
    public HotbarUI hotbar;
    public Transform player;
    public float interactDistance = 3f;

    [Header("UI")]
    public GameObject sellPrompt;  // "Press F to sell"

    private void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (sellPrompt != null)
            sellPrompt.SetActive(false);
    }

    private void Update()
    {
        if (player == null || hotbar == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        bool closeEnough = dist <= interactDistance;

        if (sellPrompt != null)
            sellPrompt.SetActive(closeEnough);

        if (closeEnough && Input.GetKeyDown(KeyCode.F))
        {
            var slot = hotbar.GetSelectedSlot();
            if (slot == null || slot.IsEmpty || slot.CurrentCrop == null)
            {
                Debug.Log("[SellBox] No crop selected to sell.");
                return;
            }

            if (CurrencyManager.Instance == null)
            {
                Debug.LogError("[SellBox] CurrencyManager.Instance is NULL – no money system found in scene.");
                return;
            }

            CropDefinition crop = slot.CurrentCrop;

            bool consumed = slot.TryConsume(1);
            if (!consumed)
            {
                Debug.Log("[SellBox] Not enough items in slot to sell.");
                return;
            }

            CurrencyManager.Instance.AddPearls(crop.sellPrice);
            Debug.Log($"[SellBox] Sold 1x {crop.displayName} for {crop.sellPrice} pearls.");
        }
    }
}
