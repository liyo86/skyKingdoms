using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase1 : FSMBoss
{
    public override void Execute(Boss agent)
    {
        if(FlowerBossHealth.Instance.CurrentHealth <= 70)
            agent.ChangePhase(Boss.BossPhase.Phase2);
        else
            agent.JumpAndCreateShockwave();
    }
}
