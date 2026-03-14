using Game.Scripts.LiveObjects;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaptopInputManager : MonoBehaviour// NEW INPUT SYSTEM
{
    private AllInputActions _input;
    [SerializeField] private Laptop _laptop;
    void Start()
    {
        InitializeInputs();
    }

    private void InitializeInputs()
    {
        _input = new AllInputActions();
        _input.Laptop.Enable();
        _input.Laptop.SwitchCamreras.performed += SwitchCameras_performed;
        _input.Laptop.StopUsingCameras.performed += StopUsingCameras_performed;
    }

    private void SwitchCameras_performed(InputAction.CallbackContext context)
    {
        _laptop.SwitchCameras();
    }

    private void StopUsingCameras_performed(InputAction.CallbackContext context)
    {
        _laptop.StopUsingCameras();
    }
}
