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
        [SerializeField] private float wallJumpForce;
        //private float wallRunTimer;

        [Header("Wall Detection")]
        [SerializeField] private float wallCheckDistance;
        [SerializeField] private float minJumpHeight;
        private RaycastHit leftWallHit;
        private RaycastHit rightWallHit;
        private bool wallLeft;
        private bool wallRight;

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

            if (inputManager.wallJumpInput) Jump();
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
            if ((wallLeft || wallRight) && inputManager.movementInput.y > 0 && AboveGround())
            {
                if (!playerMovement.isWallRunning)
                    StartWallRun();
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

            cameraController.UpdateFOV(fovMultiplier, fovTime);
        }

        private void WallRunningMovement()
        {
            cameraController.TiltCamera(wallLeft, tiltAmount);

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

            if (!(wallLeft && inputManager.movementInput.x > 0) && !(wallRight && inputManager.movementInput.x < 0))
                rb.AddForce(-wallNormal * 10f, ForceMode.Force);
        }

        private void StopWallRun()
        {
            playerMovement.isWallRunning = false;

            cameraController.ResetFOV(fovTime);
        }

        private void Jump()
        {
            if (!playerMovement.isWallRunning) return;

            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallJumpForce * (50 + rb.velocity.magnitude), ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallJumpForce * (50 + rb.velocity.magnitude), ForceMode.Force);
            }
        }

    }
}