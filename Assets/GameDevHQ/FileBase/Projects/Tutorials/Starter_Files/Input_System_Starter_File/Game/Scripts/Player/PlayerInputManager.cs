using Game.Scripts.LiveObjects;
using Game.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour// NEW INPUT SYSTEM
{
    private AllInputActions _input;
    [SerializeField] private Player _player;
    [SerializeField] private Drone _drone;

    [SerializeField] 
    void Start()
    {
        _input = new AllInputActions();
        _input.Player.Enable();
        _input.Player.DroneState.performed += DroneState_performed;
    }
    void Update()
    {
        var x = _input.Player.Movement.ReadValue<Vector2>().x;
        var y = _input.Player.Movement.ReadValue<Vector2>().y;
        _player.CalcutateMovement(x,y);
    }

    private void DroneState_performed(InputAction.CallbackContext context)
    {
        _drone.EnterFlightMode();
    }

    public void EnablePlayerInput()
    {
        _input.Player.Enable();
    }

    public void DisablePlayerInput()
    {
        _input.Player.Disable();
    }

    private void OnDisable()
    {
        if (_input != null)
        {   
            _input.Player.DroneState.performed -= DroneState_performed;
            _input.Player.Disable();
        }
    }
    private void OnDestroy()
    {
        if (_input != null)
        {
            _input.Player.Disable();
        }
    }
}
