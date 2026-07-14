using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;

    private List<ActionButtonUI> actionButtons = new();

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void OnSelectedActionChanged(object sender, ActionSelectedEventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void OnSelectedUnitChanged(object sender, UnitSelectedEventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void CreateUnitActionButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainer)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtons.Clear();

        if(UnitActionSystem.Instance.GetSelectedUnit() == null) return;

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach(BaseAction baseAction in selectedUnit.GetBaseActions())
        {
            Transform newActionButton = Instantiate(actionButtonPrefab, actionButtonContainer);
            ActionButtonUI actionButtonUI = newActionButton.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtons.Add(actionButtonUI);
        }
    }

    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in actionButtons)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }
}
