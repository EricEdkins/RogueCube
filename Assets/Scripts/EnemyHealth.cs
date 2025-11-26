using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private int xpReward = 0;

    private Renderer rend;
    private bool isDead = false; // Prevent double XP or duplicate destruction

    private void Start()
    {
        currentHealth = maxHealth;
        rend = GetComponent<Renderer>();
    }

    public void Initialize(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
        rend = GetComponent<Renderer>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; // already dead, ignore

        currentHealth -= amount;

        // Optional: quick flash effect
        if (rend != null)
            rend.material.color = Color.Lerp(Color.white, Color.red, 0.5f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        PlayerHandler player = Object.FindFirstObjectByType<PlayerHandler>();
        if (player != null)
            player.GainXP(xpReward);

        // 🔹 Let the spawner know an enemy was removed
        if (EnemySpawner.Instance != null)
            EnemySpawner.Instance.ReduceEnemyCount();

        Destroy(gameObject);
    }


}
