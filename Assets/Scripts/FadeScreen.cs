/// Author : Justin Tan
/// Date Created : 30/01/2026
/// Description : Handles the ending scene logic, including user verification and updating user statistics in Firebase.
/// 

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

    void Start() /// Initialize the renderer and set starting fade state
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

    /// <summary>
    /// Fade from black → visible
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine(FadeCoroutine(1f, 0f));
    }

    /// <summary>
    /// Fade from visible → black
    /// </summary>
    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(0f, 1f));
    }

    /// <summary>
    /// Generic fade if you want manual control
    /// </summary>
    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeCoroutine(alphaIn, alphaOut));
    }

    /// <summary>
    /// Loading-style fade (fade out → hold → fade in)
    /// </summary>
    public void FadeForLoading()
    {
        StartCoroutine(FadeForLoadingRoutine());
    }

    IEnumerator FadeForLoadingRoutine() /// Fade out, hold, then fade in
    {
        yield return StartCoroutine(FadeCoroutine(0f, 1f));

        yield return new WaitForSeconds(holdDuration);

        yield return StartCoroutine(FadeCoroutine(1f, 0f));
    }

    IEnumerator FadeCoroutine(float alphaIn, float alphaOut) /// Generic fade coroutine
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

        /// <summary>
        /// Ensure final alpha is exact
        /// </summary>
        Color finalColor = fadeColor;
        finalColor.a = alphaOut;
        rend.material.SetColor("_BaseColor", finalColor);
    }
}
