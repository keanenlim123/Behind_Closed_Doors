using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;
using Firebase.Extensions;
using TMPro;
using Firebase.Auth;
using Firebase;
using System;

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

    /// <summary>
    /// Canvas containing the login UI
    /// </summary>
    public GameObject LoginCanvas;

    /// <summary>
    /// Animator component for the door animation
    /// </summary>
    public Animator DoorAnimator;

    /// <summary>
    /// Name of the door open animation trigger (default: "OpenDoor")
    /// </summary>
    public string DoorOpenTrigger = "DoorOpen";


    public void SignOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
    }

    /// <summary>
    /// Gets the current timestamp in ISO 8601 format (UTC)
    /// </summary>
    /// <returns>Timestamp string in format: 2026-01-30T11:03:04.517Z</returns>
    private string GetCurrentTimestamp()
    {
        return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    }

    /// <summary>
    /// Handles successful authentication by playing door animation and hiding login canvas
    /// </summary>
    private void OnAuthenticationSuccess()
    {
        // Play door open animation
        if (DoorAnimator != null)
        {
            DoorAnimator.SetTrigger(DoorOpenTrigger);
            Debug.Log("Door animation triggered.");
        }
        else
        {
            Debug.LogWarning("DoorAnimator is not assigned!");
        }

        // Hide login canvas
        if (LoginCanvas != null)
        {
            LoginCanvas.SetActive(false);
            Debug.Log("Login canvas hidden.");
        }
        else
        {
            Debug.LogWarning("LoginCanvas is not assigned!");
        }
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
                email = EmailInput.text,
                createdAt = GetCurrentTimestamp()
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
                    // Trigger success actions after data upload completes
                    OnAuthenticationSuccess();
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
                
                // Trigger success actions
                OnAuthenticationSuccess();
            }
        });
    }
}