public class WinZone : TeleportZone
{
    public override void OnTeleport()
    {
        LevelLoader.Load("Win");
    }
}
