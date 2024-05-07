using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogsScript : MonoBehaviour
{
    public GameObject text;
    public GameObject panel;
    public Transform circleOrigin;
    public float triggerRadius;
    public TextMeshProUGUI dialog;

    GameObject player;
    private bool canTalk;
    private bool isTalking;
    private int stage;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        text.SetActive(false);
        panel.SetActive(false);
    }

    private void Update()
    {
        Vector2 playerPosition = player.transform.position;
        float distance = Vector2.Distance(playerPosition, circleOrigin.position);

        if (distance < triggerRadius && !isTalking)
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
        isTalking = startTaling;
        panel.SetActive(startTaling);
        if(startTaling)
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        else
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        stage = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, triggerRadius);
    }
}
