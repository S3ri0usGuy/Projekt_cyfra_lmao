using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [SerializeField] private float ability1Coolodown;
    [SerializeField] private float ability2Coolodown;
    [SerializeField] private float ability3Coolodown;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDistance;
    private bool abilit1IsOnCollodown, abilit2IsOnCollodown, abilit3IsOnCollodown;

    private PlayerMovement playerMovement;

    private float baseMovementSpeed;

    void Awake()
    {
        abilit1IsOnCollodown = false;
        abilit2IsOnCollodown = false;
        abilit3IsOnCollodown = false;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        baseMovementSpeed = playerMovement._moveSpeed;
    }

    public void Ability1()
    {
        if (!abilit1IsOnCollodown)
        {
            movementSpeedBoost();
            abilit1IsOnCollodown = true;
            Invoke("resetAbility1Cooldown", ability1Coolodown);
        }
    }

    public void resetAbility1Cooldown()
    {
        abilit1IsOnCollodown = false;
    }

    public void Ability2()
    {
        Debug.Log("2");
    }

    public void Ability3()
    {
        Debug.Log("3");
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
}
