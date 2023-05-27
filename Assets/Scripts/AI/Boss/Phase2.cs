using DG.Tweening;
using Player;
using UnityEngine;

public class Phase2 : FSMBoss
{
    private Vector3 originalPosition;
    private bool isFuryMode = false;
    private float jumpDuration = 2;
    private int jumpCount = 3; // Cantidad de saltos consecutivos
    private float jumpHeight = 2f; // Altura de cada salto

    public override void Execute(Boss agent)
    {
        Vector3 targetPosition = BoyController.Instance.transform.position;
        agent.transform.LookAt(targetPosition);
        FuryMode(agent.transform, targetPosition);
    }

    private void FuryMode(Transform targetTransform, Vector3 targetPosition)
    {
        if (!isFuryMode)
        {
            isFuryMode = true;
            originalPosition = targetTransform.position;
            JumpToTarget(targetTransform, targetPosition);
        }
    }

    private void JumpToTarget(Transform targetTransform, Vector3 targetPosition)
    {
        targetTransform.DOJump(targetPosition, jumpHeight, jumpCount, jumpDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => JumpBack(targetTransform))
            .SetId(targetTransform);
    }

    private void JumpBack(Transform targetTransform)
    {
        targetTransform.DOJump(originalPosition, jumpHeight, jumpCount, jumpDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => JumpToTarget(targetTransform, BoyController.Instance.transform.position))
            .SetId(targetTransform);
    }

    private void StopFuryMode(Transform targetTransform)
    {
        if (isFuryMode)
        {
            isFuryMode = false;
            targetTransform.DOKill();
        }
    }
}