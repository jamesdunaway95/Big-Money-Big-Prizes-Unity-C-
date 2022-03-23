using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerLook : MonoBehaviour
    {
        private CameraController cameraController;
        private InputManager inputManager;

        [SerializeField] private Transform orientation;

        [Header("Camera Look")]
        [SerializeField] private Transform normalCamera;
        [SerializeField] private float xSensitivity = 10f;
        [SerializeField] private float ySensitivity = 10f;
        [SerializeField] private bool invertY = false;

        [SerializeField] private float maxLookAngle = -90f;
        [SerializeField] private float minLookAngle = 80f;

        [Header("Camera Tilt")]
        [SerializeField] private bool enableTilt;
        [SerializeField] private float tiltAmount;

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
            inputManager = GetComponent<InputManager>();
        }

        private void Update()
        {
            RotateCamera();

            if (enableTilt && inputManager.lookInput.x != 0) RotationTilt();
        }

        private void RotateCamera()
        {
            yRotation -= inputManager.lookInput.x * xSensitivity * multiplier;
            if (invertY) { xRotation -= inputManager.lookInput.y * ySensitivity * multiplier; }
            xRotation += inputManager.lookInput.y * ySensitivity * multiplier;

            xRotation = Mathf.Clamp(xRotation, maxLookAngle, minLookAngle);

            normalCamera.rotation = Quaternion.Euler(xRotation, yRotation, cameraController.currentTilt);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        private void RotationTilt()
        {
            bool isLeft = inputManager.lookInput.x > 0;

            cameraController.TiltCamera(isLeft, tiltAmount);
        }
    }
}