using Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttacksCoolDown : MonoBehaviour
{
    public Image shootImage;
    public Image defenseImage;
    public Image specialAttackImage;

    private bool isDefenseCoolDown = false;
    private bool isShootCoolDown = false;
    private bool isSpecialAttackCoolDown = false;

    void Update()
    {
        Defense();
        Shoot();
    }


    #region DEFENSE

    private void Defense()
    {
        //Defense
        if (BoyController.Instance.isDefenseCooldown)
        {
            OnDefenseCoolDown();
            GetDefenseCooldownProgress();
        }
        else if (!BoyController.Instance.isDefenseCooldown)
        {
            defenseImage.fillAmount = 1f;
            isDefenseCoolDown = false;
        }
    }
    private void OnDefenseCoolDown()
    {
        if (isDefenseCoolDown) return;
        defenseImage.fillAmount = 0f;
        isDefenseCoolDown = true;
    }
    
    private void GetDefenseCooldownProgress()
    {
        defenseImage.fillAmount += 1.0f / BoyController.Instance.defenseCooldown * Time.deltaTime;
    }
    #endregion
    
    #region SHOOT

    private void Shoot()
    {
        if (BoyController.Instance.isShootCooldown)
        {
            OnShootCoolDown();
            GetShootCooldownProgress();
        }
        else if (!BoyController.Instance.isShootCooldown)
        {
            shootImage.fillAmount = 1f;
            isShootCoolDown = false;
        }
    }
    
    private void OnShootCoolDown()
    {
        if (isShootCoolDown) return;
        shootImage.fillAmount = 0f;
        isShootCoolDown = true;
    }
    
    private void GetShootCooldownProgress()
    {
        shootImage.fillAmount += 1.0f / BoyController.Instance.shootCooldown * Time.deltaTime;
    }
    #endregion
    
    #region SPECIAL

    private void SpecialAttack()
    {
        if (BoyController.Instance.isSpecialAttackCooldown)
        {
            OnSpecialCoolDown();
            GetSpecialAttackCooldownProgress();
        }
        else if (!BoyController.Instance.isSpecialAttackCooldown)
        {
            specialAttackImage.fillAmount = 1f;
            isSpecialAttackCoolDown = false;
        }
    }
    
    
    private void OnSpecialCoolDown()
    {
        if (isSpecialAttackCoolDown) return;
        specialAttackImage.fillAmount = 0f;
        isSpecialAttackCoolDown = true;
    }
    
    private void GetSpecialAttackCooldownProgress()
    {
        specialAttackImage.fillAmount += 1.0f / BoyController.Instance.specialAttackCooldown * Time.deltaTime;
    }
    #endregion
}
