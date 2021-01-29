using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private float mouseFollowSensitivity = 1f;
    [SerializeField] private float mouseFollowSpeed = 2f;

    private Camera mainCamera;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineCameraOffset cameraOffset;

    private Vector2 currentOffset;
    private Vector2 desiredOffset;

    private void Awake() {
        mainCamera = GetComponentInChildren<Camera>();
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cameraOffset = virtualCamera.GetComponent<CinemachineCameraOffset>();
    }

    private void Update() {
        HandleMouseOffset();
    }

    private void HandleMouseOffset() {
        Vector2 mousePos = mainCamera.ScreenToViewportPoint(Input.mousePosition);
        Vector2 centeredMousePos = new Vector2(mousePos.x - 0.5f, mousePos.y - 0.5f);
        desiredOffset = centeredMousePos * mouseFollowSensitivity;
        currentOffset = Vector2.Lerp(currentOffset, desiredOffset, Time.deltaTime * mouseFollowSpeed);
        cameraOffset.m_Offset = currentOffset;
    }
}
