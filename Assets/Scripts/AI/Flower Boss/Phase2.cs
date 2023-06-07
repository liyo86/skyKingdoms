using DG.Tweening;
using Player;
using UnityEngine;

public class Phase2 : FSMBoss
{
    private Vector3 originalPosition;
    private bool isFuryMode = false;
    private float jumpDuration = 2f;
    private int jumpCount = 3; // Cantidad de saltos consecutivos
    private float jumpHeight = 2f; // Altura de cada salto
    private float waitTime = 2f; // Tiempo de espera después de tocar al jugador

    private bool isWaiting = false;
    private float waitTimer = 0f;

    public override void Execute(Boss agent)
    {
        Vector3 targetPosition = BoyController.Instance.transform.position;
        agent.transform.LookAt(targetPosition);
        
        FuryMode(agent.transform, targetPosition);

        if (HasCollidedWithPlayer(agent.transform))
        {
            StopFuryMode(agent.transform);
            WaitBeforeContinuing();
        }

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                JumpToTarget(agent.transform, BoyController.Instance.transform.position);
            }
        }
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
            .OnComplete(WaitBeforeContinuing)
            .SetId(targetTransform);
    }

    private void StopFuryMode(Transform targetTransform)
    {
        if (isFuryMode)
        {
            isFuryMode = false;
            targetTransform.DOKill();
            targetTransform.position = originalPosition;
        }
    }

    private void WaitBeforeContinuing()
    {
        isWaiting = true;
        waitTimer = 0f;
    }

    private bool HasCollidedWithPlayer(Transform agentTransform)
    {
        Collider agentCollider = agentTransform.GetComponent<Collider>();
        Collider playerCollider = BoyController.Instance.GetComponent<Collider>();

        if (agentCollider != null && playerCollider != null)
        {
            return agentCollider.bounds.Intersects(playerCollider.bounds);
        }

        return false;
    }
}
