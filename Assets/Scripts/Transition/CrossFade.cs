using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFade : SceneTransition
{
    public CanvasGroup crossFade;
    public float fadeDuration;

    public override IEnumerator AnimTranstitionIn()
    {
        crossFade.alpha = 0f;
        crossFade.blocksRaycasts = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            crossFade.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        crossFade.alpha = 1f; // Ensure fully opaque
    }

    public override IEnumerator AnimTranstitionOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            crossFade.alpha = 1f - Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        crossFade.alpha = 0f; // Ensure fully transparent
        crossFade.blocksRaycasts = false;
    }

}
