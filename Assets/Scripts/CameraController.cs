using UnityEngine;

namespace NoStackDev.BigMoney
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera normalCamera;

        [Header("Camera FOV")]
        [SerializeField] private float baseFov = 80f;
        [SerializeField] private float fovTime = 8f;


        [Header("Camera Tilt")]
        [HideInInspector] public float currentTilt;
        [SerializeField] private float tiltAmount = 15f;
        [SerializeField] private float tiltTime = 5f;

        public void UpdateFOV(float fovMultiplier)
        {
            normalCamera.fieldOfView = Mathf.Lerp(normalCamera.fieldOfView, baseFov * fovMultiplier, fovTime * Time.deltaTime);
        }

        public void ResetFOV()
        {
            normalCamera.fieldOfView = Mathf.Lerp(normalCamera.fieldOfView, baseFov, fovTime * Time.deltaTime);
        }

        public void TiltCamera(bool isLeft)
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

        public void ResetTilt()
        {
            currentTilt = Mathf.Lerp(currentTilt, 0, tiltTime * Time.deltaTime);
        }
    }
}