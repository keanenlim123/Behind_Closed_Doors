using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;

    [Header("Scene Lists")]
    public List<int> normalScenes = new List<int>(); // Difficulty 0
    public List<int> hardScenes = new List<int>();   // Difficulty > 0

    [Header("Gameplay Settings")]
    public int difficulty = 0; // Set this from other scripts or Inspector

    // This function is called by your UI button
    public void OnNextSceneButtonClicked()
    {
        StartCoroutine(RunRandomScene());
    }

    private IEnumerator RunRandomScene()
    {
        // Pick a random scene based on difficulty
        int sceneIndex;

        if (difficulty > 0 && hardScenes.Count > 0)
        {
            sceneIndex = hardScenes[Random.Range(0, hardScenes.Count)];
        }
        else if (normalScenes.Count > 0)
        {
            sceneIndex = normalScenes[Random.Range(0, normalScenes.Count)];
        }
        else
        {
            Debug.LogWarning("Scene lists are empty!");
            yield break;
        }

        // Fade out before loading next scene
        fadeScreen.gameObject.SetActive(true);
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        SceneManager.LoadScene(sceneIndex);
    }
}
