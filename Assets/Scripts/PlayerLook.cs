using UnityEngine;

namespace DunawayDevelopment.BigMoney
{
    public class PlayerLook : MonoBehaviour
    {
        // Components
        private InputManager inputManager;
        private Rigidbody rb;

        public Camera normalCamera;

        // Variables
        public static bool cursorLocked;

        [SerializeField] private float mouseSensitivity = 100f;
        [SerializeField] private int maxAngle = 90;
        [SerializeField] private int minAngle = -90;

        private float xInput;
        private float yInput;

        // Hidden Variables
        private float xRotation;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            xInput = inputManager.GetLookInput().x * mouseSensitivity * Time.fixedDeltaTime;
            yInput = inputManager.GetLookInput().y * mouseSensitivity * Time.deltaTime;
        }

        private void FixedUpdate()
        {
            Quaternion _adj = Quaternion.AngleAxis(xInput, Vector3.up);
            Quaternion _delta = transform.localRotation * _adj;
            rb.MoveRotation(_delta);
        }

        private void LateUpdate()
        {
            xRotation -= yInput;
            xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);

            normalCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
