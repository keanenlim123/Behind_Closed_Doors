/// Author : Jaasper Lee
/// Date Created : 25/01/2026
/// Description : Handles the unlocking of a drawer when the correct key is inserted.
/// 

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DrawerLockBehaviour : MonoBehaviour
{
    public ConfigurableJoint drawerHinge;
    public float unlockedMaxAngle = 0.6f;

    private XRSocketInteractor socket;

    void Awake() /// Initialize the socket interactor
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    void OnEnable() /// Subscribe to key insertion event
    {
        socket.selectEntered.AddListener(OnKeyInserted);
    }

    void OnDisable() /// Unsubscribe from key insertion event

    {
        socket.selectEntered.RemoveListener(OnKeyInserted);
    }

    private void OnKeyInserted(SelectEnterEventArgs args) /// Called when a key is inserted into the drawer
    {
        UnlockDrawer();
    }

    private void UnlockDrawer() /// Unlocks the drawer by adjusting the joint limits
    {
        drawerHinge.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit limit = drawerHinge.linearLimit;
        limit.limit = unlockedMaxAngle; // meters
        drawerHinge.linearLimit = limit;

        Debug.Log("Drawer unlocked!");
    }
}
