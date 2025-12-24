using UnityEngine;

[DefaultExecutionOrder(-100)]
public class Music : MonoBehaviour
{
    [SerializeField] private AudioClip baseMusic;
    [SerializeField] private AudioClip combatOverlay;
    [SerializeField][Range(0, 1)] private float volume = 0.126f;

    public bool muted = false;

    private AudioSource overlaySource;
    private AudioSource baseSource;
    private bool inCombat = false;

    public static Music instance { get; private set; } = null;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    void Start()
    {
        baseSource = gameObject.AddComponent<AudioSource>();
        baseSource.bypassListenerEffects = true;
        baseSource.clip = baseMusic;
        baseSource.loop = true;

        overlaySource = gameObject.AddComponent<AudioSource>();
        overlaySource.bypassListenerEffects = true;
        overlaySource.clip = combatOverlay;
        overlaySource.loop = true;
        overlaySource.volume = 0;

        baseSource.Play();
        overlaySource.Play();
    }

    void Update()
    {
        baseSource.volume = muted ? 0 : volume;
        overlaySource.volume = Mathf.MoveTowards(overlaySource.volume, inCombat && !muted ? volume : 0, Time.deltaTime);
        inCombat = false;
    }

    public void SetInCombat()
    {
        inCombat = true;
    }
}
