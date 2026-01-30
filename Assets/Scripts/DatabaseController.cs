using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;
using Firebase.Extensions;
using TMPro;
using Firebase.Auth;
using Firebase;

/// <summary>
/// Handles Firebase authentication and database operations for players.
/// Supports signing up, signing in, signing out, and uploading initial player data including habitats and animals.
/// </summary>
public class DatabaseController : MonoBehaviour
{
    /// <summary>
    /// Input field for user email during sign up or sign in.
    /// </summary>
    public TMP_InputField EmailInput;

    /// <summary>
    /// Input field for user password during sign up or sign in.
    /// </summary>
    public TMP_InputField PasswordInput;

    /// <summary>
    /// Input field for username during sign up.
    /// </summary>
    public TMP_InputField UsernameInput;

    void Start()
    {
        DatabaseReference db = FirebaseDatabase.DefaultInstance.RootReference;
        UserData testUser = new UserData { username = "TestUser", email = "test@email.com" };
        string json = JsonUtility.ToJson(testUser);

        db.Child("users").Child("test123").SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                foreach (var e in task.Exception.Flatten().InnerExceptions)
                    Debug.LogError("Upload Error: " + e.Message);
            }
            else
            {
                Debug.Log("Hardcoded test user uploaded successfully!");
            }
        });

    }

    public void SignOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
    }

    /// <summary>
    /// Creates a new user with email and password, sets up the initial player data including habitats and animals,
    /// and uploads it to Firebase Realtime Database.
    /// </summary>
    public void SignUp()
    {
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(EmailInput.text, PasswordInput.text)
            .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("SignUp Error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result.User;
            string uid = newUser.UserId;
            Debug.Log($"User signed up successfully: {uid}");

            UserData userData = new UserData
            {
                username = UsernameInput.text,
                email = EmailInput.text
            };

            string json = JsonUtility.ToJson(userData);
            Debug.Log("Uploading JSON: " + json);

            DatabaseReference db = FirebaseDatabase.DefaultInstance.RootReference;
            db.Child("users").Child(uid).SetRawJsonValueAsync(json)
                .ContinueWithOnMainThread(uploadTask =>
            {
                if (uploadTask.IsFaulted)
                {
                    Debug.LogError("Upload failed: " + uploadTask.Exception);
                }
                else
                {
                    Debug.Log("Player data uploaded successfully.");
                }
            });
        });
    }

    /// <summary>
    /// Signs in an existing user using email and password.
    /// Logs the user id upon successful login.
    /// </summary>
    public void SignIn()
    {
        var signInTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(EmailInput.text, PasswordInput.text);
        signInTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Can't sign in due to error!!!");
                return;
            }

            if (task.IsCompleted)
            {
                FirebaseUser user = task.Result.User;
                Debug.Log($"User signed in successfully, id: {user.UserId}");
            }
        });
    }
}