using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private BoxCollider groundCollider;
    [SerializeField] private int enemyAmount = 0;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private int maxEnemies = 10;

    [Header("Difficulty Scaling")]
    [SerializeField] private float minSpawnInterval = 2f;   // lowest possible interval
    [SerializeField] private float spawnAcceleration = 0.9f; // how much faster each spawn gets (e.g., 0.9 = 10% faster)

    public int enemyCount;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < enemyAmount; i++)
            SpawnEnemy();

        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemyCount >= maxEnemies)
            return;

        Vector3 startPos = RandomPointInBounds(groundCollider.bounds);
        GameObject enemyObj = Instantiate(enemyPrefab, startPos, Quaternion.identity);

        EnemyHealth health = enemyObj.GetComponent<EnemyHealth>();
        if (health != null)
            health.Initialize(health.maxHealth);

        enemyCount++;

        // 🔹 Speed up future spawns each time
        spawnInterval *= spawnAcceleration;
        spawnInterval = Mathf.Max(spawnInterval, minSpawnInterval);

        // Reinvoke with the new faster interval
        CancelInvoke(nameof(SpawnEnemy));
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }

    public void ReduceEnemyCount()
    {
        enemyCount--;
        if (enemyCount < 0)
            enemyCount = 0;
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            1f,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
