using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : SingletonMonobehaviour<CursorManager>
{

    [SerializeField] private GameObject worldVisual;
    [SerializeField] private LayerMask layerMask;
    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance.layerMask);
        return raycastHit.point;
    }

    public static Transform GetTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance.layerMask);
        return raycastHit.transform;
    }
}
