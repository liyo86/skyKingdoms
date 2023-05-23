using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            Destroy(gameObject);
        }
    }
}

