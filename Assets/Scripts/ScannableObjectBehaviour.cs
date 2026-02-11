using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class ScannableObject : MonoBehaviour
{
    public float holdTime = 3f;
    public NotebookBehaviour notebook;

    public Image radialScanImage;
    public Canvas scanCanvas;

    public string disorderType;   // eatingDisorder / depression / schizophrenia
    public string objectKey;      // snackBar / pills / notebook etc

    public AudioSource scanAudioSource;
    public TextMeshProUGUI captionText;
    public float captionDisplayTime = 3f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Coroutine holdCoroutine;
    private bool hasBeenScanned = false;

    private DatabaseReference db;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        db = FirebaseDatabase.DefaultInstance.RootReference;

        if (scanCanvas != null)
            scanCanvas.gameObject.SetActive(false);

        if (captionText != null)
            captionText.gameObject.SetActive(false);
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

        if (notebook != null)
            notebook.LogObject(objectKey);

        if (scanAudioSource != null)
            scanAudioSource.Play();

        StartCoroutine(GetCaptionFromDatabase());

        ResetScanUI();
    }

    IEnumerator GetCaptionFromDatabase()
    {
        var task = db.Child("captions")
                     .Child(disorderType)
                     .Child(objectKey)
                     .GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(task.Exception);
            yield break;
        }

        if (task.Result.Exists)
        {
            string captionMessage = task.Result.Value.ToString();

            if (!string.IsNullOrEmpty(captionMessage))
                StartCoroutine(ShowCaption(captionMessage));
        }
    }

    IEnumerator ShowCaption(string message)
    {
        captionText.gameObject.SetActive(true);
        captionText.text = message;

        yield return new WaitForSeconds(captionDisplayTime);

        captionText.gameObject.SetActive(false);
    }

    void ResetScanUI()
    {
        if (scanCanvas != null)
            scanCanvas.gameObject.SetActive(false);

        if (radialScanImage != null)
            radialScanImage.fillAmount = 0f;
    }
}
