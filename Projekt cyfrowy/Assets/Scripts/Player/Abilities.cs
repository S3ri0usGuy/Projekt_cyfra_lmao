using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UIElements;

public class Abilities : MonoBehaviour
{
    [SerializeField] private float ability1Coolodown;
    [SerializeField] private float ability2Coolodown;
    [SerializeField] private float ability3Coolodown;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDistance;
    [SerializeField] private float barrierDuration;
    [SerializeField] private Transform teleportDestiny;
    private bool abilit1IsOnCollodown, abilit2IsOnCollodown, abilit3IsOnCollodown;

    private PlayerMovement playerMovement;
    private PlayerResources playerResources;

    private float baseMovementSpeed;

    private GameObject barrier;

    public Animator animator;

    void Awake()
    {
        abilit1IsOnCollodown = false;
        abilit2IsOnCollodown = false;
        abilit3IsOnCollodown = false;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();
        baseMovementSpeed = playerMovement._moveSpeed;
        barrier = GameObject.FindGameObjectWithTag("Barrier");
        barrier.SetActive(false);
    }

    public void ResetAbility1Cooldown()
    {
        abilit1IsOnCollodown = false;
    }

    public void ResetAbility2Cooldown()
    {
        abilit2IsOnCollodown = false;
    }
    public void ResetAbility3Cooldown()
    {
        abilit3IsOnCollodown = false;
    }

    public void Ability1()
    {
        if (!abilit1IsOnCollodown)
        {
            movementSpeedBoost();
            abilit1IsOnCollodown = true;
            Invoke("ResetAbility1Cooldown", ability1Coolodown);
        }
    }

    public void Ability2()
    {
        if (!abilit2IsOnCollodown)
        {
            castBarrier();
            abilit2IsOnCollodown = true;
        }
    }

    public void Ability3()
    {
        if (!abilit3IsOnCollodown)
        {
            animator.SetTrigger("StartTeleport");
            abilit3IsOnCollodown = true;
            Invoke("ResetAbility3Cooldown", ability3Coolodown);
        }
    }

    private void movementSpeedBoost()
    {
        playerMovement._moveSpeed = dashSpeed;
        Invoke("ResterMovementSpeed", dashDistance);
    }

    private void ResterMovementSpeed()
    {
        playerMovement._moveSpeed = baseMovementSpeed;
    }

    private void castBarrier()
    {
        playerResources.berrierIsActive = true;
        barrier.SetActive(true);
        Invoke("RemoveBarrier", barrierDuration);
    }

    public void RemoveBarrier()
    {
        if (playerResources.berrierIsActive)
        {
            barrier.SetActive(false);
            playerResources.berrierIsActive = false;
            Invoke("ResetAbility2Cooldown", ability2Coolodown);
        }
    }

    private void CastTeleport()
    {
        gameObject.transform.position = teleportDestiny.position;
    }
}
