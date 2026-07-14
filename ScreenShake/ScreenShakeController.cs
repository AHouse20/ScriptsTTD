using System;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    private void OnEnable()
    {
        BaseAction.OnAnyActionImpact += OnAnyActionImpact;
    }

    private void OnDisable()
    {
        BaseAction.OnAnyActionImpact -= OnAnyActionImpact;
    }

    private void OnAnyActionImpact(object sender, BaseActionImpactArgs e)
    {
        ScreenShake.Instance.Shake(e.screenShakeAmount);
    }
}
