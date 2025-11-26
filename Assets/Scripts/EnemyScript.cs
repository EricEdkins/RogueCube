using TMPro;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;
    private Transform player;
    private int mergeCount = 0;
    private const int maxMerges = 5;

    private Renderer rend;
    private Color baseColor;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        baseColor = rend.material.color;

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 direction = (targetPos - transform.position).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHandler ph = other.GetComponent<PlayerHandler>();
        if (ph != null)
        {
            GameManager.Instance.GameOver();
            //Destroy(ph.gameObject);
            return;
        }

        // --- Merge logic between enemies ---
        EnemyScript otherEnemy = other.GetComponent<EnemyScript>();
        if (otherEnemy != null && otherEnemy != this)
        {
            // Ensure both haven't hit the merge limit
            if (mergeCount >= maxMerges || otherEnemy.mergeCount >= maxMerges)
                return;

            // Decide which one survives
            if (transform.localScale.magnitude >= otherEnemy.transform.localScale.magnitude)
            {
                Merge(otherEnemy);
                
            }
            else
            {
                otherEnemy.Merge(this);
            }
        }
    }

    private void Merge(EnemyScript other)
    {
        if (EnemySpawner.Instance != null)
            EnemySpawner.Instance.ReduceEnemyCount();

        Destroy(other.gameObject);

        transform.localScale *= 1.05f;
        moveSpeed *= 1.03f;

        mergeCount++;

        float t = (float)mergeCount / maxMerges;
        rend.material.color = Color.Lerp(baseColor, Color.red, t);
    }


}
