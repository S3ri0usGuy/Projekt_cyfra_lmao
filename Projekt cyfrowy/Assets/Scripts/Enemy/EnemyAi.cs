using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;
using System.Reflection;

public class EnemyAi : MonoBehaviour
{
    public Transform target;
    private Transform fountain;

    Vector3 projecttileSpawnPosition;
    Quaternion emptyQuaternion;

    [SerializeField] private int movementSpeed;
    [SerializeField] private float baseTriggerDistance;
    [SerializeField] private float chasingTriggerDistance;
    [SerializeField] private bool isRanged;
    [SerializeField] private float runAwayZone;
    [SerializeField] private float endOfSafeZone;

    [SerializeField] private GameObject missile;
    [SerializeField] private float attackCastTime;
    [SerializeField] private float attackCooldown;
    private bool canDoSomething = true;
    private bool canShoot = true;

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

    private Transform player;
    private Vector2 force;
    private Vector2 direction;

    private PlayerResources playerResources;

    private void Awake()
    {
        playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fountain = GameObject.FindGameObjectWithTag("Fountain").transform;

        target = player;

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
        GameObject newMissile = Instantiate(missile, projecttileSpawnPosition, emptyQuaternion);

        ProjectilesScript missileScript = newMissile.GetComponent<ProjectilesScript>();
        if (missileScript != null)
        {
            missileScript.shooter = gameObject;
        }

        canDoSomething = true;
    }

    void LetShootAgain()
    {
        canShoot = true;
    }

    private void FixedUpdate()
    {
        if (!playerResources.isNight)
        {
            Destroy(gameObject);
        }
        
        target = player;
        distanceFromTarget = Vector2.Distance(transform.position, target.position);

        if ((isChasing && distanceFromTarget > chasingTriggerDistance) || (!isChasing && distanceFromTarget > baseTriggerDistance))
        {
            target = fountain;
            distanceFromTarget = Vector2.Distance(transform.position, target.position);
        }

        if (target == player)
        {
            if (canDoSomething && !(isRanged && distanceFromTarget > runAwayZone && distanceFromTarget <= endOfSafeZone))
            {
                curentMovementSpeed = movementSpeed;
                isChasing = true;
            }
            else if (isRanged && canShoot && distanceFromTarget <= endOfSafeZone)
            {
                isChasing = false;
                curentMovementSpeed = 0;
                canDoSomething = false;
                canShoot = false;
                Invoke(nameof(Shoot), attackCastTime);
                Invoke(nameof(LetShootAgain), attackCastTime + attackCooldown);
                //Debug.Log("Preparing shoot...");
            }
            else
            {
                curentMovementSpeed = 0;
                isChasing = false;
            }
        }
        else
        {
            if (canDoSomething && !(isRanged && distanceFromTarget > runAwayZone && distanceFromTarget <= endOfSafeZone))
            {
                curentMovementSpeed = movementSpeed;
                isChasing = true;
            }
            else if (isRanged && canShoot && distanceFromTarget <= endOfSafeZone)
            {
                isChasing = false;
                curentMovementSpeed = 0;
                canDoSomething = false;
                canShoot = false;
                Invoke(nameof(Shoot), attackCastTime);
                Invoke(nameof(LetShootAgain), attackCastTime + attackCooldown);
                //Debug.Log("Preparing shoot...");
            }
            else
            {
                curentMovementSpeed = 0;
                isChasing = false;
            }
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

    public void RunAway(bool isDamageFromMelee)
    {
        if (isDamageFromMelee || (isRanged && distanceFromTarget <= runAwayZone))
        {
            direction = (transform.position - player.position).normalized;
            force = direction * playerResources.knockbackForce;
            if (isRanged) force *= 0.75f;
            gameObject.GetComponent<Rigidbody2D>().AddForce(force);
        }
    }
}