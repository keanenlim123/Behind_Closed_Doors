/// Author : Keanen Lim
/// Date Created : 23/01/2026
/// Description : Handles the scanning of objects in VR to retrieve captions from Firebase and log them into the notebook.
/// 

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

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable; /// Reference to the XRGrabInteractable component
    private Coroutine holdCoroutine; 
    private bool hasBeenScanned = false;

    private DatabaseReference db;

    void Awake() /// Initialize components and database reference
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        db = FirebaseDatabase.DefaultInstance.RootReference;

        if (scanCanvas != null)
            scanCanvas.gameObject.SetActive(false);

        if (captionText != null)
            captionText.gameObject.SetActive(false);
    }

    void OnEnable() /// Subscribe to grab and release events
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }
    
    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args) /// Handle grab of the object
    {
        if (!hasBeenScanned)
            holdCoroutine = StartCoroutine(HoldTimer());
    }

    void OnRelease(SelectExitEventArgs args) /// Handle release of the object
    {
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
            holdCoroutine = null;
        }

        ResetScanUI();
    }

    IEnumerator HoldTimer() /// Handle the hold timer for scanning
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

    void ScanObject() /// Handle the scanning of the object
    {
        hasBeenScanned = true;

        if (notebook != null)
            notebook.LogObject(objectKey);

        if (scanAudioSource != null)
            scanAudioSource.Play();

        StartCoroutine(GetCaptionFromDatabase());

        ResetScanUI();
    }

    IEnumerator GetCaptionFromDatabase() /// Retrieve caption from Firebase Realtime Database
    {
        var task = db.Child("captions") 
                     .Child(disorderType)
                     .Child(objectKey)
                     .GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted); /// Wait for the task to complete

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

    IEnumerator ShowCaption(string message) /// Display the caption on screen for a set duration
    {
        captionText.gameObject.SetActive(true);
        captionText.text = message;

        yield return new WaitForSeconds(captionDisplayTime);

        captionText.gameObject.SetActive(false);
    }

    void ResetScanUI() /// Reset the scanning UI elements
    {
        if (scanCanvas != null)
            scanCanvas.gameObject.SetActive(false);

        if (radialScanImage != null)
            radialScanImage.fillAmount = 0f;
    }
}
