using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class NotebookBehaviour : MonoBehaviour
{
    public TextMeshProUGUI notebookText;
    public TextMeshProUGUI counterText;

    public int totalItemsToScan = 5;

    public Canvas completionCanvas;

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
}
