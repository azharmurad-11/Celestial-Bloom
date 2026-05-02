using UnityEngine;
using Unity.Cinemachine;   // << IMPORTANT

public class FreeLookCameraSettings : MonoBehaviour
{
    public CinemachineFreeLook freeLook;

    [Header("Outdoor Settings")]
    public float outdoorTopRadius = 3.5f;
    public float outdoorMidRadius = 3.0f;
    public float outdoorBottomRadius = 2.5f;

    [Header("Indoor Settings")]
    public float indoorTopRadius = 2.4f;
    public float indoorMidRadius = 2.0f;
    public float indoorBottomRadius = 1.8f;

    private void Reset()
    {
        // auto-assign when you add the script on the FreeLook object
        freeLook = GetComponent<CinemachineFreeLook>();
    }

    public void ApplyOutdoorSettings()
    {
        if (freeLook == null) return;

        freeLook.m_Orbits[0].m_Radius = outdoorTopRadius;
        freeLook.m_Orbits[1].m_Radius = outdoorMidRadius;
        freeLook.m_Orbits[2].m_Radius = outdoorBottomRadius;
    }

    public void ApplyIndoorSettings()
    {
        if (freeLook == null) return;

        freeLook.m_Orbits[0].m_Radius = indoorTopRadius;
        freeLook.m_Orbits[1].m_Radius = indoorMidRadius;
        freeLook.m_Orbits[2].m_Radius = indoorBottomRadius;
    }
}
