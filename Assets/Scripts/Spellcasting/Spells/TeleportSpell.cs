using UnityEngine;

public class TeleportSpell : MonoBehaviour
{
    public void onSpellInvoked(Vector3 target) {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("TeleportZone")) {
            TeleportZone zone;
            if ((zone = o.GetComponent<TeleportZone>()).CanTeleport(transform)) {
                GameManager.instance.player.Teleport(() => zone.OnTeleport());
                return;
            }
        }

        Debug.LogWarning("Can't teleport here!");
    }
}
