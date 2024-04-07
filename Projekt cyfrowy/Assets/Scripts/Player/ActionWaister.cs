using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.DefaultInputActions;

public class ActionWaister : MonoBehaviour
{
    private PlayerResources playerResources;
    [SerializeField] private int actionsWaisted = 1;

    private void Start()
    {
        playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject && collision.gameObject.CompareTag("Player"))
        {
            playerResources.UseActions(actionsWaisted);
        }
    }
}
