using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerLook : MonoBehaviour
    {
        // Components
        private InputManager inputManager;
        private Rigidbody rb;


        [Header("Camera Settings")]
        [Tooltip("This is the default camera for rendering the world.")]
        public Camera normalCamera;

        // Variables
        [SerializeField] private float mouseSensitivity = 100f;
        [SerializeField] private int maxAngle = 90;
        [SerializeField] private int minAngle = -90;
        [SerializeField] private float baseFov = 70f;
        [SerializeField] private float sprintFovModifier = 1.25f;
        [SerializeField] private float fovTransitionSpeed = 8f;

        // Hidden Variables
        private float xRotation;
        private float xInput;
        private float yInput;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // TODO: May look into a better method, seems messy to get input from input class then send it where it needs to go.
        // Input should be read in update
        private void Update()
        {
            xInput = inputManager.lookInput.x * mouseSensitivity * Time.fixedDeltaTime;
            yInput = inputManager.lookInput.y * mouseSensitivity * Time.deltaTime;

            HandleFOV();
        }

        // Rigidbodies and physics should be updated in Fixed Update
        // Rotate player based on mouse delta
        private void FixedUpdate()
        {
            Quaternion _adj = Quaternion.AngleAxis(xInput, Vector3.up);
            Quaternion _delta = transform.localRotation * _adj;
            rb.MoveRotation(_delta);
        }
        
        // Camera should be moved in LateUpdate (supposedly)
        // Rotate camera up and down based on mouse delta
        private void LateUpdate()
        {
            xRotation -= yInput;
            xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);

            normalCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // FIXME: This is kinda ugly. Also I need to think of a better solution to call these methods. This should only be called if inputManager.isSprinting is changed.
        private void HandleFOV()
        {
            if (inputManager.sprintInput)
            {
                normalCamera.fieldOfView = Mathf.Lerp(normalCamera.fieldOfView, baseFov * sprintFovModifier, Time.deltaTime * fovTransitionSpeed);
            }
            else
            {
                normalCamera.fieldOfView = Mathf.Lerp(normalCamera.fieldOfView, baseFov, Time.deltaTime * fovTransitionSpeed);
            }
        }
    }
}
