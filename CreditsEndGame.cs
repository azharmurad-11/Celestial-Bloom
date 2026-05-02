using UnityEngine;

public class CreditsEndGame : MonoBehaviour
{
    public float waitSeconds = 8f;

    private void Start()
    {
        Invoke(nameof(QuitNow), waitSeconds);
    }

    public void QuitNow()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
