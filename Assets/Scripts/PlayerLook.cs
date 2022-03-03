using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private InputManager inputManager;
    private Camera playerCamera;

    [SerializeField] private float mouseSensitivity;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        playerCamera = Camera.main;
    }

    private void Update()
    {
        transform.rotation = new Quaternion(transform.rotation.x, playerCamera.transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }
}
