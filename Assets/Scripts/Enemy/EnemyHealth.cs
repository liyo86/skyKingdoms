using DG.Tweening;
using Managers;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth Instance;
    [SerializeField] private GameObject hitParticle;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            MyLevelManager.Instance.enemyCount++;
            
            hitParticle.SetActive(true);
            
            other.gameObject.SetActive(false);
            
            transform.DOScale(Vector3.zero, 1).SetEase(Ease.InBounce).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}

