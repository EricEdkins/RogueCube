using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerHandler ph = other.GetComponent<PlayerHandler>();
        if (ph != null)
        {
            ph.IncreasePoints();
            CoinSpawner.Instance.ReduceCoinCount(); // <— tell spawner
            Destroy(gameObject);
        }
    }
}
