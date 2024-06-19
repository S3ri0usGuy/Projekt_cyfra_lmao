using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float _moveSpeed;

    private Vector2 pointerInput, _movement;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private WeponParent weponParent;
    private PlayerDialogsScript playerDialogsScript;
    private Abilities abilities;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";

    [SerializeField] private InputActionReference movement, attack, pointerPosition, talk, ability1, ability2, ability3, switchWeapon, togglePause;

    public float weaponChangeCooldownTime;
    public float lastAttackTime;

    private void OnEnable()
    {
        attack.action.performed += PerformAttack;
        talk.action.performed += PerformTalking;
        ability1.action.performed += PerformAbility1;
        ability2.action.performed += PerformAbility2;
        ability3.action.performed += PerformAbility3;
        switchWeapon.action.performed += PerformSwitchWeapon;
        togglePause.action.performed += TogglePause;
    }

    private void OnDisable()
    {
        attack.action.performed -= PerformAttack;
        talk.action.performed -= PerformTalking;
        ability1.action.performed -= PerformAbility1;
        ability2.action.performed -= PerformAbility2;
        ability3.action.performed -= PerformAbility3;
        switchWeapon.action.performed -= PerformSwitchWeapon;
        togglePause.action.performed -= TogglePause;
    }

    public void PerformAttack(InputAction.CallbackContext context)
    {
        if (weponParent.gameObject.activeInHierarchy)
        {
            weponParent.Attack();
            lastAttackTime = Time.time;
        }
        else
        {
            Debug.LogWarning("WeponParent is inactive!");
        }
    }

    private void PerformTalking(InputAction.CallbackContext obj)
    {
        playerDialogsScript.Talk();
    }
    private void PerformAbility1(InputAction.CallbackContext obj)
    {
        abilities.Ability1();
    }
    private void PerformAbility2(InputAction.CallbackContext obj)
    {
        abilities.Ability2();
    }
    private void PerformAbility3(InputAction.CallbackContext obj)
    {
        abilities.Ability3();
    }
    private void PerformSwitchWeapon(InputAction.CallbackContext obj)
    {
        abilities.SwitchWeapon();
    }
    private void TogglePause(InputAction.CallbackContext obj)
    {
        abilities.TogglePause();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        playerDialogsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogsScript>();
        SetNewWepon();
        abilities = GameObject.FindGameObjectWithTag("Player").GetComponent<Abilities>();
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

    public void SetNewWepon()
    {
        weponParent = GetComponentInChildren<WeponParent>();
    }
}
