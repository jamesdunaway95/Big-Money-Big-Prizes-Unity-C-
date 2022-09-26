using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerClimbing : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform orientation;
        private Rigidbody rb;
        private InputManager inputManager;
        private GroundDetection groundDetection;
        private PlayerMovement playerMovement;
        [SerializeField] LayerMask whatIsWall;

        [Header("Climbing")]
        [SerializeField] private float climbSpeed;
        [SerializeField] private float maxClimbTime;
        private float climbTimer;

        private bool climbing;

        [Header("Detection")]
        [SerializeField] private float detectionLength;
        [SerializeField] private float sphereCastRadius;
        [SerializeField] private float maxWallLookAngle;
        private float wallLookAngle;

        private RaycastHit frontWallHit;
        private bool wallFront;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();
            groundDetection = GetComponent<GroundDetection>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            WallCheck();
            StateMachine();

            if (climbing) ClimbingMovement();
        }

        private void StateMachine()
        {
            if (wallFront && inputManager.movementInput.y > 0 && wallLookAngle < maxWallLookAngle)
            {
                if (!climbing && climbTimer > 0) StartClimbing();

                // Timer
                if (climbTimer > 0) climbTimer -= Time.deltaTime;
                if (climbTimer < 0) StopClimbing();
            }

            // State 3 - none
            else
            {
                if (climbing) StopClimbing();
            }
        }

        private void WallCheck()
        {
            wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
            wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

            if (groundDetection.isGrounded)
            {
                climbTimer = maxClimbTime;
            }
        }
        
        private void StartClimbing()
        {
            climbing = true;
            playerMovement.isClimbing = true;
        }

        private void ClimbingMovement()
        {
            rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
        }

        private void StopClimbing()
        {
            climbing = false;
            playerMovement.isClimbing = false;
        }
    }
}
