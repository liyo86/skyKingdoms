using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    [SerializeField] private Image HP;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color damageColor;
    
    private int maxHealth;
    private int currentHealth;
    public int CurrentHealth
    {
        get => currentHealth;
    }

    public int MaxHealth
    {
        get => maxHealth;
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
        float startValue = HP.fillAmount;
        float targetValue = currentHealth / 100f;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / (endTime - startTime);
            HP.fillAmount = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }
    }

    public void Reset()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        HP.fillAmount = 1f;
    }
}
