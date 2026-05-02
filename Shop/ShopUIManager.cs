using UnityEngine;

// Put this on your Shop Canvas or Shop Panel
public class ShopUIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject shopPanel;          // The root shop UI panel

    [Header("Camera Control")]
    // Drag the component that handles camera look here
    // e.g. your camera controller script OR CinemachineFreeLook component
    public Behaviour cameraLookComponent;

    public bool IsOpen { get; private set; } = false;

    private void Start()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    public void OpenShop()
    {
        if (shopPanel == null)
        {
            Debug.LogWarning("[ShopUIManager] No shopPanel assigned!");
            return;
        }

        shopPanel.SetActive(true);
        IsOpen = true;

        // Unlock mouse for UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable camera look so moving mouse doesn't move camera
        if (cameraLookComponent != null)
            cameraLookComponent.enabled = false;

        // Tell tutorial that an external prompt (shop) is showing
        if (TutorialManager.Instance != null)
            TutorialManager.Instance.OnExternalPromptShown();

        Debug.Log("[ShopUIManager] Shop OPEN");
    }

    public void CloseShop()
    {
        if (shopPanel == null)
        {
            Debug.LogWarning("[ShopUIManager] No shopPanel assigned!");
            return;
        }

        shopPanel.SetActive(false);
        IsOpen = false;

        // Lock mouse back to game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Re-enable camera look
        if (cameraLookComponent != null)
            cameraLookComponent.enabled = true;

        // Tell tutorial external prompt is gone
        if (TutorialManager.Instance != null)
            TutorialManager.Instance.OnExternalPromptHidden();

        Debug.Log("[ShopUIManager] Shop CLOSE");
    }

    // Hook this to your X button
    public void OnCloseButtonClicked()
    {
        CloseShop();
    }
}
