using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OilSlick : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform detectionPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float attackSpeed = 4f;
    [SerializeField] private float oilTime = 1f;
    [SerializeField] private float detectionRange = 3f;

    private float timeUntilFire;

    void Update()
    {
        OilSpill();
    }

    private void OilSpill()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask); //Takes the turret positin, range, direction (our position in vector2), distance from target, and layermask

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>(); //Gets the enemy movement script of any enemy hit by the raycast             
                if (Vector2.Distance(transform.position, hit.transform.position) >= targetingRange)
                { 
                    em.UpdateSpeed(3f);//Updates the speed once you get the script   
                }
                else if (Vector2.Distance(transform.position, hit.transform.position) <= targetingRange)
                {
                    StartCoroutine(ResetEnemeySpeed(em)); //pass the method resetEnemySpeed
                }
            }
        }

    }

    private IEnumerator ResetEnemeySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(oilTime);

        em.ResetSpeed();
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
