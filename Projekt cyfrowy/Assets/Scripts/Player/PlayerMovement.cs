using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private Vector2 pointerInput, _movement;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";

    private bool flipped = false;

    private WeponParent weponParent;

    [SerializeField] private InputActionReference movement, attack, pointerPosition;

    private void OnEnable()
    {
        attack.action.performed += PerformAttack;
    }

    private void OnDisable()
    {
        attack.action.performed -= PerformAttack;
    }

    private void PerformAttack(InputAction.CallbackContext obj)
    {
        weponParent.Attack();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        weponParent = GetComponentInChildren<WeponParent>();
    }

    private void Update()
    {
        pointerInput = GetPointerInput();
        
        weponParent.PointerPosition = pointerInput;

        _movement.Set(InputManager.movement.x, InputManager.movement.y);

        _rigidbody.velocity = _movement * _moveSpeed;

        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(mousePos.x < transform.position.x && !flipped)
        {
            flip();
        }
        else if(mousePos.x > transform.position.x && flipped)
        {
            flip();
        }
    }

    private void flip()
    {
        flipped = !flipped;
        transform.Rotate(0f, 180f, 0f);
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
