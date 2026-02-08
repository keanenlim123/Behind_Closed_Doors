using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class AchievementPickupListener : MonoBehaviour
{
    public BadgeSystem badgeSystem;

    void Awake()
    {
        if (badgeSystem == null)
            badgeSystem = FindObjectOfType<BadgeSystem>();
    }

    void OnEnable()
    {
        // Listen to all XRGrabInteractables in the scene
        foreach (var grab in FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>())
        {
            grab.selectEntered.AddListener(OnObjectGrabbed);
        }
    }

    void OnDisable()
    {
        foreach (var grab in FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>())
        {
            grab.selectEntered.RemoveListener(OnObjectGrabbed);
        }
    }

    private void OnObjectGrabbed(SelectEnterEventArgs args)
    {
        var tag = args.interactableObject.transform.GetComponent<AchievementTag>();
        if (tag != null && badgeSystem != null)
        {
            badgeSystem.RegisterPickup(tag.achievementId);
        }
    }
}