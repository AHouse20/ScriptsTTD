using FMODUnity;
using System;
using UnityEngine;

public class ArrowProjectile : BaseProjectile
{

    [SerializeField] private Transform trailRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int speed;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private int boxColliderDelay = 20;


    private int lifeSpan;
    public override void OnHit()
    {

        trailRenderer.parent = null;
        base.OnHit();

    }

    public override void OnProjectileSpawned()
    {
        transform.forward = (targetPosition - transform.position).normalized;
        rb.isKinematic = false;
        rb.AddForce((targetPosition - transform.position).normalized * speed, ForceMode.Impulse);
        lifeSpan = 0;
    }

    private void FixedUpdate()
    {
        lifeSpan++;
        if (lifeSpan == boxColliderDelay)
        {
            boxCollider.enabled = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(hit) return;
        rb.isKinematic = true;
        boxCollider.enabled = false;
        transform.SetParent(collision.transform, true);
        Instantiate(impactVFX, collision.GetContact(0).point, transform.rotation);
        OnHit();
    }

}
