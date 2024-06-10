using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kitLogic : MonoBehaviour
{
    [SerializeField] private float minPushForce;
    [SerializeField] private float maxPushForce;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomPushForce = Random.Range(minPushForce, maxPushForce);
        rb.AddForce(randomDirection * randomPushForce, ForceMode2D.Impulse);

        Invoke("DestroyGameObject", 3f);
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}