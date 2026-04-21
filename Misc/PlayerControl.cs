using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerControl : SingletonMonobehaviour<PlayerControl>
{
    public PlayerInput playerInput;
    public UnityEvent cancelEvent = new UnityEvent();

    public void Cancel()
    {
        cancelEvent.Invoke();
    }
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }
}
