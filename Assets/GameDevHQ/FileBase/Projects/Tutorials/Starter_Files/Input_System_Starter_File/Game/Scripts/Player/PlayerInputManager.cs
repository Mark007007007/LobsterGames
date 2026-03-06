using Game.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour// NEW INPUT SYSTEM
{
    private AllInputActions _input;
    [SerializeField] private Player _player;
    void Start()
    {
        _input = new AllInputActions();
        _input.Player.Enable();
    }
    void Update()
    {
        var x = _input.Player.Movement.ReadValue<Vector2>().x;
        var y = _input.Player.Movement.ReadValue<Vector2>().y;
        _player.CalcutateMovement(x,y);
    }

    private void OnDestroy()
    {
        if (_input != null)
        {
            _input.Player.Disable();
        }
    }
}
