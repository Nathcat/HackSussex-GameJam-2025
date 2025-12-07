using UnityEngine;

public class TeleportSpell : MonoBehaviour
{
    public void onSpellInvoked(Vector3 target) {
        bool s = false;

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("TeleportZone")) {
            TeleportZone zone;
            if ((zone = o.GetComponent<TeleportZone>()).CanTeleport(transform)) {
                zone.OnTeleport();
                s = true;
                break;
            }
        }

        if (!s) Debug.LogWarning("Can't teleport here!");

        return;
    }
}
