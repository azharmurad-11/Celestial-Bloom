using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellUIManager : MonoBehaviour
{
    [System.Serializable]
    public class SellCropConfig
    {
        public string cropName;
        public Sprite icon;
        public int sellPrice;
    }

    [System.Serializable]
    public class SellRowUI
    {
        public GameObject root;
        public Image iconImage;
        public TMP_Text nameText;
        public TMP_Text priceText;
        public Button sellButton;
    }

    public HotbarUI hotbar;
    public CurrencyManager currency;

    public SellCropConfig[] crops;
    public SellRowUI[] rows;

    private void Awake()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            int rowIndex = i;
            rows[i].sellButton.onClick.AddListener(() => OnSellClicked(rowIndex));
        }
    }

    private void OnEnable()
    {
        if (hotbar == null) hotbar = FindObjectOfType<HotbarUI>(true);
        if (currency == null) currency = CurrencyManager.Instance != null ? CurrencyManager.Instance : FindObjectOfType<CurrencyManager>(true);

        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < rows.Length; i++)
            SetupRow(i);
    }

    private void SetupRow(int index)
    {
        if (index >= crops.Length)
        {
            rows[index].root.SetActive(false);
            return;
        }

        var cfg = crops[index];
        var row = rows[index];

        row.root.SetActive(true);
        row.iconImage.sprite = cfg.icon;
        row.nameText.text = cfg.cropName;
        row.priceText.text = cfg.sellPrice.ToString();
    }

    private void OnSellClicked(int index)
    {
        if (hotbar == null || currency == null)
        {
            Debug.LogWarning("[SellUIManager] Missing hotbar or currency reference.");
            return;
        }

        SellCropConfig cfg = crops[index];

        for (int i = 0; i < hotbar.slots.Length; i++)
        {
            var slot = hotbar.slots[i];

            if (slot != null && !slot.IsEmpty && slot.CurrentIcon == cfg.icon)
            {
                slot.Clear();

                currency.AddPearls(cfg.sellPrice);

                if (QuestCropsGoal.Instance != null)
                    QuestCropsGoal.Instance.RegisterCropSold(1);

                Refresh();
                return;
            }
        }

        Debug.Log($"You don’t have any {cfg.cropName} to sell.");
    }
}
