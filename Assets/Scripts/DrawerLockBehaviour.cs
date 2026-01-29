using UnityEngine;

public class DrawerLockBehaviour : MonoBehaviour
{
    public ConfigurableJoint drawerJoint; // Assign the drawer's joint here
    public bool hasKey = false;           // Whether player has key

    void Update()
    {
        // Optional: Check for key pickup manually
        if (hasKey)
        {
            UnlockDrawer();
        }
    }

    public void UnlockDrawer()
    {
        if (drawerJoint != null)
        {
            // Unlock the axis you want (e.g., X axis)
            drawerJoint.xMotion = ConfigurableJointMotion.Limited;

            // Optional: adjust limit to allow full slide
            SoftJointLimit limit = drawerJoint.linearLimit;
            limit.limit = 0.5f; // maximum slide distance
            drawerJoint.linearLimit = limit;
        }
    }

    // Call this from your Key script when the player gets the key
    public void OnKeyCollected()
    {
        hasKey = true;
        UnlockDrawer();
    }
}
