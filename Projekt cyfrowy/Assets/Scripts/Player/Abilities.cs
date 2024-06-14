using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Abilities : MonoBehaviour
{
    [SerializeField] private float ability1Cooldown;
    [SerializeField] private float ability2Cooldown;
    [SerializeField] private float ability3Cooldown;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDistance;
    [SerializeField] private float barrierDuration;
    [SerializeField] private Transform teleportDestiny;

    [SerializeField] private TextMeshProUGUI ability1CooldownText;
    [SerializeField] private TextMeshProUGUI ability2CooldownText;
    [SerializeField] private TextMeshProUGUI ability3CooldownText;
    [SerializeField] public GameObject ability1;
    [SerializeField] public GameObject ability2;
    [SerializeField] public GameObject ability3;
    [SerializeField] private GameObject ability1Faded;
    [SerializeField] private GameObject ability2Faded;
    [SerializeField] private GameObject ability3Faded;

    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject swordInterface;
    [SerializeField] private GameObject bowInterface;

    private bool ability1IsOnCooldown, ability2IsOnCooldown, ability3IsOnCooldown, playerHoldSword, canSwitch;
    public bool playerHasability2, playerHasability3, playerHasBow;

    private PlayerMovement playerMovement;
    private PlayerResources playerResources;
    [SerializeField] private WeponParent weponParent;
    [SerializeField] private WeponParent bowParent;
    [SerializeField] private GameObject PausePanel;


    private float baseMovementSpeed;

    private GameObject barrier;

    public Animator animator;

    private bool isPaused = false;

    void Awake()
    {
        ability1IsOnCooldown = false;
        ability2IsOnCooldown = false;
        ability3IsOnCooldown = false;
        playerMovement = GetComponent<PlayerMovement>();
        playerResources = GetComponent<PlayerResources>();
        baseMovementSpeed = playerMovement._moveSpeed;
        barrier = GameObject.FindGameObjectWithTag("Barrier");
        barrier.SetActive(false);

        ability1CooldownText.text = "";
        ability2CooldownText.text = "";
        ability3CooldownText.text = "";
        ability1Faded.SetActive(false);
        ability2Faded.SetActive(false);
        ability3Faded.SetActive(false);

        ability1.SetActive(false);
        ability2.SetActive(false);
        ability3.SetActive(false);
        playerHasability2 = false;
        playerHasability3 = false;
        playerHasBow = false;

        playerHoldSword = true;
        bow.SetActive(!playerHoldSword);
        bowInterface.SetActive(!playerHoldSword);
    }

    public void ResetAbility1Cooldown()
    {
        ability1IsOnCooldown = false;
        ability1Faded.SetActive(false);
        ability1CooldownText.text = "";
    }

    public void ResetAbility2Cooldown()
    {
        ability2IsOnCooldown = false;
        ability2Faded.SetActive(false);
        ability2CooldownText.text = "";
    }

    public void ResetAbility3Cooldown()
    {
        ability3IsOnCooldown = false;
        ability3Faded.SetActive(false);
        ability3CooldownText.text = "";
    }

    public void Ability1()
    {
        if (!ability1IsOnCooldown && playerResources.isNight)
        {
            movementSpeedBoost();
            ability1IsOnCooldown = true;
            ability1Faded.SetActive(true);
            StartCoroutine(StartCooldown(ability1Cooldown, ability1CooldownText, ResetAbility1Cooldown));
        }
    }

    public void Ability2()
    {
        if (!ability2IsOnCooldown && playerResources.isNight && playerHasability2)
        {
            castBarrier();
            ability2IsOnCooldown = true;
            ability2Faded.SetActive(true);
            StartCoroutine(StartCooldown(ability2Cooldown + barrierDuration, ability2CooldownText, ResetAbility2Cooldown));
        }
    }

    public void Ability3()
    {
        if (!ability3IsOnCooldown && playerResources.isNight && playerHasability3)
        {
            animator.SetTrigger("StartTeleport");
            ability3IsOnCooldown = true;
            ability3Faded.SetActive(true);
            StartCoroutine(StartCooldown(ability3Cooldown, ability3CooldownText, ResetAbility3Cooldown));
        }
    }

    public void SwitchWeapon()
    {
        if (playerHasBow && Time.time - playerMovement.lastAttackTime >= playerMovement.weaponChangeCooldownTime)
        {
            sword.SetActive(!playerHoldSword);
            bow.SetActive(playerHoldSword);
            swordInterface.SetActive(!playerHoldSword);
            bowInterface.SetActive(playerHoldSword);
            playerHoldSword = !playerHoldSword;
            playerMovement.SetNewWepon();
            weponParent.attackBlocked = false;
            bowParent.attackBlocked = false;
        }
        else if(canSwitch)
        {
            canSwitch = false;
            Invoke("SwitchWeapon", playerMovement.weaponChangeCooldownTime - (Time.time - playerMovement.lastAttackTime) + 0.03f);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();

        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        PausePanel.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        PausePanel.SetActive(false);
    }

    private void movementSpeedBoost()
    {
        playerMovement._moveSpeed = dashSpeed;
        Invoke("ResetMovementSpeed", dashDistance);
    }

    private void ResetMovementSpeed()
    {
        playerMovement._moveSpeed = baseMovementSpeed;
    }

    private void castBarrier()
    {
        playerResources.barrierIsActive = true;
        barrier.SetActive(true);
        Invoke("RemoveBarrier", barrierDuration);
    }

    public void RemoveBarrier()
    {
        if (playerResources.barrierIsActive)
        {
            barrier.SetActive(false);
            playerResources.barrierIsActive = false;
        }
    }

    public void OnTeleportAnimationComplete()
    {
        CastTeleport();
    }

    public void CastTeleport()
    {
        gameObject.transform.position = teleportDestiny.position;
    }

    private IEnumerator StartCooldown(float cooldown, TextMeshProUGUI cooldownText, System.Action resetAction)
    {
        float remainingTime = cooldown;
        while (remainingTime > 0)
        {
            cooldownText.text = remainingTime.ToString("F1");
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        resetAction();
    }
}
