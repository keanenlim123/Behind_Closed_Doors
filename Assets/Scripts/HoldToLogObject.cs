using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections;

public class HoldToLogObject : MonoBehaviour
{
    public float holdTime = 3f;
    public TextMeshProUGUI notebookText;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Coroutine holdCoroutine;

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
        if (!notebookText.text.Contains(gameObject.name))
        {
            notebookText.text += gameObject.name;
        }
    }
}
