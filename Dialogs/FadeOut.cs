using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    public float duration = 1f;
    CanvasGroup cg;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    public IEnumerator DoFadeOut()
    {
        cg.blocksRaycasts = true;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        cg.alpha = 1f;
    }
}
