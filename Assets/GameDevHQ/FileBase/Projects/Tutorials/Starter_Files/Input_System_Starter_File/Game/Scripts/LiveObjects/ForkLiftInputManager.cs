using Game.Scripts.LiveObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForkLiftInputManager : MonoBehaviour
{
    private AllInputActions _input;// NEW INPUT SYSTEM
    [SerializeField] private Forklift _forklift;
    void Start()
    {
        InitializeInputs();
    }

    private void InitializeInputs()
    {
        _input = new AllInputActions();
        _input.Forklift.Enable();
        _input.Forklift.GetOutOfForklift.performed += GetOutOfForkLift_performed;
    }
    private void GetOutOfForkLift_performed(InputAction.CallbackContext context)
    {
        _forklift.ExitDriveMode();
    }

    void Update()
    {
        float x = _input.Forklift.Movement.ReadValue<Vector2>().x;
        float y = _input.Forklift.Movement.ReadValue<Vector2>().y;

        float lift = _input.Forklift.LiftControl.ReadValue<float>();

        _forklift.CalcutateMovement(x,y);
        _forklift.LiftControls(lift);
    }

    void OnDisable()
    {
        if (_input != null)
        {
            _input.Forklift.GetOutOfForklift.performed -= GetOutOfForkLift_performed;
            _input.Forklift.Disable();
        }
    }
}
