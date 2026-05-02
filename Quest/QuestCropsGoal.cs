using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestCropsGoal : MonoBehaviour
{
    public static QuestCropsGoal Instance { get; private set; }

    [Header("Goal")]
    public int targetAmount = 6;
    public int currentAmount = 0;

    [Header("UI")]
    public TMP_Text progressText;

    [Header("Credits")]
    public string creditsSceneName = "Credits";

    private bool isCompleted = false;
    private bool creditsTriggered = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void RegisterCropSold(int amount)
    {
        if (creditsTriggered) return;

        if (!isCompleted)
        {
            currentAmount += amount;
            if (currentAmount > targetAmount)
                currentAmount = targetAmount;

            if (currentAmount >= targetAmount)
                isCompleted = true;

            UpdateUI();

            if (SaveSystem.Instance != null)
                SaveSystem.Instance.SaveGame();
        }
    }

    // Call THIS when you OPEN the quest board
    public void OnQuestBoardOpened()
    {
        Debug.Log("[QuestCropsGoal] Quest board opened. Completed=" + isCompleted + " creditsTriggered=" + creditsTriggered);

        if (!isCompleted || creditsTriggered) return;

        creditsTriggered = true;

        if (SaveSystem.Instance != null)
            SaveSystem.Instance.SaveGame();

        if (string.IsNullOrEmpty(creditsSceneName))
        {
            Debug.LogError("[QuestCropsGoal] creditsSceneName is empty!");
            return;
        }

        Debug.Log("[QuestCropsGoal] Loading credits: " + creditsSceneName);
        SceneManager.LoadScene(creditsSceneName);
    }

    private void UpdateUI()
    {
        if (progressText != null)
            progressText.text = currentAmount + "/" + targetAmount;
    }

    // Save helpers
    public QuestSaveData GetSave()
    {
        return new QuestSaveData
        {
            currentAmount = currentAmount,
            isCompleted = isCompleted
        };
    }

    public void RestoreFromSave(QuestSaveData d)
    {
        if (d == null) return;

        currentAmount = d.currentAmount;
        isCompleted = d.isCompleted;

        // IMPORTANT: do NOT auto-trigger credits on load
        creditsTriggered = false;

        UpdateUI();
    }

    public bool IsCompleted() => isCompleted;
}

[System.Serializable]
public class QuestSaveData
{
    public int currentAmount;
    public bool isCompleted;
}
