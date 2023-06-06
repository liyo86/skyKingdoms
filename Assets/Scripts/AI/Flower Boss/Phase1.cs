using System.Collections;
using UnityEngine;

public class Phase1 : FSMBoss
{
    private float _timeBetweenJumps = 2f; // Tiempo entre saltos (segundos)
    private float _jumpTimer = 0f; // Temporizador para el salto
    private float jumpForce = 5f; // Fuerza del salto
    
    public override void Execute(Boss agent)
    {
        if(FlowerBossHealth.Instance.CurrentHealth <= 70)
            agent.ChangePhase(Boss.BossPhase.Phase2);
        else
            JumpAndCreateShockwave(agent);
    }
    
    private void JumpAndCreateShockwave(Boss agent)
    {
        _jumpTimer += Time.deltaTime;

        if (_jumpTimer >= _timeBetweenJumps)
        { 
            agent.bossRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            agent.StartCoroutine(WaitAndShoot(agent));
            _jumpTimer = 0;
        }
       
    }
    
    private IEnumerator WaitAndShoot(Boss agent)
    {
        yield return new WaitForSeconds(0.5f);
        
        if(agent.shockWaveParticle != null)
            agent.shockWaveParticle.Play();
        
        MyAudioManager.Instance.PlaySfx("bossAttack1SFX");
        
        Vector3 attackOnePosition = new Vector3(agent.transform.position.x, UnityEngine.Random.Range(0f, 2f), agent.transform.position.z - 6);
        
        Object.Instantiate(agent.AttackPhase1, attackOnePosition, Quaternion.identity);
    }
}
