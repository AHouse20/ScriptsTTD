using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();
    }
}
