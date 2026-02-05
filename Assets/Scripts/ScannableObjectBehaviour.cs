using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.UI;

public class ScannableObject : MonoBehaviour
{
    public float holdTime = 3f;
    public NotebookBehaviour notebook;

    public Image radialScanImage; // assign the circular Image
    public Canvas scanCanvas;     // optional, shows bar

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Coroutine holdCoroutine;
    private bool hasBeenScanned = false;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (scanCanvas != null)
            scanCanvas.gameObject.SetActive(false);
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
        if (!hasBeenScanned)
            holdCoroutine = StartCoroutine(HoldTimer());
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }

        ResetScanUI();
    }

    IEnumerator HoldTimer()
    {
        if (scanCanvas != null)
            scanCanvas.gameObject.SetActive(true);

        float timer = 0f;

        while (timer < holdTime)
        {
            timer += Time.deltaTime;

            if (radialScanImage != null)
                radialScanImage.fillAmount = timer / holdTime;

            yield return null;
        }

        ScanObject();
    }

    void ScanObject()
    {
        hasBeenScanned = true;
        notebook.LogObject(gameObject.name);
        ResetScanUI();
    }

    void ResetScanUI()
    {
        if (scanCanvas != null)
            scanCanvas.gameObject.SetActive(false);

        if (radialScanImage != null)
            radialScanImage.fillAmount = 0f;
    }
}
