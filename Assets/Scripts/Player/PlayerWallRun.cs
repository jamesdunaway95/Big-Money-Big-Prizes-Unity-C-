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
                    Debug.Log("left wall run");
                    StartWallRun();
                }
                else if (wallRight)
                {
                    Debug.Log("right wall run");
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
                Debug.Log("Jumped off left wall");
                Vector3 wallRunJumpDirection = transform.up + wallLeftHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
<<<<<<< HEAD:Assets/Scripts/Player/PlayerWallRun.cs
                rb.AddForce(wallRunJumpDirection * wallJumpForce * (75 + rb.velocity.magnitude * 2), ForceMode.Force);
=======
                rb.AddForce(wallRunJumpDirection * wallJumpForce * (50 + rb.velocity.magnitude * 3), ForceMode.Force);
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04:Assets/Scripts/WallRun.cs
            }
            else if (wallRight)
            {
                Debug.Log("Jumped off right wall");
                Vector3 wallRunJumpDirection = transform.up + wallRightHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
<<<<<<< HEAD:Assets/Scripts/Player/PlayerWallRun.cs
                rb.AddForce(wallRunJumpDirection * wallJumpForce * (75 + rb.velocity.magnitude * 2), ForceMode.Force);
=======
                rb.AddForce(wallRunJumpDirection * wallJumpForce * (50 + rb.velocity.magnitude * 3), ForceMode.Force);
>>>>>>> 2babb41109e75060e8ad637adf3539a9536d8b04:Assets/Scripts/WallRun.cs
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
