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

        [Header("Climb Jumping")]
        [SerializeField] private float climbJumpUpForce;
        [SerializeField] private float climbJumpBackForce;
        [SerializeField] private int climbJumps;
        private int climbJumpsLeft;

        [Header("Detection")]
        [SerializeField] private float detectionLength;
        [SerializeField] private float sphereCastRadius;
        [SerializeField] private float maxWallLookAngle;
        [SerializeField] private float minWallNormalAngleChange;
        private float wallLookAngle;

        [Header("Exiting")]
        [SerializeField] private bool exitingWall;
        [SerializeField] private float exitWallTime;
        private float exitWallTimer;

        private RaycastHit frontWallHit;
        private bool wallFront;

        private Transform lastWall;
        private Vector3 lastWallNormal;

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

            if (climbing && !exitingWall) ClimbingMovement();
        }

        private void StateMachine()
        {
            if (wallFront && inputManager.movementInput.y > 0 && wallLookAngle < maxWallLookAngle && !exitingWall)
            {
                if (!climbing && climbTimer > 0) StartClimbing();

                // Timer
                if (climbTimer > 0) climbTimer -= Time.deltaTime;
                if (climbTimer < 0) StopClimbing();
            }
            // State 2 - exiting
            else if (exitingWall)
            {
                if (climbing) StopClimbing();

                if (exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
                if (exitWallTimer < 0)
                {
                    playerMovement.isExitingWallClimb = false;
                    exitingWall = false;
                }
            }

            // State 3 - none
            else
            {
                if (climbing) StopClimbing();
            }

            if (wallFront && inputManager.wallJumpInput && climbJumpsLeft > 0) ClimbJump();
        }

        private void WallCheck()
        {
            wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
            wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

            bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

            if ((wallFront && newWall) || groundDetection.isGrounded)
            {
                climbTimer = maxClimbTime;
                climbJumpsLeft = climbJumps;
            }
        }
        
        private void StartClimbing()
        {
            climbing = true;
            playerMovement.isClimbing = true;

            lastWall = frontWallHit.transform;
            lastWallNormal = frontWallHit.normal;
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

        private void ClimbJump()
        {
            playerMovement.isExitingWallClimb = true;
            exitingWall = true;
            exitWallTimer = exitWallTime;

            Vector3 forceToApply = transform.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(forceToApply, ForceMode.Impulse);

            climbJumpsLeft--;
        }
    }
}
