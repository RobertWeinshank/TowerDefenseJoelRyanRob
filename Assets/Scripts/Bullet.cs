using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public AudioClip impactSound;

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
        
        yield return new WaitForEndOfFrame();

        em.ResetSpeed();
    }

    public void ChangeDamage(int damage)
    {
        bulletDamage = damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyMovement em = other.transform.GetComponent<EnemyMovement>();
        other.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage); //On bullet collision, take damage
        AudioSource.PlayClipAtPoint(impactSound, transform.position);
        Destroy(gameObject); //destroy bullet after collision
        StartCoroutine(ResetEnemeySpeed(em));
    }
}

