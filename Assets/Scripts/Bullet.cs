using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Atrributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    private Transform target;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if(!target) return;

        Vector2 direction = (target.position - transform.position).normalized; //move the bullet towards the enemy

        rb.linearVelocity = direction * bulletSpeed;
    }

    private IEnumerator ResetEnemeySpeed(EnemyMovement em)
    {
        em.UpdateSpeed(.5f);

        //Debug.Log("Slow Basic Turret Shot");

        yield return new WaitForSeconds(.15f);

        //Debug.Log("Reset Basic Turret Shot");
        em.ResetSpeed();
        Destroy(gameObject);

    }

    public void ChangeDamage(int damage)
    {
        bulletDamage = damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyMovement em = other.transform.GetComponent<EnemyMovement>();
        other.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage); //On bullet collision, take damage
        StartCoroutine(ResetEnemeySpeed(em));
        //Destroy(gameObject); //destroy bullet after collision
        
    }
}

