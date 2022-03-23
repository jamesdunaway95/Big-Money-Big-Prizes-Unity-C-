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
        [SerializeField] private float maxWallRunTime;
        [SerializeField] private float wallRunGravity;
        [SerializeField] private float wallJumpForce;
        //private float wallRunTimer;
        private float horizontalInput;
        private float verticalInput;

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
            horizontalInput = inputManager.movementInput.x;
            verticalInput = inputManager.movementInput.y;

            // State 1 - Wall Running
            if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
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
            rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            cameraController.TiltCamera(wallLeft, tiltAmount);

            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
            Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

            if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
                wallForward = -wallForward;

            rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
            rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

            if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
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