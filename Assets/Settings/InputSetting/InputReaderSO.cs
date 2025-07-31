using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Settings.InputSetting
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "SO/InputReader", order = 0)]
    public class InputReaderSO : ScriptableObject, Controls.IPlayerActions
    {
        public Action OnDashKeyPressed;
        
        private Controls _controls;

        public Vector2 MoveDirection {get; private set;}

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveDirection = context.ReadValue<Vector2>().normalized;
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnDashKeyPressed?.Invoke();
            }
        }
    }
}