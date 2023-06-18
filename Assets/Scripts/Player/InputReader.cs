using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public event Action OnJump = delegate { };
    public event Action<bool> OnSprint = delegate { };
    public event Action OnAttack = delegate { };

    public Vector2 inputVector { get; private set; }

    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Movement.Enable();
        inputActions.Movement.Jump.performed += Jump;
        inputActions.Movement.Attack.performed += Attack;
        inputActions.Movement.Sprint.started += ctx => Sprint(true);
        inputActions.Movement.Sprint.canceled += ctx => Sprint(false);
    }

    private void OnDisable()
    {
        inputActions.Movement.Disable();
    }

    private void Update()
    {
        inputVector = inputActions.Movement.Movement.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJump();
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAttack();
        }
    }

    private void Sprint(bool isSprinting)
    {
        OnSprint(isSprinting);
    }
}
