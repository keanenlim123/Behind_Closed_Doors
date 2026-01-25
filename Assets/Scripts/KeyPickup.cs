using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyPickup : MonoBehaviour
{
    public DrawerLock drawer; // Assign the DrawerLock script here
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // Subscribe to the selectEntered event (triggered when grabbed)
        grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    void OnDestroy()
    {
        // Always unsubscribe to avoid memory leaks
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
    }

    // Called when the key is grabbed
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        drawer.OnKeyCollected();
    }
}
