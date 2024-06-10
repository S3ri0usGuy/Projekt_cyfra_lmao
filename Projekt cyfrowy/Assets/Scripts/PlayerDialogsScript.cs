using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDialogsScript : MonoBehaviour
{
    public GameObject text;
    public GameObject panel;
    public Transform NPC1, NPC2, NPC3, NPC4;
    [SerializeField] public float triggerRadius;
    public TextMeshProUGUI dialog;

    private bool canTalk = false;
    private bool isTalking = false;
    private int stage;

    private Vector2 playerPosition;

    private void Start()
    {
        text.SetActive(false);
        panel.SetActive(false);
    }

    private void Update()
    {
        playerPosition = gameObject.transform.position;
        float distance1 = Vector2.Distance(playerPosition, NPC1.position);
        float distance2 = Vector2.Distance(playerPosition, NPC2.position);
        float distance3 = Vector2.Distance(playerPosition, NPC3.position);
        float distance4 = Vector2.Distance(playerPosition, NPC4.position);

        if ((distance1 < triggerRadius && !isTalking) || (distance2 < triggerRadius && !isTalking) || (distance3 < triggerRadius && !isTalking) || (distance4 < triggerRadius && !isTalking))
            canTalk = true;
        else
            canTalk = false;

        text.SetActive(canTalk);
    }

    public void Talk()
    {
        if (!canTalk && !isTalking)
            return;
        if(!isTalking)
            TogleTalking(true);

        switch (stage)
        {
            case 0:
                dialog.text = "siema";
                break;
            case 1:
                dialog.text = "elo";
                break;
            case 2:
                dialog.text = "idŸ sobie";
                break;
            case 3:
                dialog.text = "ok";
                break;
            case 4:
                TogleTalking(false);
                break;
        }
        stage++;
    }

    public void TogleTalking(bool startTaling)
    {
        Debug.Log("toggle talk");
        isTalking = startTaling;
        panel.SetActive(startTaling);
        if(startTaling)
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        else
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        stage = 0;
    }
}
