using Game.Scripts.LiveObjects;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneInputManager : MonoBehaviour// NEW INPUT SYSTEM
{
    private AllInputActions _input;
    [SerializeField] private Drone _drone;
    void Start()
    {
        _input = new AllInputActions();
        _input.Drone.Enable();
        _input.Drone.PlayerState.performed += PlayerState_performed;
    }
    void Update()
    {
        Thrust();
        Tilt();
        Movement();
    }

    void PlayerState_performed(InputAction.CallbackContext context)
    {
        _drone.EscapeFlightMode();
    }

    private void Tilt()
    {
        Vector2 tilt = _input.Drone.TiltDrone.ReadValue<Vector2>();
        _drone.TiltDrone(tilt);
    }

    private void Thrust()
    {
        float verticalDirection = _input.Drone.Thrust.ReadValue<float>();
        _drone.Thrust(verticalDirection);
    }

    private void Movement()
    {
        float direction = _input.Drone.Movement.ReadValue<float>();
        _drone.CalculateMovementUpdate(direction);
    }
    public void EnableInputManager()
    {
        _input.Drone.Enable();
    }

    public void DisableInputManager()
    {
        _input.Drone.Disable();
    }

    private void OnDisable()
    {
        if (_input != null)
        {   
            _input.Drone.PlayerState.performed -= PlayerState_performed;
            _input.Drone.Disable();
        }
    }
    
    private void OnDestroy()
    {
        if (_input != null)
        {
            _input.Drone.Disable();
        }
    }
}
