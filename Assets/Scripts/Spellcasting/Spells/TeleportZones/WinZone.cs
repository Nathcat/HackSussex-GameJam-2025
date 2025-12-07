using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : TeleportZone
{
    public override void OnTeleport()
    {
        SceneManager.LoadScene("Win");
    }
}
