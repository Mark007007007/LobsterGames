using Game.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private AllInputActions _input;
    [SerializeField] private Player _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = new AllInputActions();
        _input.Player.Enable();
        //_input.Player.Movement.performed += Movement_Performed;
    }

    // private void Movement_Performed(InputAction.CallbackContext context)
    // {
       


    // }

    // Update is called once per frame
    void Update()
    {
        var x = _input.Player.Movement.ReadValue<Vector2>().x;
        var y = _input.Player.Movement.ReadValue<Vector2>().y;
        _player.CalcutateMovement(x,y);
    }
}
