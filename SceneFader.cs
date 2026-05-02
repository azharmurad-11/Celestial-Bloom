using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// This script should be placed on your FadePanel or a Scene Manager object.
public class SceneFader : MonoBehaviour
{
    [Header("References")]
    // Drag your FadePanel Image component here.
    public Image fadePanel;

    [Header("Settings")]
    public float fadeDuration = 1.5f; // Duration of both fade-in and fade-out.

    private void Awake()
    {
        // Ensure the fadePanel reference is set.
        if (fadePanel == null)
        {
            Debug.LogError("Fade Panel reference is missing on SceneFader script.");
            return;
        }

        // Make sure the panel starts fully black in the new scene for an immediate fade-in.
        fadePanel.color = new Color(0, 0, 0, 1);

        // Block clicks while the scene is fading in.
        fadePanel.raycastTarget = true;
    }

    private void Start()
    {
        // Start the fade-in immediately when the scene starts.
        StartFadeIn();
    }

    // --- Public Methods to Initiate Fading ---

    /// <summary>
    /// Starts the fade-out process and loads the specified scene when complete.
    /// Call this from your UI Button's OnClick() events.
    /// </summary>
    public void FadeOutToScene(string sceneName)
    {
        // Ensure the panel is visible and blocking clicks before fading out.
        fadePanel.gameObject.SetActive(true);
        fadePanel.raycastTarget = true;

        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    /// <summary>
    /// Initiates a fade-in (clear screen) when a scene loads.
    /// </summary>
    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    // --- Coroutine Methods for Smooth Fading ---

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        float time = 0f;
        Color startColor = new Color(0, 0, 0, fadePanel.color.a); // Start from current alpha
        Color endColor = new Color(0, 0, 0, 1); // Fade to full black

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / fadeDuration);
            fadePanel.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        // Finalize state and load the scene
        fadePanel.color = endColor;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;
        Color startColor = new Color(0, 0, 0, 1); // Start from full black
        Color endColor = new Color(0, 0, 0, 0);  // Fade to clear

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / fadeDuration);
            fadePanel.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        // Finalize state and allow interaction
        fadePanel.color = endColor;
        fadePanel.raycastTarget = false; // Allow clicks/interactions
        fadePanel.gameObject.SetActive(false); // Optional: Hide the panel when done
    }
}