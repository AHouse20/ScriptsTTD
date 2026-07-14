using FMODUnity;
using System;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected EventReference impactSound;
    [SerializeField] protected Transform impactVFX;
    protected Vector3 targetPosition;
    protected Transform targetTransform;
    protected bool hit;
    protected Action onHit;
    public virtual void Setup(Transform targetTransform, Action onHit = null)
    {
        this.onHit = onHit;
        this.targetPosition = targetTransform.position;
        this.targetTransform = targetTransform;
        OnProjectileSpawned();
    }

    public virtual void Setup(GridPosition targetGridPosition, Action onHit = null)
    {
        this.onHit = onHit;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        OnProjectileSpawned();
    }

    public abstract void OnProjectileSpawned();
    public virtual void OnHit()
    {
        hit = true;
        if (!impactSound.IsNull) AudioManager.Instance.PlayOneShot(impactSound, transform.position);
        onHit?.Invoke();
    }
}
