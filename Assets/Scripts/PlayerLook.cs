using System;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerLook : MonoBehaviour
    {
        private InputManager input;

        [SerializeField] private Transform orientation;

        [Header("Camera")]
        [SerializeField] private Transform normalCamera;
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
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            xLookInput = input.lookInput.x;
            yLookInput = input.lookInput.y;

            yRotation -= xLookInput * xSensitivity * multiplier;
            if (invertY) { xRotation -= yLookInput * ySensitivity * multiplier; }
            xRotation += yLookInput * ySensitivity * multiplier;

            xRotation = Mathf.Clamp(xRotation, maxLookAngle, minLookAngle);

            normalCamera.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}