using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class WallRun : MonoBehaviour
    {
        private Rigidbody rb;
        private CameraController cameraController;

        [Header("Movement")]
        [SerializeField] private Transform orientation;

        [Header("Detection")]
        [Tooltip("How far away the wall can be when wall running.")]
        [SerializeField] private float wallDistance = 0.6f;
        [Tooltip("How high off the ground the player must be to wall run.")]
        [SerializeField] private float minimumJumpHeight = 1.5f;

        [Header("Wall Running")]
        [SerializeField] private float wallRunGravity = 1f;
        [SerializeField] private float wallJumpForce = 12f;
        [SerializeField] private float wallRunFovMultiplier = 1.2f;
        public bool isWallRunning = false;

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
            cameraController = GetComponent<CameraController>();
        }

        private void Update()
        {
            CheckWall();
            HandleWallRun();
        }

        private void HandleWallRun()
        {
            if (CanWallRun())
            {
                if (wallLeft)
                {
                    Debug.Log("Wall running - left");
                    StartWallRun();
                }
                else if (wallRight)
                {
                    Debug.Log("Wall running - right");
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

        public void Jump()
        {
            if (!isWallRunning) return;

            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + wallLeftHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallJumpForce * (100 + rb.velocity.magnitude), ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + wallRightHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallJumpForce * (100 + rb.velocity.magnitude), ForceMode.Force);
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

            cameraController.UpdateFOV(wallRunFovMultiplier);
            cameraController.TiltCamera(wallLeft);
        }

        private void StopWallRun()
        {
            isWallRunning = false;
            rb.useGravity = true;

            cameraController.ResetFOV();
            cameraController.ResetTilt();
        }
    }
}
