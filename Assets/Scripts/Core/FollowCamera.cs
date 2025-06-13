using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform target;

        [Header("Zoom Settings")]
        [SerializeField] float minZoomDistance = 2f;
        [SerializeField] float maxZoomDistance = 10f;
        [SerializeField] float zoomSpeed = 2f;
        [SerializeField] float zoomSmoothTime = 0.1f;

        [Header("Rotation Settings")]
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] float minPitch = -30f;
        [SerializeField] float maxPitch = 60f;

        [Header("Camera Offset")]
        [SerializeField] Vector3 targetOffset = Vector3.up;

        float currentZoom;
        float targetZoom;
        float zoomVelocity;

        float yaw = 0f;
        float pitch = 20f;

        void Start()
        {
            targetZoom = Mathf.Clamp((minZoomDistance + maxZoomDistance) * 0.5f, minZoomDistance, maxZoomDistance);
            currentZoom = targetZoom;
        }

        void LateUpdate()
        {
            HandleZoom();
            HandleRotation();

            Vector3 direction = Quaternion.Euler(pitch, yaw, 0) * Vector3.back;
            Vector3 desiredPosition = target.position + targetOffset + direction * currentZoom;
            transform.position = desiredPosition;
            transform.LookAt(target.position + targetOffset);
        }

        void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                targetZoom -= scroll * zoomSpeed;
                targetZoom = Mathf.Clamp(targetZoom, minZoomDistance, maxZoomDistance);
            }
            currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomVelocity, zoomSmoothTime);
        }

        void HandleRotation()
        {
            if (Input.GetMouseButton(1)) // Right mouse button for rotation
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                yaw += mouseX * rotationSpeed;
                pitch -= mouseY * rotationSpeed;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            }
        }
    }
}
