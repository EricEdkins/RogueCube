using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public static CoinSpawner Instance; // <— add this line

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private BoxCollider groundCollider;
    [SerializeField] private int coinAmount;
    public int coinCount;
    public int coinTopUpAmount = 15;
    

    void Awake()
    {
        Instance = this; // <— assign singleton reference
    }

    void Start()
    {
        for (int i = 0; i < coinAmount; i++)
        {
            Vector3 startPos = RandomPointInBounds(groundCollider.bounds);
            Instantiate(coinPrefab, startPos, Quaternion.identity);
            coinCount++;
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            1f,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    void Update()
    {
        if (coinCount < coinTopUpAmount)
        {
            Vector3 startPos = RandomPointInBounds(groundCollider.bounds);
            Instantiate(coinPrefab, startPos, Quaternion.identity);
            coinCount++;
        }
    }

    public void ReduceCoinCount()
    {
        coinCount--;
    }
}
