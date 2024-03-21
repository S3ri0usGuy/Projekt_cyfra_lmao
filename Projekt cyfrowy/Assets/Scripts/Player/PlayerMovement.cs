using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    
    private Vector2 _movement;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";

    private bool flipped = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        Debug.Log("siema");
    }

    private void Update()
    {
        _movement.Set(InputManager.movement.x, InputManager.movement.y);

        _rigidbody.velocity = _movement * _moveSpeed;

        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(mousePos.x<transform.position.x && !flipped)
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
}
