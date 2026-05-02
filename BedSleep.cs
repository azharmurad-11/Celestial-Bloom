using UnityEngine;
using System.Collections;
using Tenkoku.Core;

public class BedSleepWithTenkoku : MonoBehaviour
{
    [Header("Tenkoku")]
    public TenkokuModule tenkokuModule;

    [Header("Player")]
    public PlayerThirdPersonMovement playerController;

    [Header("UI Prompts")]
    public GameObject sleepPrompt;
    public GameObject onlyAtNightMessage;

    [Header("Fade UI (GameObject Canvas)")]
    public GameObject fadeCanvas;
    public float fadeSpeed = 2f;

    [Header("Sleep Settings")]
    public float sleepDelay = 1f;
    public int nightStartHour = 20;
    public int nightEndHour = 6;
    public int morningHour = 8;

    private bool playerInRange = false;
    private bool isSleeping = false;

    private CanvasGroup fadeGroup;

    private void Start()
    {
        if (sleepPrompt != null) sleepPrompt.SetActive(false);
        if (onlyAtNightMessage != null) onlyAtNightMessage.SetActive(false);

        if (fadeCanvas != null)
        {
            fadeGroup = fadeCanvas.GetComponent<CanvasGroup>();
            if (fadeGroup == null) fadeGroup = fadeCanvas.AddComponent<CanvasGroup>();

            fadeCanvas.SetActive(true);
            fadeGroup.alpha = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (sleepPrompt != null) sleepPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (sleepPrompt != null) sleepPrompt.SetActive(false);
            if (onlyAtNightMessage != null) onlyAtNightMessage.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange || isSleeping) return;

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (IsNight()) StartCoroutine(SleepRoutine());
            else StartCoroutine(ShowOnlyAtNightMessage());
        }
    }

    private IEnumerator SleepRoutine()
    {
        isSleeping = true;

        if (playerController != null) playerController.enabled = false;

        yield return StartCoroutine(Fade(1f));
        yield return new WaitForSeconds(sleepDelay);

        SetTenkokuTime(morningHour);

        if (FarmingDayManager.Instance != null)
            FarmingDayManager.Instance.AdvanceDay();

        if (SaveSystem.Instance != null)
            SaveSystem.Instance.SaveGame();

        yield return StartCoroutine(Fade(0f));

        if (playerController != null) playerController.enabled = true;

        isSleeping = false;
    }

    private IEnumerator ShowOnlyAtNightMessage()
    {
        if (onlyAtNightMessage == null) yield break;
        onlyAtNightMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        onlyAtNightMessage.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeGroup == null) yield break;

        float startAlpha = fadeGroup.alpha;
        float t = 0f;

        while (!Mathf.Approximately(fadeGroup.alpha, targetAlpha))
        {
            t += Time.deltaTime * fadeSpeed;
            fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }
    }

    private bool IsNight()
    {
        int hour = tenkokuModule.currentHour;
        return (hour >= nightStartHour || hour < nightEndHour);
    }

    private void SetTenkokuTime(int hour)
    {
        tenkokuModule.currentHour = hour;
        tenkokuModule.currentMinute = 0;
        tenkokuModule.currentSecond = 0;
    }
}
