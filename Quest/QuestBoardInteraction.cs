using UnityEngine;

public class QuestBoardInteraction : MonoBehaviour
{
    [Header("Player")]
    public string playerTag = "Player";
    public MonoBehaviour playerController;   // e.g. PlayerThirdPersonMovement
    public GameObject playerVisualRoot;      // model to hide

    [Header("UI")]
    public GameObject promptUI;              // "Press Q"
    public GameObject boardCanvas;           // quest UI
    public KeyCode interactKey = KeyCode.Q;

    [Header("Cameras")]
    public Camera gameplayCamera;            // <- THIS will be your Main Camera
    public Camera boardCamera;               // <- close-up camera on board

    private bool playerInRange = false;
    private bool isViewingBoard = false;

    private CursorLockMode previousLock;
    private bool previousCursorVisible;

    private void Start()
    {
        if (promptUI != null) promptUI.SetActive(false);
        if (boardCanvas != null) boardCanvas.SetActive(false);

        if (gameplayCamera != null) gameplayCamera.enabled = true;
        if (boardCamera != null) boardCamera.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = true;

        if (!isViewingBoard && promptUI != null)
            promptUI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = false;

        if (promptUI != null)
            promptUI.SetActive(false);

        if (isViewingBoard)
            CloseBoard();
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (!isViewingBoard) OpenBoard();
            else CloseBoard();
        }
    }

    private void OpenBoard()
    {
        isViewingBoard = true;

        if (promptUI != null)
            promptUI.SetActive(false);

        if (boardCanvas != null)
            boardCanvas.SetActive(true);

        // ✅ THIS is the missing connection:
        // If quest is completed, opening the board will trigger credits.
        QuestCropsGoal.Instance?.OnQuestBoardOpened();

        if (playerController != null)
            playerController.enabled = false;

        if (playerVisualRoot != null)
            playerVisualRoot.SetActive(false);

        if (gameplayCamera != null)
            gameplayCamera.enabled = false;

        if (boardCamera != null)
            boardCamera.enabled = true;

        previousLock = Cursor.lockState;
        previousCursorVisible = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseBoard()
    {
        isViewingBoard = false;

        if (boardCanvas != null)
            boardCanvas.SetActive(false);

        if (playerController != null)
            playerController.enabled = true;

        if (playerVisualRoot != null)
            playerVisualRoot.SetActive(true);

        if (boardCamera != null)
            boardCamera.enabled = false;

        if (gameplayCamera != null)
            gameplayCamera.enabled = true;

        Cursor.lockState = previousLock;
        Cursor.visible = previousCursorVisible;

        if (playerInRange && promptUI != null)
            promptUI.SetActive(true);
    }
}
