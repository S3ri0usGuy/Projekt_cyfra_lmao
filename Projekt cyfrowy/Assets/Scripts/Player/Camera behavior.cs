using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerabehavior : MonoBehaviour
{
    [SerializeField] private Transform target;    
    private Vector3 ofset = new Vector3(0f, 0f, -10f);

    private void Update()
    {
        Vector3 targetPosition = target.position + ofset;
        transform.position = targetPosition;
    }
}
