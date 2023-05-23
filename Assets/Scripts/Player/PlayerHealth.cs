using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Tooltip("Slider de la vida del jugador.")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color damageColor;
    
    private int maxHealth;
    private int currentHealth;
    public int CurrentHealth
    {
        get => currentHealth;
    }

    void Start()
    {
        Instance = this;
        Init();
    }

    void Init()
    {
        Reset();
    }

    public void AddDamage(int damage)
    {
        currentHealth -= damage;
        
        Debug.Log(currentHealth);
        
        StartCoroutine(ShowDamageEffect());

        if (currentHealth <= 0)
        {
            MyGameManager.Instance.GameOver();
        }
    }
    
    IEnumerator ShowDamageEffect()
    {
        //healthSlider.image.color = damageColor;
        //yield return new WaitForSeconds(0.3f); 

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

        //healthSlider.image.color = normalColor;
    }

    public void Reset()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
