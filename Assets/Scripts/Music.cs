using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class Music : MonoBehaviour
{
    [SerializeField] private AudioClip baseClip;
    [SerializeField] private AudioClip melodyClip;
    [SerializeField] private AudioClip combatClip;
    [SerializeField][Range(0, 1)] private float volume = 0.126f;

    public bool muted = false;

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

    public static Music instance { get; private set; } = null;

    void Awake()
    {
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
        baseSource.clip = baseClip;
        baseSource.volume = volume;
        baseSource.loop = true;

        melodySource = gameObject.AddComponent<AudioSource>();
        melodySource.bypassListenerEffects = true;
        melodySource.clip = melodyClip;
        melodySource.volume = volume;
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
        baseSource.volume = Mathf.Lerp(baseSource.volume, muted ? 0 : volume, Time.deltaTime * 2);
        melodySource.volume = Mathf.Lerp(melodySource.volume, muted || gameOver ? 0 : volume, Time.deltaTime * 2);
        combatSource.volume = Mathf.Lerp(combatSource.volume, ((inCombat || gameOver) && !muted) ? volume : 0, Time.deltaTime * 2);
        inCombat = false;
    }

    public void SetInCombat()
    {
        inCombat = true;
    }
}
