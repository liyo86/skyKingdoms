using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTAttackPhaseOne : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    void Start()
    {
        transform.DOMove(_target.position, 2)
            .SetEase(Ease.Flash)
            .OnComplete(() => Destroy(gameObject));
    }
}
