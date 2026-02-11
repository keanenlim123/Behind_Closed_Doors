/// Author : Waine Low
/// Date Created : 05/02/2026
/// Description : Listens for achievement-related pickups in the game.
/// When an object with an AchievementTag is grabbed, it registers the pickup with the BadgeSystem.

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class AchievementPickupListener : MonoBehaviour
{
    public BadgeSystem badgeSystem; /// Reference to the BadgeSystem

    void Awake() /// Initialize the badge system reference
    {
        if (badgeSystem == null)
            badgeSystem = FindObjectOfType<BadgeSystem>();
    }

    void OnEnable()
    {
        /// Listen to all XRGrabInteractables in the scene
        foreach (var grab in FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>())
        {
            grab.selectEntered.AddListener(OnObjectGrabbed);
        }
    }

    void OnDisable() /// Stop listening when disabled
    {
        foreach (var grab in FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>())
        {
            grab.selectEntered.RemoveListener(OnObjectGrabbed);
        }
    }

    private void OnObjectGrabbed(SelectEnterEventArgs args) /// Called when an object is grabbed
    {
        var tag = args.interactableObject.transform.GetComponent<AchievementTag>();
        if (tag != null && badgeSystem != null)
        {
            badgeSystem.RegisterPickup(tag.achievementId);
        }
    }
}