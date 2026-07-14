using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class ScreenShake : SingletonMonobehaviour<ScreenShake>
{
    private CinemachineImpulseSource cinemachineImpulseSource;


    protected override void Awake()
    {
        base.Awake();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    [Button]
    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulseWithForce(intensity);
    }
}
