using FIMSpace.FProceduralAnimation;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private RagdollAnimator2 ragdollAnimator;
    private HealthSystem healthSystem;
    private RAF_DismembermentManager dismembermentManager;
    [SerializeField] private List<Transform> limbsToBreak = new();

    private void Awake()
    {
        ragdollAnimator = gameObject.GetComponentInChildren<RagdollAnimator2>();
        healthSystem = gameObject.GetComponent<HealthSystem>();
    }

    private void OnEnable()
    {
        healthSystem.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        healthSystem.OnDeath -= OnDeath;
    }

    private void OnDeath(object sender, EventArgs e)
    {
        ragdollAnimator.Handler.AnimatingMode = RagdollHandler.EAnimatingMode.Falling;
        if(limbsToBreak.Count > 0)
        {
            BreakLimbs();
        }
    }

    public void BreakLimbs()
    {
        dismembermentManager = ragdollAnimator.Handler.GetExtraFeature<RAF_DismembermentManager>();
        foreach (Transform limb in limbsToBreak)
        {
            RagdollChainBone bone = ragdollAnimator.User_GetBoneSetupByBoneName(limb.name);
            dismembermentManager.DismemberBone(bone, EDismemberType.Disconnect);
        }
    }
}
