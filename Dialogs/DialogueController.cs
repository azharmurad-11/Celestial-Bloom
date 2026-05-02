using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialogueRoot;
    public TMP_Text speakerText;
    public TMP_Text lineText;

    [Header("Audio")]
    public AudioSource voiceSource;      // assign OR auto-create
    public bool stopPreviousVoice = true;

    [Header("Optional Typewriter")]
    public bool typewriter = false;
    public float typeSpeed = 0.02f;

    Coroutine typingCo;

    void Awake()
    {
        // Auto-create an AudioSource if you forgot to add one
        if (voiceSource == null)
        {
            voiceSource = gameObject.GetComponent<AudioSource>();
            if (voiceSource == null)
                voiceSource = gameObject.AddComponent<AudioSource>();

            voiceSource.playOnAwake = false;
            voiceSource.loop = false;
            voiceSource.spatialBlend = 0f; // 2D voice by default
        }
    }

    void Start()
    {
        Hide();
    }

    public void ShowLine(string speaker, string line, AudioClip clip = null, float volume = 1f)
    {
        if (dialogueRoot != null)
            dialogueRoot.SetActive(true);

        if (typingCo != null)
            StopCoroutine(typingCo);

        // Text
        if (!typewriter)
        {
            if (speakerText != null) speakerText.text = speaker;
            if (lineText != null) lineText.text = line;
        }
        else
        {
            typingCo = StartCoroutine(TypeLine(speaker, line));
        }

        // Audio
        PlayVoice(clip, volume);
    }

    public void Hide()
    {
        if (typingCo != null)
            StopCoroutine(typingCo);

        if (dialogueRoot != null)
            dialogueRoot.SetActive(false);

        if (stopPreviousVoice && voiceSource != null)
            voiceSource.Stop();
    }

    void PlayVoice(AudioClip clip, float volume)
    {
        if (voiceSource == null) return;

        if (stopPreviousVoice)
            voiceSource.Stop();

        if (clip == null) return;

        voiceSource.volume = Mathf.Clamp01(volume);
        voiceSource.PlayOneShot(clip, voiceSource.volume);
    }

    IEnumerator TypeLine(string speaker, string line)
    {
        if (speakerText != null) speakerText.text = speaker;
        if (lineText != null) lineText.text = "";

        for (int i = 0; i < line.Length; i++)
        {
            lineText.text += line[i];
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
