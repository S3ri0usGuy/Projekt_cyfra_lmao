using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;

public class WeponParent : MonoBehaviour
{
    Quaternion emptyQuaternion;
    Vector3 projecttileSpawnPosition;
    [SerializeField] private GameObject missile;

    public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 PointerPosition { get; set; }

    public Animator animator;
    public float delay = 0.3f;
    public bool attackBlocked;

    public bool isRange = false;

    public bool IsAttacking { get; private set; }

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    public Transform circleOrigin;
    public float radius;

    private void Update()
    {
        if (IsAttacking)
            return;
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if(direction.x < 0)
        {
            scale.y = -1;
        }
        else if(direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;

        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }

    public void Attack()
    {
        if (attackBlocked)
            return;
        animator.SetTrigger("Attack");
        IsAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            EnemyHp enemyHp;
            if (enemyHp = collider.GetComponent<EnemyHp>())
            {
                enemyHp.GetHit(2, transform.parent.gameObject, true);
            }
        }
    }

    public void Shoot()
    {
        projecttileSpawnPosition = transform.position;
        Instantiate(missile, projecttileSpawnPosition, emptyQuaternion);
    }
}