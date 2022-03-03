using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private PlayerControls playerControls;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else
        {
            _instance = this;
        }

        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    public Vector2 GetMovementInput()
    {
        return playerControls.Gameplay.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetLookInput()
    {
        return playerControls.Gameplay.Look.ReadValue<Vector2>();
    }

    public bool GetJumpInput()
    {
        return playerControls.Gameplay.Jump.triggered;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
