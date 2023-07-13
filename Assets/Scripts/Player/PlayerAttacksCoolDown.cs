using AI.Player_Controller;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttacksCoolDown : MonoBehaviour
{
    public Image shootImage;
    public Image defenseImage;
    public Image specialAttackImage;

    private IAttackCooldown attackCooldownController; // Referencia a la interfaz común

    private bool isDefenseCooldown = false;
    private bool isShootCooldown = false;
    private bool isSpecialAttackCooldown = false;

    private void Start()
    {
        // Obtener la referencia al controlador de enfriamientos apropiado
        if (GetComponent<BoyController>().gameObject.activeSelf)
        {
            attackCooldownController = GetComponent<BoyController>();
        }
        else if (GetComponent<AIController>().gameObject.activeSelf)
        {
            attackCooldownController = GetComponent<AIController>();
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        Defense();
        Shoot();
        SpecialAttack();
#endif
    }

    private void Defense()
    {
        if (attackCooldownController != null && attackCooldownController.IsDefenseCooldown)
        {
            OnDefenseCooldown();
            GetDefenseCooldownProgress();
        }
        else
        {
            defenseImage.fillAmount = 1f;
            isDefenseCooldown = false;
        }
    }

    private void OnDefenseCooldown()
    {
        if (isDefenseCooldown) return;
        defenseImage.fillAmount = 0f;
        isDefenseCooldown = true;
    }

    private void GetDefenseCooldownProgress()
    {
        defenseImage.fillAmount += 1.0f / attackCooldownController.DefenseCooldown * Time.deltaTime;
    }

    private void Shoot()
    {
        if (attackCooldownController != null && attackCooldownController.IsShootCooldown)
        {
            OnShootCooldown();
            GetShootCooldownProgress();
        }
        else
        {
            shootImage.fillAmount = 1f;
            isShootCooldown = false;
        }
    }

    private void OnShootCooldown()
    {
        if (isShootCooldown) return;
        shootImage.fillAmount = 0f;
        isShootCooldown = true;
    }

    private void GetShootCooldownProgress()
    {
        shootImage.fillAmount += 1.0f / attackCooldownController.ShootCooldown * Time.deltaTime;
    }

    private void SpecialAttack()
    {
        if (attackCooldownController != null && attackCooldownController.IsSpecialAttackCooldown)
        {
            OnSpecialAttackCooldown();
            GetSpecialAttackCooldownProgress();
        }
        else
        {
            specialAttackImage.fillAmount = 1f;
            isSpecialAttackCooldown = false;
        }
    }

    private void OnSpecialAttackCooldown()
    {
        if (isSpecialAttackCooldown) return;
        specialAttackImage.fillAmount = 0f;
        isSpecialAttackCooldown = true;
    }

    private void GetSpecialAttackCooldownProgress()
    {
        specialAttackImage.fillAmount += 1.0f / attackCooldownController.SpecialAttackCooldown * Time.deltaTime;
    }
}
