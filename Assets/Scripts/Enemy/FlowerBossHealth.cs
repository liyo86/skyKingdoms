using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FlowerBossHealth : MonoBehaviour
{
    [Tooltip("Slider de la vida del enemigo.")]
    [SerializeField] private Slider healthSlider;

    private int maxHealth;
    private int currentHealth;
    private bool isDead;
    private bool gameOver;
    

    public int CurrentHealth
    {
        get => currentHealth;
    }


    public static FlowerBossHealth Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Init();
    }

    private void Update()
    {
        if(MyGameManager.Instance.gameOver)
            GameOver();
    }

    private void Init()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        isDead = false;
    }
    
    public void AddDamage(int damage)
    {
        currentHealth -= damage;

        StartCoroutine(ShowDamageEffect());

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
    
    IEnumerator ShowDamageEffect()
    {
        float startTime = Time.deltaTime;
        float endTime = startTime + 0.5f;  
        float startValue = healthSlider.value;
        float targetValue = currentHealth;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / (endTime - startTime);
            healthSlider.value = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }
    }
    void Die()
    {
        if (isDead) return; 

        isDead = true;

        float deathDuration = 1f;

        transform.DOScale(new Vector3(0, 0, 0), deathDuration).SetEase(Ease.InFlash).OnComplete(() =>
        {
            gameObject.SetActive(false);
            
        }).Play();
    }

    void GameOver()
    {
        if (gameOver) return;
        StopAllCoroutines();
        gameOver = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            AddDamage(10);
        }
    }
}
