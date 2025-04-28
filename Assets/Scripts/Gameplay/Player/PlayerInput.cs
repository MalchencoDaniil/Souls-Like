using UnityEngine;

public class PlayerInput
{
    public bool AttackInput()
    {
        return InputManager._instance._inputActions.Player.Attack.triggered;
    }

    public Vector2 MovementInput()
    {
        return InputManager._instance._inputActions.Player.Movement.ReadValue<Vector2>();
    }

    public bool FocusInput()
    {
        return InputManager._instance._inputActions.Camera.Focus.triggered;
    }

    public bool RollInput()
    {
        return InputManager._instance._inputActions.Player.Roll.triggered;
    }
}