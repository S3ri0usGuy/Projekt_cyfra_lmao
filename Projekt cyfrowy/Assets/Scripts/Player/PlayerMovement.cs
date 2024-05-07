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
    private DialogsScript dialogsScript;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";

    private WeponParent weponParent;

    [SerializeField] private InputActionReference movement, attack, pointerPosition, talk;

    private void OnEnable()
    {
        attack.action.performed += PerformAttack;
        talk.action.performed += PerformTalking;
    }

    private void OnDisable()
    {
        attack.action.performed -= PerformAttack;
        talk.action.performed -= PerformTalking;
    }

    private void PerformAttack(InputAction.CallbackContext obj)
    {
        weponParent.Attack();
    }
    private void PerformTalking(InputAction.CallbackContext obj)
    {
        dialogsScript.Talk();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        dialogsScript = GameObject.FindGameObjectWithTag("NPC").GetComponent<DialogsScript>();
        weponParent = GetComponentInChildren<WeponParent>();
    }

    private void Update()
    {
        pointerInput = GetPointerInput();
        
        weponParent.PointerPosition = pointerInput;

        _movement.Set(InputManager.movement.x, InputManager.movement.y);

        if(_movement.x == 0 &&  _movement.y == 0)
            _animator.SetBool("IsMoving", false);
        else
            _animator.SetBool("IsMoving", true);

        _rigidbody.velocity = _movement * _moveSpeed;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _animator.SetFloat(_horizontal, (mousePos.x - transform.position.x) / 100);
        _animator.SetFloat(_vertical, (mousePos.y - transform.position.y) / 100);
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
