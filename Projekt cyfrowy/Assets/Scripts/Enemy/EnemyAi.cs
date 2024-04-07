using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class EnemyAi : MonoBehaviour
{
    public Transform target;

    Vector3 projecttileSpawnPosition;
    Quaternion emptyQuaternion;

    [SerializeField] private int movementSpeed;
    [SerializeField] private float baseTriggerDistance;
    [SerializeField] private float chasingTriggerDistance;
    [SerializeField] private bool isRanged;
    [SerializeField] private float runAwayZone;
    [SerializeField] private float endOfSafeZone;

    [SerializeField] private GameObject missile;
    [SerializeField] private float attackCooldown;
    private bool canDoSomething = true;

    [SerializeField] private int movementSpeedBoost;
    private int currentMovementSpeedBoost = 1;

    private int curentMovementSpeed;
    private float distanceFromTarget;
    private bool isChasing = true;
    Vector2 targetPosition;

    public float nextWaypointDistance = 1f;

    public Transform enemyGrapics;

    Path path;
    int currentWaypiont = 0;

    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    {
        curentMovementSpeed = movementSpeed;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating(nameof(UpdatePath), 0f, .1f);
    }

    void UpdatePath()
    {
        targetPosition = target.position;

        if(isRanged && distanceFromTarget <= runAwayZone)
        {
            targetPosition -= ((Vector2)target.position - rb.position) * 4;
        }

        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, targetPosition, OnPathComlete);
        }
    }

    void OnPathComlete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypiont = 0;
        }
    }

    void Shoot()
    {
        projecttileSpawnPosition = rb.position;
        Instantiate(missile, projecttileSpawnPosition, emptyQuaternion);
        //Debug.Log("Shoot!");
        canDoSomething = true;
    }

    private void FixedUpdate()
    {
        distanceFromTarget = Vector2.Distance(transform.position, target.position);

        if(canDoSomething && ((!isChasing && distanceFromTarget < baseTriggerDistance) || (isChasing && distanceFromTarget < chasingTriggerDistance))
            && !(isRanged && distanceFromTarget > runAwayZone && distanceFromTarget <= endOfSafeZone))
        {
            curentMovementSpeed = movementSpeed * currentMovementSpeedBoost;
            isChasing = true;
        }
        else if (isRanged && canDoSomething && distanceFromTarget <= endOfSafeZone)
        {
            isChasing = false;
            curentMovementSpeed = 0;
            canDoSomething = false;
            Invoke(nameof(Shoot), attackCooldown);
            //Debug.Log("Preparing shoot...");
        }
        else
        {
            isChasing = false;
            curentMovementSpeed = 0;
        }

        if (path == null)
            return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypiont] - rb.position).normalized;
        Vector2 force = curentMovementSpeed * Time.deltaTime * direction;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypiont]);

        if(distance < nextWaypointDistance)
        {
            currentWaypiont++;
        }

        if (force.x >= 10f)
        {
            enemyGrapics.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x <= -10f)
        {
            enemyGrapics.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void RunAway()
    {
        if(isRanged && distanceFromTarget <= runAwayZone)
        {
            currentMovementSpeedBoost = movementSpeedBoost;
            Invoke(nameof(RemoveSpeedBoost), 0.2f);
        }
    }

    public void RemoveSpeedBoost()
    {
        currentMovementSpeedBoost = 1;
    }
}
