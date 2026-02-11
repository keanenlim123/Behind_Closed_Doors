/// Author : Keanen Lim
/// Date Created : 21/01/2026
/// Description : Handles the logging of scanned objects into the in-game notebook.
/// 

using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class NotebookBehaviour : MonoBehaviour
{
    public TextMeshProUGUI notebookText;
    public TextMeshProUGUI counterText;

    public int totalItemsToScan = 5;

    public Canvas completionCanvas;

    public AudioSource logAudio; /// Assign an AudioSource in the Inspector

    private List<string> loggedObjects = new List<string>();
    private bool completed = false;

    void Start() /// Initialize the notebook and hide completion canvas
    {
        if (completionCanvas != null)
            completionCanvas.gameObject.SetActive(false);
    }

    public void LogObject(string objectName) /// Log a scanned object into the notebook
    {
        if (loggedObjects.Contains(objectName) || completed)
            return;

        loggedObjects.Add(objectName);
        UpdateNotebookText();
        UpdateCounter();

        PlayLogAudio();

        CheckCompletion();
    }

    void UpdateNotebookText() /// Update the notebook display with logged objects
    {
        notebookText.text = "";

        for (int i = 0; i < loggedObjects.Count; i++)
        {
            notebookText.text += "- " + loggedObjects[i] + "\n";
        }
    }

    void UpdateCounter() /// Update the scanned objects counter
    {
        counterText.text = loggedObjects.Count + " / " + totalItemsToScan;
    }

    void CheckCompletion() /// Check if all items have been scanned and show completion canvas
    {
        if (loggedObjects.Count >= totalItemsToScan)
        {
            completed = true;

            if (completionCanvas != null)
                completionCanvas.gameObject.SetActive(true);
        }
    }

    void PlayLogAudio()
    {
        if (logAudio != null)
            logAudio.Play();
    }
}
