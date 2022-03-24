using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera normalCamera;

        [Header("Camera FOV")]
        [SerializeField] private float baseFov;


        [Header("Camera Tilt")]
        public float currentTilt; // Debugging
        public float tiltTime;
        public float resetTiltTime;

        private void Update()
        {
            ResetTilt();
        }

        public void UpdateFOV(float fovMultiplier, float fovTime)
        {
            normalCamera.fieldOfView = Mathf.Lerp(normalCamera.fieldOfView, baseFov * fovMultiplier, fovTime * Time.deltaTime);
        }

        public void ResetFOV(float fovTime)
        {
            normalCamera.fieldOfView = Mathf.Lerp(normalCamera.fieldOfView, baseFov, fovTime * Time.deltaTime);
        }

        public void TiltCamera(bool isLeft, float tiltAmount)
        {
            if (isLeft)
            {
                currentTilt = Mathf.Lerp(currentTilt, -tiltAmount, tiltTime * Time.deltaTime);
            }
            else if (!isLeft)
            {
                currentTilt = Mathf.Lerp(currentTilt, tiltAmount, tiltTime * Time.deltaTime);
            }
        }

        private void ResetTilt()
        {
            if (currentTilt < 0.01f && currentTilt > -0.01f) return;

            currentTilt = Mathf.Lerp(currentTilt, 0, resetTiltTime * Time.deltaTime);
        }
    }
}