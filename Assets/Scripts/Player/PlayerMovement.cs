using System.Collections;
using UnityEngine;

namespace NoStackDev.BigMoney
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundDetection))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody rb;
        private CapsuleCollider capsuleCollider;
        private GroundDetection groundDetection;
        private InputManager inputManager;
        private CameraController cameraController;

        [Header("Movement")]
        [SerializeField] private Transform orientation;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float wallRunSpeed;

        private float moveSpeed;
        private float desiredMoveSpeed;
        private float lastDesiredMoveSpeed;

        [Tooltip("Changes how fast you speed Accelerate.")]
        public float speedIncreaseMultiplier;
        [Tooltip("Changes how much the slope angle affects your speed.")]
        public float slopeIncreaseMultipler;

        [Header("Jumping / Falling")]
        [SerializeField] private float airMultiplier;
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCooldown;
        private bool readyToJump = true;

        [Header("Sliding")]
        [SerializeField] private float slideSpeed;
        [SerializeField] private float slideMultiplier;

        [Header("Crouching")]
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float reducedHeight;
        private float originalHeight;

        [Header("Camera Settings")]
        [SerializeField] private bool cameraTilt;
        [SerializeField] private float tiltAmount;

        private Vector3 moveDirection;

        public MovementState state;
        public bool isCrouching = false;
        public bool isSliding = false;
        public bool isWallRunning = false;

        public float velocity; // DEBUG

        public enum MovementState
        {
            walking,
            sprinting,
            wallRunning,
            crouching,
            sliding,
            air
        }


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            capsuleCollider = GetComponentInChildren<CapsuleCollider>();
            groundDetection = GetComponent<GroundDetection>();
            inputManager = GetComponent<InputManager>();
            cameraController = GetComponent<CameraController>();

            rb.freezeRotation = true;

            originalHeight = capsuleCollider.height;
        }

        private void Update()
        {
            velocity = rb.velocity.magnitude; // DEBUG

            // Movement
            HandleMovementState();
            LimitVelocity();

            // Jumping
            if (inputManager.jumpInput && groundDetection.isGrounded && readyToJump) Jump();

            // Crouching
            if (inputManager.crouchInput) Crouch();
            if (!inputManager.crouchInput && isCrouching) StopCrouch();

            // Camera
            if (cameraTilt && inputManager.movementInput.x != 0) HandleCamera();
        }

        private void HandleCamera()
        {
            bool isLeft = inputManager.movementInput.x < 0;

            cameraController.TiltCamera(isLeft, tiltAmount);
        }

        private void FixedUpdate()
        {
            rb.useGravity = !groundDetection.OnSlope();

            HandleMovement();
        }

        #region Movement
        private void HandleMovementState()
        {
            if (isWallRunning)
            {
                state = MovementState.wallRunning;
                desiredMoveSpeed = wallRunSpeed;
            }
            else if (isSliding)
            {
                state = MovementState.sliding;

                if (groundDetection.OnSlope() && rb.velocity.y < 0.1f)
                {
                    desiredMoveSpeed = slideSpeed;
                } 
                else
                {
                    desiredMoveSpeed = sprintSpeed;
                }
            }
            else if (isCrouching)
            {
                state = MovementState.crouching;
                desiredMoveSpeed = crouchSpeed;
            }
            else if (groundDetection.isGrounded && inputManager.sprintInput)
            {
                state = MovementState.sprinting;
                desiredMoveSpeed = sprintSpeed;
            }
            else if (groundDetection.isGrounded)
            {
                state = MovementState.walking;
                desiredMoveSpeed = walkSpeed;
            }
            else
            {
                state = MovementState.air;
            }

            if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
            {
                StopAllCoroutines();
                StartCoroutine(LerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }

            lastDesiredMoveSpeed = desiredMoveSpeed;
        }

        private void HandleMovement()
        {
            moveDirection = orientation.forward * inputManager.movementInput.y + orientation.right * inputManager.movementInput.x;

            if (groundDetection.OnSlope())
            {
                rb.AddRelativeForce(groundDetection.GetSlopeMoveDirection(moveDirection) * moveSpeed * 7f, ForceMode.Force);
            }
            else if (groundDetection.isGrounded)
            {
                rb.AddRelativeForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else if (!groundDetection.isGrounded)
            {
                rb.AddRelativeForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
            }
        }

        private void LimitVelocity()
        {
            if (groundDetection.OnSlope())
            {
                if (rb.velocity.magnitude > maxSpeed)
                {
                    rb.velocity = rb.velocity.normalized * maxSpeed;
                }
            }
            else
            {
                Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                if (flatVelocity.magnitude > maxSpeed)
                {
                    Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
                    rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
                }
            }
        }

        private IEnumerator LerpMoveSpeed()
        {
            float time = 0;
            float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
            float startValue = moveSpeed;

            while (time < difference)
            {
                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

                if (groundDetection.OnSlope())
                {
                    float slopeAngle = Vector3.Angle(Vector3.up, groundDetection.slopeHit.normal);
                    float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                    time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultipler * slopeAngleIncrease; // Velocity increases faster on slopes and even faster on steeper slopes.
                }
                else
                {
                    time += Time.deltaTime * speedIncreaseMultiplier;
                }

                yield return null;
            }

            moveSpeed = desiredMoveSpeed;
        }
        #endregion

        #region Jumping
        public void Jump()
        {
            readyToJump = false;

            Invoke(nameof(ResetJump), jumpCooldown);

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            readyToJump = true;
        }
        #endregion

        #region Crouching
        private void Crouch()
        {
            isCrouching = true;

            capsuleCollider.height = reducedHeight;
            transform.localScale = new Vector3(transform.localScale.x, reducedHeight * 0.5f, transform.localScale.z); // Only temporary until a model and animation is added.

            rb.AddRelativeForce(Vector3.down * 5f, ForceMode.Force);
        }

        private void StopCrouch()
        {
            isCrouching = false;

            capsuleCollider.height = originalHeight;
            transform.localScale = new Vector3(transform.localScale.x, originalHeight * 0.5f, transform.localScale.z); // Only temporary until a model and animation is added.
        }
        #endregion
    }
}