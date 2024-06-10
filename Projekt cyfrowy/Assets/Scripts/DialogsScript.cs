using UnityEngine;

public class DialogsScript : MonoBehaviour
{
    private PlayerDialogsScript playerDialogsScript;

    private void Start()
    {
        playerDialogsScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogsScript>();
    }

    private void OnDrawGizmosSelected()
    {
        if (playerDialogsScript == null)
        {
            playerDialogsScript = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerDialogsScript>();
        }

        if (playerDialogsScript != null)
        {
            Gizmos.color = Color.blue;
            Vector3 position = transform == null ? Vector3.zero : transform.position;
            Gizmos.DrawWireSphere(position, playerDialogsScript.triggerRadius);
        }
    }
}
