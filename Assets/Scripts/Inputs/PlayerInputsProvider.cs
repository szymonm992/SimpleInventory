using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SimpleInventory.Inputs
{
    public class PlayerInputsProvider : IPlayerInputsProvider, InputActions.IDefaultActions
    {
        public event Action InventoryButtonPressedEvent;

        public Vector2 Movement { get; private set; }
        public Vector2 MouseDelta { get; private set; }
        public bool Interact { get; private set; }
        public bool InventoryKey { get; private set; }

        private readonly InputActions inputActions;

        public PlayerInputsProvider()
        {
            inputActions = new();
            inputActions.Default.SetCallbacks(this);
            SetInputsEnabled(true);
        }

        public void Dispose()
        {
            SetInputsEnabled(false);
            inputActions.Dispose();
        }

        public void SetInputsEnabled(bool enabled)
        {
            if (enabled)
            {
                inputActions.Enable();
            }
            else
            {
                inputActions.Disable();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            Interact = context.performed;
        } 

        public void OnMovement(InputAction.CallbackContext context)
        {
            Movement = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            MouseDelta = context.ReadValue<Vector2>();
        }

        public void OnInventory(InputAction.CallbackContext context)
        {
            InventoryKey = context.performed;

            if (context.performed)
            {
                InventoryButtonPressedEvent?.Invoke();
                Debug.Log("sdfsfsd");
            }
        }
    }
}
