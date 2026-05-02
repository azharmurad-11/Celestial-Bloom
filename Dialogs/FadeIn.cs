using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    public float duration = 1f;
    CanvasGroup cg;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 1f; // start black
    }

    IEnumerator Start()
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }

        cg.alpha = 0f;
        cg.blocksRaycasts = false;
    }
}
