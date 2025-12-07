using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public AudioClip walk;
    public AudioClip hit;
    public AudioClip orb;
    public AudioClip bolt;
    public AudioClip explosion;
    public AudioClip heal;
    public AudioClip deny;
    public AudioClip doorUnlock;
    public AudioClip doorDeny;
    public AudioClip connect;

    private void Awake()
    {
        instance = this;
    }
}
