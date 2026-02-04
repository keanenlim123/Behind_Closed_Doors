using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    [Header("Fade Settings")]
    public bool startFadedIn = true;
    public float fadeDuration = 5f;
    public float holdDuration = 3f;
    public Color fadeColor = Color.black;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (startFadedIn)
        {
            // Start fully black, then fade in
            Color c = fadeColor;
            c.a = 1f;
            rend.material.SetColor("_BaseColor", c);

            FadeIn();
        }
    }

    // Fade from black → visible
    public void FadeIn()
    {
        StartCoroutine(FadeCoroutine(1f, 0f));
    }

    // Fade from visible → black
    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(0f, 1f));
    }

    // Generic fade if you want manual control
    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeCoroutine(alphaIn, alphaOut));
    }

    // Loading-style fade (fade out → hold → fade in)
    public void FadeForLoading()
    {
        StartCoroutine(FadeForLoadingRoutine());
    }

    IEnumerator FadeForLoadingRoutine()
    {
        yield return StartCoroutine(FadeCoroutine(0f, 1f));

        yield return new WaitForSeconds(holdDuration);

        yield return StartCoroutine(FadeCoroutine(1f, 0f));
    }

    IEnumerator FadeCoroutine(float alphaIn, float alphaOut)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            Color newColor = fadeColor;
            newColor.a = alpha;

            rend.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure final alpha is exact
        Color finalColor = fadeColor;
        finalColor.a = alphaOut;
        rend.material.SetColor("_BaseColor", finalColor);
    }
}
