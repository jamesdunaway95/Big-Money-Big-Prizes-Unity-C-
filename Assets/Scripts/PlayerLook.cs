using System;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerLook : MonoBehaviour
    {
        // Components
        private InputManager input;
        private Camera normalCamera;

        // Variables
        [Header("Camera")]
        [SerializeField] private float xSensitivity = 10f;
        [SerializeField] private float ySensitivity = 10f;
        [SerializeField] private bool invertY = false;

        [SerializeField] private float maxLookAngle = -90f;
        [SerializeField] private float minLookAngle = 80f;

        private float xLookInput;
        private float yLookInput;

        private float multiplier =- 0.01f;

        private float xRotation;
        private float yRotation;

        private void Awake()
        {
            input = GetComponent<InputManager>();
            normalCamera = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleInput();

            normalCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        private void HandleInput()
        {
            xLookInput = input.lookInput.x;
            yLookInput = input.lookInput.y;

            yRotation -= xLookInput * xSensitivity * multiplier;
            if (invertY) { xRotation -= yLookInput * ySensitivity * multiplier; }
            xRotation += yLookInput * ySensitivity * multiplier;

            xRotation = Mathf.Clamp(xRotation, maxLookAngle, minLookAngle);
        }
    }
}