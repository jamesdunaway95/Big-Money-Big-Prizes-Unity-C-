using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerLook : MonoBehaviour
    {
        private CameraController cameraController;

        [SerializeField] private Transform orientation;

        [Header("Camera")]
        [SerializeField] private Transform normalCamera;
        [SerializeField] private float xSensitivity = 10f;
        [SerializeField] private float ySensitivity = 10f;
        [SerializeField] private bool invertY = false;

        [SerializeField] private float maxLookAngle = -90f;
        [SerializeField] private float minLookAngle = 80f;

        [HideInInspector] public Vector2 lookInput;

        private float multiplier =- 0.01f;

        private float xRotation;
        private float yRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Awake()
        {
            cameraController = GetComponent<CameraController>();
        }

        private void Update()
        {
            yRotation -= lookInput.x * xSensitivity * multiplier;
            if (invertY) { xRotation -= lookInput.y * ySensitivity * multiplier; }
            xRotation += lookInput.y * ySensitivity * multiplier;

            xRotation = Mathf.Clamp(xRotation, maxLookAngle, minLookAngle);

            normalCamera.rotation = Quaternion.Euler(xRotation, yRotation, cameraController.currentTilt);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}