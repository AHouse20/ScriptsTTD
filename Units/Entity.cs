using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected GridPosition gridPosition;
    [SerializeField] protected bool isAttackable = true;
    [SerializeField] protected bool isTargetable = true;
    [SerializeField] protected bool blocksMovement;
    protected HealthSystem healthSystem;
    [SerializeField] private Transform centerOfMass;

    protected virtual void Update()
    {


    }

    protected virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    protected virtual void Start()
    {
        if (healthSystem) healthSystem.OnDeath += OnDeath;
    }

    protected virtual void OnDisable()
    {
        if (healthSystem) healthSystem.OnDeath -= OnDeath;
    }

    protected virtual void OnDeath(object sender, EventArgs e)
    {

    }

    public bool IsAttackable()
    {
        return isAttackable;
    }

    public bool IsTargetable()
    {
        return isTargetable;
    }

    public bool BlocksMovement()
    {
        return blocksMovement;
    }
    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public void Damage(int damageToDeal)
    {
        if (healthSystem) healthSystem.Damage(damageToDeal);
    }

    public Transform GetCenterOfMass()
    {
        return centerOfMass;
    }

}
