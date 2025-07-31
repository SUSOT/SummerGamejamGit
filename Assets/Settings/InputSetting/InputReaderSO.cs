using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Settings.InputSetting
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "SO/InputReader", order = 0)]
    public class InputReaderSO : ScriptableObject, Controls.IPlayerActions, Controls.IUIActions
    {
        public Action OnDashKeyPressed;
        public event Action<Vector2> OnUINavigation;
        public event Action<Vector2> OnUISilder;
        public event Action OnUIOnSubmitPressed;
        public event Action OnUIOnCancelPressed;

        private Controls _controls;

        public Vector2 MoveDirection {get; private set;}

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
                _controls.UI.SetCallbacks(this);
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

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 uiMovement = context.ReadValue<Vector2>().normalized;
                OnUINavigation?.Invoke(uiMovement);
            }
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnUIOnSubmitPressed?.Invoke();
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                OnUIOnCancelPressed?.Invoke();
            }
        }
        public void OnSilder(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 uiMovement = context.ReadValue<Vector2>().normalized;
                OnUINavigation?.Invoke(uiMovement);
            }
        }

        public void DisablePlayerCnt()
        {
            _controls.Player.Disable();
        }

        public void EnablePlayerCnt()
        {
            _controls.Player.Enable();
        }

        public void EnableUICnt()
        {
            _controls.UI.Enable();
        }

        public void DisableUICnt()
        {
            _controls.UI.Disable();
        }


        #region Not Use

        public void OnPoint(InputAction.CallbackContext context)
        {
            
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            
        }

        #endregion
    }
}