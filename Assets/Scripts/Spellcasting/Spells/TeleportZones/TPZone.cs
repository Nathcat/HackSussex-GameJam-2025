using UnityEngine;
using UnityEngine.SceneManagement;

public class TPZone : TeleportZone
{
    public Vector3 targetLocation;

    public override void OnTeleport()
    {
        GameManager.instance.player.transform.position = targetLocation;
        AudioManager.instance.teleport.PlayAt(targetLocation);
    }
}
