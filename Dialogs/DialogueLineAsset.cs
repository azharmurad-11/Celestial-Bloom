using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene Dialogue/Line")]
public class DialogueLineAsset : ScriptableObject
{
    public string speaker;

    [TextArea(2, 6)]
    public string text;

    [Header("Optional Audio")]
    public AudioClip voiceClip;   
    [Range(0f, 1f)] public float volume = 1f;
}
