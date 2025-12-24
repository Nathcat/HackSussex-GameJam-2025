using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private AggroGroup aggroGroup;
    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private bool infinite = false;
    [SerializeField] private EnemyController[] enemies;


    private float timer = 0f;
    private bool spawning = false;
    private int count = -1;

    private void Start()
    {
        timer = spawnDelay;
        aggroGroup.aggroEvent.AddListener(Aggro);
    }

    private void Update()
    {
        if (!spawning) return;

        timer -= Time.deltaTime;
        if (timer <= 0f && count < enemies.Length)
        {
            EnemyController enemy = Instantiate(enemies[count], transform.position, Quaternion.identity);
            enemy.aggroGroup = aggroGroup;

            count = count + 1;
            if (count >= enemies.Length && infinite) count = 0;

            timer = spawnInterval;
        }
    }

    public void Aggro(Entity target)
    {
        if (count == -1)
        {
            SetSpawning(true);
            count = 0;
        }
    }

    public void SetSpawning(bool value)
    {
        spawning = value;
    }
}
