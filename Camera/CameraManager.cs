using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;
    [SerializeField] private Transform defaultCameraTransform;

    private void OnEnable()
    {
        BaseAction.OnAnyActionStarted += OnAnyActionStarted;
        BaseAction.OnAnyActionComplete += OnAnyActionComplete;
    }

    private void OnDisable()
    {
        BaseAction.OnAnyActionStarted -= OnAnyActionStarted;
        BaseAction.OnAnyActionComplete -= OnAnyActionComplete;
    }

    private void OnAnyActionStarted(object sender, OnAnyActionStartedEventArgs e)
    {
        if (!e.useActionCamera) return;
        actionCameraGameObject.transform.SetParent(e.actionCameraTransform, false);
        ShowActionCamera();
    }

    private void OnAnyActionComplete(object sender, OnAnyActionCompleteEventArgs e)
    {
        if (!e.useActionCamera) return;
        HideActionCamera();
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }
}
