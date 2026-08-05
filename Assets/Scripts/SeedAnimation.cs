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
    //private float originalY;

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //private void Start()
    //{
    //    this.originalY = this.transform.position.y;
    //}

    //// Update is called once per frame
    //private void Update()
    //{
    //    transform.position = new Vector3(transform.position.x, originalY + (Mathf.Sin(Time.time) * 1), transform.position.z);
    //    Destroy(this, 3);
    //}

    //private IEnumerator WaitForSeconds()
    //{
    //    yield return new WaitForSeconds(3f);
    //    gameObject.SetActive(false);
    //    Destroy(gameObject, 3);
    //}

}
