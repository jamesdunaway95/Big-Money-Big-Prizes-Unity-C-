using UnityEngine;

namespace NoStackDev.BigMoney
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(GroundDetection))]
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody rb;
        private GroundDetection groundDetection;

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
        }

        private void Update()
        {
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
            }
        }
        #endregion

        public void Jump()
        {
            if (!groundDetection.isGrounded) return;

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}