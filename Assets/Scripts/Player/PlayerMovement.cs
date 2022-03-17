using UnityEngine;

namespace NoStackDev.BigMoney
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundDetection))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody rb;
        private GroundDetection groundDetection;
<<<<<<< HEAD
        private InputManager inputManager;
        private CapsuleCollider capsuleCollider;

        [Header("Movement")]
        [SerializeField] private Transform orientation;
        [SerializeField] private float maxSpeed = 12f;
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float walkSpeed = 8f;
        [SerializeField] private float sprintSpeed = 13f;

        [SerializeField] private float moveMultiplier = 10f;
        [SerializeField] private float slopeMultiplier = 9f;

        [Header("Jumping / Falling")]
        [SerializeField] private float airMultiplier = 2.75f;
        [SerializeField] private float jumpForce = 16f;
        [SerializeField] private float jumpCooldown;
        private bool readyToJump = true;

        [Header("Sliding")]
        [SerializeField] private float slideMultiplier;
        public bool isSliding = false;

        [Header("Crouching")]
        [SerializeField] private float crouchSpeed;
        private float originalHeight;
        [SerializeField] private float reducedHeight;
        public bool isCrouching = false;


        [HideInInspector] public Vector2 movementInput;
        private Vector3 moveDirection;

        private Vector3 slopeMoveDirection;

        public MovementState state;

        public enum MovementState
        {
            walking,
            sprinting,
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

            rb.freezeRotation = true;

            originalHeight = capsuleCollider.height;
=======

        [Header("Movement")]
        [SerializeField] private Transform orientation;
        public float maxSpeed = 22f;
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float moveMultiplier = 10f;
        [SerializeField] private float airMultiplier = 2.75f;
        [SerializeField] private float jumpForce = 16f;
        [HideInInspector] public Vector3 sumOfAppliedAccelerations;

        [HideInInspector] public Vector2 movementInput;
        [HideInInspector] public Vector3 moveDirection;
        private Vector3 slopeMoveDirection;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            groundDetection = GetComponent<GroundDetection>();

            rb.freezeRotation = true;
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
        }

        private void Update()
        {
<<<<<<< HEAD
            HandleMovementState();
            LimitVelocity();

            if (inputManager.jumpInput && groundDetection.isGrounded && readyToJump) Jump();
            if (inputManager.crouchInput) Crouch();
            if (!inputManager.crouchInput && isCrouching) StopCrouch();  
        }

        private void FixedUpdate()
        {
            rb.useGravity = !groundDetection.OnSlope();

            HandleMovement();
        }

        #region Movement
        private void HandleMovementState()
        {
            if (isCrouching)
            {
                state = MovementState.crouching;
                moveSpeed = crouchSpeed;
            }
            else if (isSliding)
            {
                state = MovementState.sliding;
            }
            else if (groundDetection.isGrounded && inputManager.sprintInput)
            {
                state = MovementState.sprinting;
                moveSpeed = sprintSpeed;
            }
            else if (groundDetection.isGrounded)
            {
                state = MovementState.walking;
                moveSpeed = walkSpeed;
            }
            else
            {
                state = MovementState.air;
            }
        }

        private void HandleMovement()
        {
            if (isSliding)
            {
                moveDirection = orientation.right * movementInput.x;
                rb.AddRelativeForce(moveDirection.normalized * moveSpeed * slideMultiplier, ForceMode.Force);
            }
            else if (groundDetection.OnSlope())
            {
                moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;
                rb.AddRelativeForce(groundDetection.GetSlopeMoveDirection(moveDirection) * moveSpeed * slopeMultiplier, ForceMode.Force);
            }
            else if (groundDetection.isGrounded)
            {
                moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;
                rb.AddRelativeForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Force);
            }
            else if (!groundDetection.isGrounded)
            {
                moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;
                rb.AddRelativeForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
            }
        }

        private void LimitVelocity()
        {
            if (isSliding) return;

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
=======
            moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;

            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, groundDetection.SlopeHit().normal);
        }

        #region Movement
        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (GetComponent<PlayerSlide>().isSliding) return;

            if (groundDetection.isGrounded && !groundDetection.OnSlope())
            {
                Debug.Log("On flat ground");
                sumOfAppliedAccelerations = moveDirection.normalized * moveSpeed * moveMultiplier;
                rb.AddForce(sumOfAppliedAccelerations, ForceMode.Force);
            }
            else if (groundDetection.isGrounded && groundDetection.OnSlope())
            {
                Debug.Log("On regular slope");
                sumOfAppliedAccelerations = slopeMoveDirection.normalized * moveSpeed * moveMultiplier;
                rb.AddForce(sumOfAppliedAccelerations, ForceMode.Acceleration);
            }
            else
            {
                Debug.Log("In the air");
                sumOfAppliedAccelerations = moveDirection.normalized * moveSpeed * airMultiplier;
                rb.AddForce(sumOfAppliedAccelerations, ForceMode.Acceleration);
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
            }
        }
        #endregion

<<<<<<< HEAD
        #region Jumping
        public void Jump()
        {
            readyToJump = false;

            Invoke(nameof(ResetJump), jumpCooldown);
=======
        public void Jump()
        {
            if (!groundDetection.isGrounded) return;
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
<<<<<<< HEAD

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
=======
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04
    }
}