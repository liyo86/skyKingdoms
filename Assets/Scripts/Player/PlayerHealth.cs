using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    [SerializeField] private Image HP;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color damageColor;

    private int currentHealth;
    private int MaxHealth { get; set; }

    private void Start()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        Reset();
    }

    public void AddDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            MyGameManager.Instance.GameOver();
        }
        else
        {
            StartCoroutine(ShowDamageEffect());
        }
    }

    
    private IEnumerator ShowDamageEffect()
    {
        var startTime = Time.time;
        var endTime = startTime + 0.5f;  
        var startValue = HP.fillAmount;
        var targetValue = (float)currentHealth / MaxHealth;

        while (Time.time < endTime)
        {
            var t = (Time.time - startTime) / (endTime - startTime);
            HP.fillAmount = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }
    }

    public void Reset()
    {
        MaxHealth = 100;
        currentHealth = MaxHealth;
        HP.fillAmount = 1f;
    }
}