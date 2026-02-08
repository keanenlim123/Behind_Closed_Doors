using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class NotebookBehaviour : MonoBehaviour
{
    public TextMeshProUGUI notebookText;
    public TextMeshProUGUI counterText;

    public int totalItemsToScan = 5;

    public Canvas completionCanvas;

    public AudioSource logAudio; // Assign an AudioSource in the Inspector

    private List<string> loggedObjects = new List<string>();
    private bool completed = false;

    void Start()
    {
        if (completionCanvas != null)
            completionCanvas.gameObject.SetActive(false);
    }

    public void LogObject(string objectName)
    {
        if (loggedObjects.Contains(objectName) || completed)
            return;

        loggedObjects.Add(objectName);
        UpdateNotebookText();
        UpdateCounter();

        PlayLogAudio();

        CheckCompletion();
    }

    void UpdateNotebookText()
    {
        notebookText.text = "";

        for (int i = 0; i < loggedObjects.Count; i++)
        {
            notebookText.text += "- " + loggedObjects[i] + "\n";
        }
    }

    void UpdateCounter()
    {
        counterText.text = loggedObjects.Count + " / " + totalItemsToScan;
    }

    void CheckCompletion()
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
