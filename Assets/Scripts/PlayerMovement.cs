using System;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(InputManager))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody rb;
        private InputManager input;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float moveMultiplier = 10f;
        [SerializeField] private float airMultiplier = 10f;
        [SerializeField] private float jumpForce = 5f;

        [Header("Ground Detection")]
        [SerializeField] private float playerHeight = 2f;
        [SerializeField] private bool isGrounded;

        [Header("Physics")]
        [SerializeField] private float groundDrag = 6f;
        [SerializeField] private float airDrag = 1f;

        private float horizontalMovement;
        private float verticalMovement;

        private Vector3 moveDirection;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            input = GetComponent<InputManager>();
        }

        private void Start()
        {
            rb.freezeRotation = true;
        }

        private void Update()
        {
            HandleGroundDetection();
            HandleInput();
            HandleDrag();

            if (input.jumpInput && isGrounded)
            {
                HandleJump();
            }
        }

        private void HandleInput()
        {
            horizontalMovement = input.movementInput.x;
            verticalMovement = input.movementInput.y;

            moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
        }

        private void HandleDrag()
        {
            if (isGrounded) 
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = airDrag;
            }

        }

        private void HandleGroundDetection()
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f);
        }

        private void HandleJump()
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }


        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (isGrounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Acceleration);
            }
        }
    }
}