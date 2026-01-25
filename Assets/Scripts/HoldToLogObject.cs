using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class HoldToLogObject : MonoBehaviour
{
    public float holdTime = 3f;
    public TextMeshProUGUI notebookText;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Coroutine holdCoroutine;

    // List to store logged object names
    private List<string> loggedObjects = new List<string>();

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        holdCoroutine = StartCoroutine(HoldTimer());
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }
    }

    IEnumerator HoldTimer()
    {
        yield return new WaitForSeconds(holdTime);
        AddObjectName();
    }

    void AddObjectName()
    {
        if (!loggedObjects.Contains(gameObject.name))
        {
            loggedObjects.Add(gameObject.name);
            UpdateNotebookText();
        }
    }

    void UpdateNotebookText()
    {
        for (int i = 0; i < loggedObjects.Count; i++)
        {
            notebookText.text += "- " + loggedObjects[i] + "\n";
        }
    }
}
