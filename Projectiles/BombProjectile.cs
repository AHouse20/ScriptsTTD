using UnityEngine;

public class BombProjectile : BaseProjectile
{
    [SerializeField] private float moveSpeed = 25f;
    private const float power = 40;
    private const float radius = 10;
    [SerializeField] private AnimationCurve arcYCurve;
    [SerializeField] private float arcYCurveMagnitude;
    private float totalDistance;
    private Vector3 positionXZ;

    private void Awake()
    {
        positionXZ = transform.position;
    }
    public override void OnHit()
    {
        Instantiate(impactVFX, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            if (hit.attachedRigidbody)
            {
                hit.attachedRigidbody.AddExplosionForce(power, transform.position, radius, 0.5f, ForceMode.Impulse);
            }
        }
        base.OnHit();
        Destroy(gameObject);
    }

    public override void OnProjectileSpawned()
    {
        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }

    private void Update()
    {
        if (hit) return;
        Vector3 moveDir = (targetPosition - positionXZ).normalized;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float positionY = arcYCurve.Evaluate(distanceNormalized) * arcYCurveMagnitude;
        positionXZ += (moveSpeed * totalDistance)* Time.deltaTime * moveDir;

        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        if (distanceNormalized > 0.98f)
        {
            OnHit();
        }
    }
}
