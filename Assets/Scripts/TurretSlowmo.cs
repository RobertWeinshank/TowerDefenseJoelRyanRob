using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretSlowmo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float attackSpeed = 4f;
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private int damage = 1;

    private float timeUntilFire;

    void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / attackSpeed)
        {
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask); //Takes the turret positin, range, direction (our position in vector2), distance from target, and layermask

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>(); //Gets the enemy movement script of any enemy hit by the raycast
                em.UpdateSpeed(0.5f);//Updates the speed once you get the script

                EnemyHealth eh = hits[i].transform.GetComponent<EnemyHealth>(); //Damages the enemy overtime
                eh.TakeDamage(damage);
                Debug.Log("Enemy taking rain damage");

                StartCoroutine(ResetEnemeySpeed(em)); //pass the method resetEnemySpeed
            }
        }
    }

    private IEnumerator ResetEnemeySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
