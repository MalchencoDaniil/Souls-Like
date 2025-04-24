using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerInput
    {
        internal Vector2 MovementInput()
        {
            return InputManager._instance._inputActions.Player.Movement.ReadValue<Vector2>();
        }
    }
}