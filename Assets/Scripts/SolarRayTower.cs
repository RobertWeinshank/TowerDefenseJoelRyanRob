using UnityEditor;
using UnityEngine;

public class SolarRayTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float damagePerSecond = 1f;
    [SerializeField] private int raybeamDamage = 1;

    private Transform target;
    private float timeUntilFire;

    private LineRenderer line;
    private DistanceJoint2D raybeam;


    private void Start()
    {
        raybeam = gameObject.AddComponent<DistanceJoint2D>();
        line = GetComponent<LineRenderer>();

        raybeam.enabled = false;
        line.enabled = false;
    }


    private void Update()
    {
        line.SetPosition(0, firingPoint.position); //Create the line at the fire point

        if (target == null)
        {
            
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
            StopRaybeam(); //If no targets are in range, stop the raybeam cast
        }
        else //if there are targets in range, shoot
        {
            timeUntilFire += Time.deltaTime;
            FireRaybeam(target); //Display the raybeam at the target in range

            if (timeUntilFire >= 1f / damagePerSecond)
            {               
                Solarbeam();//Deal damage to enemy                
            }
        }
    }

    private void Solarbeam()
    {
        target.gameObject.GetComponent<EnemyHealth>().TakeDamage(raybeamDamage); //call enemeyHealth script to deal raybeam damage
        //Debug.Log(target.gameObject.GetComponent<EnemyHealth>().hitPoints + "Hitpoints");
        timeUntilFire = 0f; //Reset fire time
        if (target.gameObject.GetComponent<EnemyHealth>().hitPoints == 0 || target.gameObject.GetComponent<EnemyHealth>().isDestroyed)
        {
            StopRaybeam();//Stop the raybeam if the target's hp is 0 or is destroyed
        }
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask); //Takes the turret positin, range, direction (our position in vector2), distance from target, and layermask

        if (hits.Length > 0)
        {
            target = hits[0].transform; //turret finds a target in range
        }

    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg; // Get the angle between the target and turret (in both x and y) and multiply it by rad2

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);// slowley rotates the turret instead of having it snap to target / back to center
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void FireRaybeam(Transform hit)
    {
        //set the raybeam's location to the enemy's position
        raybeam.enabled = true;
        raybeam.connectedAnchor = hit.position;

        line.enabled = true;
        line.SetPosition(1, hit.position);
    }

    private void StopRaybeam()
    {
        //Debug.Log("Stopping laser");
        raybeam.enabled = false;
        line.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
