using FIMSpace.FProceduralAnimation;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

public class BreakLimb : MonoBehaviour
{
    [SerializeField] private RAF_DismembermentManager dismembermentManager;
    [SerializeField] private RagdollAnimator2 ragdollAnimator;
    [SerializeField] private RagdollChainBone bone;
    [SerializeField] private List<Transform> limbsToBreak;
    [SerializeField] private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponentInParent<HealthSystem>();
    }

    private void OnEnable()
    {
        if(healthSystem != null) healthSystem.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        if (healthSystem != null) healthSystem.OnDeath -= OnDeath;
    }

    private void OnDeath(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    [Button]
    public void BreakThisLimb()
    {
        dismembermentManager = ragdollAnimator.Handler.GetExtraFeature<RAF_DismembermentManager>();
        foreach (Transform limb in limbsToBreak)
        {
            bone = ragdollAnimator.User_GetBoneSetupByBoneName(limb.name);
            dismembermentManager.DismemberBone(bone, EDismemberType.Disconnect);
        }
    }
}
