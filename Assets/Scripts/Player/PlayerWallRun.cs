using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class PlayerWallRun : MonoBehaviour
    {
        private Rigidbody rb;
        private InputManager inputManager;
        private CameraController cameraController;

        [Header("Movement")]
        [SerializeField] private Transform orientation;

        [Header("Detection")]
        [Tooltip("How far away the wall can be when wall running.")]
        [SerializeField] private float wallDistance;
        [Tooltip("How high off the ground the player must be to wall run.")]
        [SerializeField] private float minimumJumpHeight;

        [Header("Wall Running")]
        [SerializeField] private float wallRunGravity;
        [SerializeField] private float wallJumpForce;
        public bool isWallRunning = false;

        [Header("Camera Settings")]
        [SerializeField] private float tiltAmount;
        [SerializeField] private float fovMultiplier;
        [SerializeField] private float fovTime;

        private bool wallLeft = false;
        private bool wallRight = false;

        private RaycastHit wallLeftHit;
        private RaycastHit wallRightHit;

        private bool CanWallRun()
        {
            return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputManager = GetComponent<InputManager>();
            cameraController = GetComponent<CameraController>();
        }

        private void Update()
        {
            CheckWall();
            HandleWallRun();

            if (inputManager.wallJumpInput) Jump();
        }

        private void HandleWallRun()
        {
            if (CanWallRun())
            {
                if (wallLeft)
                {
                    StartWallRun();
                }
                else if (wallRight)
                {
                    StartWallRun();
                }
                else
                {
                    StopWallRun();
                }
            }
            else
            {
                StopWallRun();
            }
        }

        private void Jump()
        {
            if (!isWallRunning) return;

            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + wallLeftHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddRelativeForce(wallRunJumpDirection * wallJumpForce * (50 + rb.velocity.magnitude), ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + wallRightHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddRelativeForce(wallRunJumpDirection * wallJumpForce * (50 + rb.velocity.magnitude), ForceMode.Force);
            }
        }

        private void CheckWall()
        {
            wallLeft = Physics.Raycast(transform.position, -orientation.right, out wallLeftHit, wallDistance);
            wallRight = Physics.Raycast(transform.position, orientation.right, out wallRightHit, wallDistance);
        }

        private void StartWallRun()
        {
            isWallRunning = true;
            rb.useGravity = false;

            rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

            cameraController.UpdateFOV(fovMultiplier, fovTime);
            cameraController.TiltCamera(wallLeft, tiltAmount);
        }

        private void StopWallRun()
        {
            isWallRunning = false;
            rb.useGravity = true;

            cameraController.ResetFOV(fovTime);
        }
    }
}