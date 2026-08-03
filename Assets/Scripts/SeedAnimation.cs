using UnityEngine;
using System;
using System.Collections;
public class SeedAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Atrributes")]
    [SerializeField] private float seedSpeed = 5f;

    private Transform target;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized; //move the bullet towards the enemy

        rb.linearVelocity = direction * seedSpeed;
    }
}
