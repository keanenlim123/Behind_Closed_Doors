using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DrawerLockBehaviour : MonoBehaviour
{
    public ConfigurableJoint drawerHinge;
    public float unlockedMaxAngle = 0.6f;

    private XRSocketInteractor socket;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    void OnEnable()
    {
        socket.selectEntered.AddListener(OnKeyInserted);
    }

    void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnKeyInserted);
    }

    private void OnKeyInserted(SelectEnterEventArgs args)
    {
        UnlockDrawer();
    }

    private void UnlockDrawer()
    {
        drawerHinge.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit limit = drawerHinge.linearLimit;
        limit.limit = unlockedMaxAngle; // meters
        drawerHinge.linearLimit = limit;

        Debug.Log("Drawer unlocked!");
    }
}
