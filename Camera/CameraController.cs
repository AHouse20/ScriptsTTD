using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineFollow cinemachineFollow;

    InputAction moveCameraAction;
    InputAction rotateCameraAction;
    InputAction zoomCameraAction;

    float moveSpeed = 22f;
    float rotationSpeed = 100f;
    float zoomSpeed = 10f;

    private Vector3 targetFollowOffset;

    Vector3 cameraMoveDir;
    Vector3 cameraRotationVector = Vector3.zero;
    private Vector3 cameraZoomVector = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetFollowOffset = cinemachineFollow.FollowOffset;

        moveCameraAction = InputSystem.actions.FindAction("Move");
        rotateCameraAction = InputSystem.actions.FindAction("Rotate");
        zoomCameraAction = InputSystem.actions.FindAction("Zoom");

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        cameraMoveDir = moveCameraAction.ReadValue<Vector2>();
        Vector3 normalizedCameraMoveDir = transform.forward * cameraMoveDir.y + transform.right * cameraMoveDir.x;
        transform.position += normalizedCameraMoveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        cameraRotationVector.y = rotateCameraAction.ReadValue<float>();
        transform.eulerAngles += cameraRotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        cameraZoomVector.y = zoomCameraAction.ReadValue<float>();
        targetFollowOffset -= cameraZoomVector;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, 1f, 30f);
        cinemachineFollow.FollowOffset = Vector3.Lerp(cinemachineFollow.FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
