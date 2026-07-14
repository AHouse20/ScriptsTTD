using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform arrowProjectilePrefab;
    [SerializeField] private Transform shootPoint;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnMovingChanged += OnMovingChanged;
        }
        if (TryGetComponent<AttackAction>(out AttackAction attackAction))
        {
            attackAction.OnStateChanged += OnStateChanged;
        }
    }

    private void OnStateChanged(object sender, AttackActionArgs args)
    {
        switch (args.state)
        {
            case AttackAction.State.Windup:
                animator.SetTrigger("Aim");
                break;
            case AttackAction.State.Active:
                Fire(args.targetUnit, args.sourceUnit, args.onHit);
                animator.SetTrigger("Activate");
                break;
            case AttackAction.State.Backswing:
                animator.SetTrigger("Finished");
                break;
            case AttackAction.State.Inactive:
                animator.SetTrigger("Cancel");
                break;
        }
    }

    private void OnMovingChanged(object sender, bool e)
    {
        animator.SetBool("IsMoving", e);
    }

    private void Fire(Unit targetUnit, Unit sourceUnit, Action onHit)
    {

        if (shootPoint != null)
        {
            Transform arrowTransform = Instantiate(arrowProjectilePrefab, shootPoint.position, Quaternion.identity);
            arrowTransform.forward = (targetUnit.GetCenterOfMass().position - shootPoint.position).normalized;
            arrowTransform.GetComponent<ArrowProjectile>().Setup(targetUnit.GetCenterOfMass(), onHit);
        }
    }
}
