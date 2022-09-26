using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerWallRun : MonoBehaviour
    {
        [Header("References")]
        private Rigidbody rb;
        private InputManager inputManager;
        private PlayerMovement playerMovement;
        private CameraController cameraController;
        [SerializeField] private Transform orientation;

        [Header("Layers")]
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private LayerMask groundLayer;

        [Header("Wall Running")]
        [SerializeField] private float wallRunForce;
        [SerializeField] private float wallClimbSpeed;
        [SerializeField] private float maxWallRunTime;
        private float wallRunTimer;

        [Header("Wall Jumping")]
        [SerializeField] private float wallJumpUpForce;
        [SerializeField] private float wallJumpSideForce;

        [Header("Wall Detection")]
        [SerializeField] private float wallCheckDistance;
        [SerializeField] private float minJumpHeight;
        private RaycastHit leftWallHit;
        private RaycastHit rightWallHit;
        private bool wallLeft;
        private bool wallRight;

        [Header("Exiting Wall")]
        [SerializeField] private bool exitingWall;
        [SerializeField] private float exitWallTime;
        private float exitWallTimer;

        [Header("Gravity")]
        [SerializeField] private bool useGravity;
        [SerializeField] private float gravityCounterForce;

        [Header("Camera Settings")]
        [SerializeField] private float tiltAmount;
        [SerializeField] private float fovMultiplier;
        [SerializeField] private float fovTime;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();
            playerMovement = GetComponent<PlayerMovement>();
            cameraController = GetComponent<CameraController>();
        }

        private void Update()
        {
            CheckForWall();
            StateMachine();
        }

        private void FixedUpdate()
        {
            if (playerMovement.isWallRunning) WallRunningMovement();
        }

        private void CheckForWall()
        {
            wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wallLayer);
            wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wallLayer);
        }

        private bool AboveGround()
        {
            return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, groundLayer);
        }

        private void StateMachine()
        {
            // State 1 - Wall Running
            if ((wallLeft || wallRight) && inputManager.movementInput.y > 0 && AboveGround() && !exitingWall)
            {
                if (!playerMovement.isWallRunning)
                    StartWallRun();

                // Wallrun timer
                if (wallRunTimer > 0)
                {
                    wallRunTimer -= Time.deltaTime;
                }

                if (wallRunTimer <= 0 && playerMovement.isWallRunning)
                {
                    exitingWall = true;
                    exitWallTimer = exitWallTime;
                }

                // Wall jump
                if (inputManager.jumpInput) WallJump();
            }
            else if (exitingWall)
            {
                if (playerMovement.isWallRunning)
                {
                    StopWallRun();
                }

                if (exitWallTimer > 0)
                {
                    exitWallTimer -= Time.deltaTime;
                }

                if (exitWallTimer <= 0)
                {
                    exitingWall = false;
                }
            }
            else
            {
                if (playerMovement.isWallRunning)
                    StopWallRun();
            }
        }

        private void StartWallRun()
        {
            playerMovement.isWallRunning = true;

            wallRunTimer = maxWallRunTime;

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            cameraController.UpdateFOV(fovMultiplier, fovTime);
        }

        private void WallRunningMovement()
        {
            cameraController.TiltCamera(wallLeft, tiltAmount);

            rb.useGravity = useGravity;

            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
            Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

            // Wall run in direction the player is facing
            if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
                wallForward = -wallForward;

            // Forwards force
            rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

            // Upwards force (wall climbing)
            if (inputManager.sprintInput)
                rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);

            // Push to wall force
            if (!(wallLeft && inputManager.movementInput.x > 0) && !(wallRight && inputManager.movementInput.x < 0))
                rb.AddForce(-wallNormal * 10f, ForceMode.Force);

            if (useGravity && !exitingWall)
            {
                rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
            }
        }

        private void StopWallRun()
        {
            playerMovement.isWallRunning = false;

            cameraController.ResetFOV(fovTime);
        }

        private void WallJump()
        {
            // Enter exiting wall state
            exitingWall = true;
            exitWallTimer = exitWallTime;

            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

            Vector3 foreceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

            // Reset Y Velocity and Add force
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(foreceToApply, ForceMode.Impulse);
        }
    }
}