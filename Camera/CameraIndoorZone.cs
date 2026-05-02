using UnityEngine;
using Unity.Cinemachine;   // new Cinemachine namespace

[RequireComponent(typeof(BoxCollider))]
public class CameraIndoorZone : MonoBehaviour
{
    [Header("Player")]
    public string playerTag = "Player";

    private CinemachineFreeLook freeLook;
    private CinemachineDeoccluder deoccluder;

    private bool originalEnabled = true;
    private bool initialized = false;

    private void Awake()
    {
        // Make sure the collider is a trigger
        var col = GetComponent<BoxCollider>();
        col.isTrigger = true;
    }

    private void TryInit()
    {
        if (initialized) return;

        // Find the FreeLook camera in the scene
        freeLook = FindObjectOfType<CinemachineFreeLook>();
        if (freeLook == null)
        {
            Debug.LogWarning("[CameraIndoorZone] No CinemachineFreeLook found in scene.");
            return;
        }

        // Get the Deoccluder extension on that FreeLook
        deoccluder = freeLook.GetComponent<CinemachineDeoccluder>();
        if (deoccluder == null)
        {
            Debug.LogWarning("[CameraIndoorZone] No CinemachineDeoccluder found on FreeLook camera.");
            return;
        }

        originalEnabled = deoccluder.enabled;
        initialized = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        TryInit();
        if (!initialized) return;

        // INSIDE HOUSE → turn OFF deoccluder so camera stops jumping to roof
        deoccluder.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (!initialized) return;

        // OUTSIDE → restore original behaviour
        deoccluder.enabled = originalEnabled;
    }
}
