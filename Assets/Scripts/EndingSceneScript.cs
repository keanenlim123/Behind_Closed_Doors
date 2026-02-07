using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingSceneScript : MonoBehaviour
{
    public Button firstVerificationButton;
    public Button secondVerificationButton;
    public string sceneToLoad = "NextScene"; // Set your target scene name

    private bool firstVerified = false;

    void Start()
    {
        firstVerificationButton.onClick.AddListener(OnFirstVerification);
        secondVerificationButton.onClick.AddListener(OnSecondVerification);
    }

    void OnFirstVerification()
    {
        firstVerified = true;
        Debug.Log("First verification complete.");
    }

    void OnSecondVerification()
    {
        if (firstVerified)
        {
            SceneManager.LoadScene(sceneToLoad);
            Debug.Log("Second verification complete. Loading scene...");
        }
        else
        {
            Debug.Log("Second verification attempted before first. Not loading scene.");
        }
    }
}