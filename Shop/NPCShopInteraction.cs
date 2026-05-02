using UnityEngine;

public class NPCShopInteract : MonoBehaviour
{
    [Header("Settings")]
    public string npcName = "Valerie";

    [Header("Player")]
    public Transform playerTransform;
    public float interactDistance = 3f;

    [Header("UI References")]
    public GameObject interactPrompt;   // "Press F to talk"
    public GameObject dialogueUI;       // panel: "Hey, how can I help you? [Shop] [Sell]"
    public GameObject shopUI;           // full shop menu (buy)
    public GameObject sellUI;           // sell menu (we’ll hook later)

    [Header("Camera Control")]
    // Drag your camera-look script here (the one that rotates the camera)
    public MonoBehaviour cameraLookScript;

    [Header("Debug")]
    public bool dialogueOpen = false;
    public bool shopOpen = false;
    public bool sellOpen = false;

    private void Start()
    {
        Debug.Log($"[{npcName}] Script started on: {gameObject.name}");

        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }

        if (interactPrompt != null)
            interactPrompt.SetActive(false);
        else
            Debug.LogWarning($"[{npcName}] Interact prompt NOT assigned!");

        if (dialogueUI != null)
            dialogueUI.SetActive(false);
        else
            Debug.LogWarning($"[{npcName}] Dialogue UI NOT assigned!");

        if (shopUI != null)
            shopUI.SetActive(false);
        else
            Debug.LogWarning($"[{npcName}] Shop UI NOT assigned!");

        if (sellUI != null)
            sellUI.SetActive(false);   // can be null if you haven’t made it yet
    }

    private void Update()
    {
        if (playerTransform == null)
            return;

        float dist = Vector3.Distance(playerTransform.position, transform.position);
        bool closeEnough = dist <= interactDistance;

        bool anyUIOpen = dialogueOpen || shopOpen || sellOpen;

        // Show "Press F" only when close and no UI open
        if (interactPrompt != null)
            interactPrompt.SetActive(closeEnough && !anyUIOpen);

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"[{npcName}] F pressed. dist={dist:F2}, closeEnough={closeEnough}, anyUIOpen={anyUIOpen}");

            if (anyUIOpen)
            {
                // F closes everything if something is open
                CloseAll();
            }
            else if (closeEnough)
            {
                OpenDialogue();
            }
        }
    }

    // =========================
    // Main open/close helpers
    // =========================

    private void OpenDialogue()
    {
        if (dialogueUI == null)
        {
            Debug.LogWarning($"[{npcName}] Cannot open dialogue, dialogueUI is NULL!");
            return;
        }

        dialogueUI.SetActive(true);
        dialogueOpen = true;
        shopOpen = false;
        sellOpen = false;

        // Unlock cursor for UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Stop camera look
        if (cameraLookScript != null)
            cameraLookScript.enabled = false;

        // Hide tutorial text (follow the star) while NPC UI is open
        if (TutorialManager.Instance != null)
            TutorialManager.Instance.OnExternalPromptShown();

        Debug.Log($"[{npcName}] Dialogue OPENED.");
    }

    private void OpenShop()
    {
        if (shopUI == null)
        {
            Debug.LogWarning($"[{npcName}] Cannot open shop, shopUI is NULL!");
            return;
        }

        // Close dialogue, open shop
        if (dialogueUI != null)
            dialogueUI.SetActive(false);

        shopUI.SetActive(true);
        shopOpen = true;
        dialogueOpen = false;
        sellOpen = false;

        Debug.Log($"[{npcName}] Shop UI OPENED from dialogue.");
    }

    private void OpenSell()
    {
        if (sellUI == null)
        {
            Debug.LogWarning($"[{npcName}] Cannot open sell UI, sellUI is NULL!");
            return;
        }

        // Close dialogue, open sell
        if (dialogueUI != null)
            dialogueUI.SetActive(false);

        sellUI.SetActive(true);
        sellOpen = true;
        dialogueOpen = false;
        shopOpen = false;

        Debug.Log($"[{npcName}] Sell UI OPENED from dialogue.");
    }

    private void CloseAll()
    {
        if (dialogueUI != null)
            dialogueUI.SetActive(false);

        if (shopUI != null)
            shopUI.SetActive(false);

        if (sellUI != null)
            sellUI.SetActive(false);

        dialogueOpen = false;
        shopOpen = false;
        sellOpen = false;

        // Lock cursor back to game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Re-enable camera look
        if (cameraLookScript != null)
            cameraLookScript.enabled = true;

        // Let tutorial show star text again if needed
        if (TutorialManager.Instance != null)
            TutorialManager.Instance.OnExternalPromptHidden();

        Debug.Log($"[{npcName}] All NPC UI CLOSED.");
    }

    // =========================
    // UI Button hooks
    // =========================

    // Dialogue button: "Shop"
    public void OnChooseShop()
    {
        OpenShop();
    }

    // Dialogue button: "Sell"
    public void OnChooseSell()
    {
        OpenSell();
    }

    // X button in either shop or sell
    public void OnCloseButtonClicked()
    {
        CloseAll();
    }
}
