using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class CutsceneSceneLoader : MonoBehaviour
{
    // ✅ Flag to prevent LoadingScene from looping back into Cutscene1
    const string KeyCutsceneFinished = "CB_CUTSCENE1_FINISHED";

    [Header("Next Scene")]
    // For Cutscene1, set this to "LoadingScene"
    public string nextSceneName;

    [Header("Assign FadeOut from your FadePanel")]
    public FadeOut fadeOut; // drag FadePanel (with FadeOut script) here

    [Header("Intro Settings")]
    [Tooltip("If true, mark intro as finished so LoadingScene goes to gameplay next.")]
    public bool markIntroFinished = true;

    [Tooltip("Gameplay scene name used by LoadingScene routing.")]
    public string gameplaySceneName = "Celestial Bloom";

    private PlayableDirector director;
    private bool triggered;

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    void OnEnable()
    {
        if (director != null)
            director.stopped += OnTimelineStopped;
    }

    void OnDisable()
    {
        if (director != null)
            director.stopped -= OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector pd)
    {
        if (triggered) return;
        triggered = true;

        StartCoroutine(EndCutscene());
    }

    IEnumerator EndCutscene()
    {
        // Fade out if assigned
        if (fadeOut != null)
            yield return fadeOut.DoFadeOut();
        else
            Debug.LogWarning("[CutsceneSceneLoader] FadeOut not assigned, loading without fade.");

        // ✅ Mark intro finished so LoadingScene won't re-open Cutscene1
        if (markIntroFinished)
        {
            PlayerPrefs.SetInt(KeyCutsceneFinished, 1);
            PlayerPrefs.Save();

            // Optional: create a save immediately so future Play is Continue
            if (SaveSystem.Instance != null)
                SaveSystem.Instance.SaveGame();
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
