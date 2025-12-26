using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class Music : MonoBehaviour
{
    [SerializeField] private AudioClip baseClip;
    [SerializeField] private AudioClip melodyClip;
    [SerializeField] private AudioClip combatClip;
    [SerializeField][Range(0, 1)] private float volume = 0.126f;

    public bool muted { get; private set; }

    private AudioSource combatSource;
    private AudioSource melodySource;
    private AudioSource baseSource;
    private bool inCombat = false;

    private bool gameOver
    {
        get
        {
            return SceneManager.GetActiveScene().name == "GameOver";
        }
    }

    private bool silent
    {
        get
        {
            return muted || SceneManager.GetActiveScene().name == "Win";
        }
    }

    public static Music instance { get; private set; } = null;

    void Awake()
    {
        muted = PlayerPrefs.GetInt("music_muted", 0) == 1;
        if (instance != null) Destroy(gameObject);
        else {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    void Start()
    {
        baseSource = gameObject.AddComponent<AudioSource>();
        baseSource.bypassListenerEffects = true;
        baseSource.volume = muted ? 0 : volume;
        baseSource.clip = baseClip;
        baseSource.loop = true;

        melodySource = gameObject.AddComponent<AudioSource>();
        melodySource.bypassListenerEffects = true;
        melodySource.volume = muted ? 0 : volume;
        melodySource.clip = melodyClip;
        melodySource.loop = true;

        combatSource = gameObject.AddComponent<AudioSource>();
        combatSource.bypassListenerEffects = true;
        combatSource.clip = combatClip;
        combatSource.loop = true;
        combatSource.volume = 0;

        baseSource.Play();
        melodySource.Play();
        combatSource.Play();
    }

    void Update()
    {
        baseSource.volume = Mathf.Lerp(baseSource.volume, silent ? 0 : volume, Time.deltaTime * 2);
        melodySource.volume = Mathf.Lerp(melodySource.volume, silent || gameOver ? 0 : volume, Time.deltaTime * 2);
        combatSource.volume = Mathf.Lerp(combatSource.volume, ((inCombat || gameOver) && !silent) ? volume : 0, Time.deltaTime * 2);
        inCombat = false;
    }

    public static void SetInCombat()
    {
        if (instance != null) instance.inCombat = true;
    }

    public static bool ToggleMute()
    {
        instance.muted = !instance.muted;
        PlayerPrefs.SetInt("music_muted", instance.muted ? 1 : 0);
        return instance.muted;
    }
}
