using System;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class GoblinBossHealth : MonoBehaviour
{
    [Tooltip("Slider de la vida del enemigo.")]
    [SerializeField] private Slider healthSlider;

    private int maxHealth;
    private int currentHealth;
    private bool isDead;
    private bool gameOver;

    public static GoblinBossHealth Instance;

    public int CurrentHealth
    {
        get => currentHealth;
    }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Init();
    }

    void Init()
    {
        Reset();
    }
    
    private void Update()
    {
        if(MyGameManager.Instance.gameOver)
            GameOver();
    }
    
    void AddDamage(int damage)
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
        float startTime = Time.time;
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
    
    public void Reset()
    {
        healthSlider.gameObject.SetActive(true);
        maxHealth = 100;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        isDead = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            AddDamage(10);
        }
    }
    
    void Die()
    {
        if (isDead) return; 

        isDead = true;

        healthSlider.gameObject.SetActive(false);

        float deathDuration = 1f;

        transform.DOScale(new Vector3(0, 0, 0), deathDuration).SetEase(Ease.InFlash).OnComplete(() =>
        {
           // gameObject.SetActive(false);
           Destroy(gameObject);
        }).Play();
    }

    void GameOver()
    {
        if (gameOver) return;
        StopAllCoroutines();
        gameOver = true;
    }
}
