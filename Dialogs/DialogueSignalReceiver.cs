using UnityEngine;

public class DialogueSignalReceiver : MonoBehaviour
{
    public DialogueController dialogue;

    public void SetLine(DialogueLineAsset line)
    {
        if (dialogue == null || line == null) return;

        dialogue.ShowLine(
            line.speaker,
            line.text,
            line.voiceClip,
            line.volume
        );
    }

    public void HideDialogue()
    {
        if (dialogue == null) return;
        dialogue.Hide();
    }
}
