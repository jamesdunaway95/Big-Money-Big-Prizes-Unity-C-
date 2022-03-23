using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera normalCamera;

        [Header("Camera FOV")]
        [SerializeField] private float baseFov;


        [Header("Camera Tilt")]
        public float currentTilt;

        public void UpdateFOV(float fovMultiplier, float fovTime)
        {
            normalCamera.fieldOfView = Mathf.Lerp(normalCamera.fieldOfView, baseFov * fovMultiplier, fovTime * Time.deltaTime);
        }

        public void ResetFOV(float fovTime)
        {
            normalCamera.fieldOfView = Mathf.Lerp(normalCamera.fieldOfView, baseFov, fovTime * Time.deltaTime);
        }

        public void TiltCamera(bool isLeft, float tiltAmount, float tiltTime)
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

        public void ResetTilt(float tiltTime)
        {
            if (currentTilt == 0) return;

            currentTilt = Mathf.Lerp(currentTilt, 0, tiltTime * Time.deltaTime);
        }
    }
}