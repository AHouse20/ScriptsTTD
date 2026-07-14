using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] Image healthBarImage;
    [SerializeField] Image healthBarSecondaryImage;
    [SerializeField] Animation healthBarOverlayAnimation;
    [SerializeField] HealthSystem healthSystem;

    private void Start()
    {
        healthSystem.OnDamaged += OnDamaged;
        UpdateHealthBar();
    }

    private void Update()
    {
        if (healthBarSecondaryImage.fillAmount != healthBarImage.fillAmount)
        {
            healthBarSecondaryImage.fillAmount = Mathf.Lerp(healthBarSecondaryImage.fillAmount, healthBarImage.fillAmount, Time.deltaTime +0.02f);
        }
    }

    private void OnDamaged(object sender, int e)
    {
        UpdateHealthBar();
        healthBarOverlayAnimation.Play();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
