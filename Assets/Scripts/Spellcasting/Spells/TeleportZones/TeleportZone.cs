using UnityEngine;

public abstract class TeleportZone : MonoBehaviour
{
    [SerializeField]
    private float range = 5f;

    public abstract void OnTeleport();

    public bool CanTeleport(Transform entity) {
        Vector2 ep = new Vector2(entity.position.x, entity.position.y);
        Vector2 p = new Vector2(transform.position.x, transform.position.y);

        return (ep - p).magnitude <= range;
    }
}
