using UnityEngine;

public class TPZone : TeleportZone
{
    public Transform target;

    private void OnDrawGizmos()
    {
        if (target == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target.position, 0.5f);
        Gizmos.DrawLine(transform.position, target.position);
    }

    public override void OnTeleport()
    {
        GameManager.instance.player.transform.position = target.position;
    }
}
