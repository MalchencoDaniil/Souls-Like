using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Player
{
    public class PlayerInput
    {
        internal Vector2 MovementInput()
        {
            return InputManager._instance._inputActions.Player.Movement.ReadValue<Vector2>();
        }

        internal bool SwitchMoveType()
        {
            return InputManager._instance._inputActions.Camera.Focus.triggered;
        }

        internal bool RollInputEvent()
        {
            return InputManager._instance._inputActions.Player.Roll.triggered;
        }
    }
}