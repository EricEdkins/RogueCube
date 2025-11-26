using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float lifetime = 1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react if we hit an enemy
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); // only destroy when we hit an enemy
        }
    }
}
