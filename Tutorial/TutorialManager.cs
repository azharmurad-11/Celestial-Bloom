using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public enum TutorialStep
    {
        None,
        FollowStarToShop,
        BuyCrops,
        GoToQuestBoard,
        PlantCrop,
        WaterCrop,
        Finished
    }

    [Header("References")]
    public FloatingStarPath star;
    public Transform player;

    [Header("Path Points")]
    public List<Transform> pathToShop;
    public List<Transform> pathToQuestBoard;

    [Header("Target Positions (For Distance Check Only)")]
    public Transform cropShopPoint;
    public Transform questBoardPoint;

    [Header("UI (TMP)")]
    public TMP_Text tutorialText;

    [Header("Distances")]
    public float reachedShopDistance = 2f;
    public float reachedQuestBoardDistance = 2f;

    public TutorialStep CurrentStep { get; private set; } = TutorialStep.None;

    // UI suppression
    private bool externalPromptActive = false;
    private bool wasTutorialVisibleBeforeExternalPrompt = false;

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
        // Start tutorial ONLY if no save exists
        if (SaveSystem.Instance == null || !SaveSystem.HasSaveFile())
        {
            BeginTutorial();
            SaveNow();
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (CurrentStep == TutorialStep.FollowStarToShop && cropShopPoint != null)
        {
            if (Vector3.Distance(player.position, cropShopPoint.position) <= reachedShopDistance)
                OnReachedCropShop();
        }
        else if (CurrentStep == TutorialStep.GoToQuestBoard && questBoardPoint != null)
        {
            if (Vector3.Distance(player.position, questBoardPoint.position) <= reachedQuestBoardDistance)
                OnReachedQuestBoard();
        }
    }

    // ==============================
    // REQUIRED BY FloatingStarPath
    // ==============================
    public void OnStarReachedTarget()
    {
        // Intentionally empty.
        // Distance-based logic handles progression.
    }

    // ==============================
    // LOAD FROM SAVE
    // ==============================
    public void LoadFromSave(TutorialStep step)
    {
        if (step == TutorialStep.Finished)
        {
            CurrentStep = TutorialStep.Finished;
            if (star != null) star.gameObject.SetActive(false);
            ClearTutorialText();
            return;
        }

        CurrentStep = step;

        if (star != null)
            star.player = player;

        switch (CurrentStep)
        {
            case TutorialStep.FollowStarToShop:
                BeginTutorial();
                break;

            case TutorialStep.BuyCrops:
                SetText("Buy some crop seeds from the shop.");
                if (star != null) star.gameObject.SetActive(true);
                break;

            case TutorialStep.GoToQuestBoard:
                SetText("Great! Now follow the star to the quest board.");
                if (star != null)
                {
                    star.gameObject.SetActive(true);
                    star.FollowPath(pathToQuestBoard);
                }
                break;

            case TutorialStep.PlantCrop:
                if (star != null) star.gameObject.SetActive(false);
                SetText("Now go to your field and plant a crop by pressing E.");
                break;

            case TutorialStep.WaterCrop:
                if (star != null) star.gameObject.SetActive(false);
                SetText("Nice! Now water the crop by pressing T.");
                break;
        }
    }

    // ==============================
    // FLOW
    // ==============================
    private void BeginTutorial()
    {
        CurrentStep = TutorialStep.FollowStarToShop;
        SetText("Follow the star to buy your first crops.");

        if (star != null)
        {
            star.gameObject.SetActive(true);
            star.player = player;
            star.SnapToPlayer();
            star.FollowPath(pathToShop);
        }
    }

    private void OnReachedCropShop()
    {
        if (CurrentStep != TutorialStep.FollowStarToShop) return;

        CurrentStep = TutorialStep.BuyCrops;
        SetText("Buy some crop seeds from the shop.");
        SaveNow();
    }

    public void OnBoughtCrops()
    {
        if (CurrentStep != TutorialStep.BuyCrops) return;

        CurrentStep = TutorialStep.GoToQuestBoard;
        SetText("Great! Now follow the star to the quest board.");

        if (star != null)
            star.FollowPath(pathToQuestBoard);

        SaveNow();
    }

    private void OnReachedQuestBoard()
    {
        if (CurrentStep != TutorialStep.GoToQuestBoard) return;

        CurrentStep = TutorialStep.PlantCrop;
        if (star != null) star.gameObject.SetActive(false);
        SetText("Now go to your field and plant a crop by pressing E.");
        SaveNow();
    }

    public void OnCropPlanted()
    {
        if (CurrentStep != TutorialStep.PlantCrop) return;

        CurrentStep = TutorialStep.WaterCrop;
        SetText("Nice! Now water the crop by pressing T.");
        SaveNow();
    }

    public void OnCropWatered()
    {
        if (CurrentStep != TutorialStep.WaterCrop) return;

        CurrentStep = TutorialStep.Finished;
        ClearTutorialText();
        if (star != null) star.gameObject.SetActive(false);
        SaveNow();
    }

    // ==============================
    // UI
    // ==============================
    private void SetText(string msg)
    {
        if (tutorialText == null) return;

        tutorialText.text = msg;
        tutorialText.gameObject.SetActive(
            !string.IsNullOrEmpty(msg) &&
            CurrentStep != TutorialStep.Finished &&
            !externalPromptActive
        );
    }

    private void ClearTutorialText()
    {
        if (tutorialText == null) return;
        tutorialText.text = "";
        tutorialText.gameObject.SetActive(false);
    }

    public void OnExternalPromptShown()
    {
        if (tutorialText == null) return;
        externalPromptActive = true;
        wasTutorialVisibleBeforeExternalPrompt = tutorialText.gameObject.activeSelf;
        tutorialText.gameObject.SetActive(false);
    }

    public void OnExternalPromptHidden()
    {
        if (!externalPromptActive) return;
        externalPromptActive = false;

        if (CurrentStep != TutorialStep.Finished && wasTutorialVisibleBeforeExternalPrompt)
            tutorialText.gameObject.SetActive(true);
    }

    private void SaveNow()
    {
        if (SaveSystem.Instance != null)
            SaveSystem.Instance.SaveGame();
    }
}
