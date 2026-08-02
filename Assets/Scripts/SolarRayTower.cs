using UnityEditor;
using UnityEngine;

public class SolarRayTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private UnityEngine.UI.Button upgradeButton;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float damagePerSecond = 1f;
    [SerializeField] private int raybeamDamage = 1;
    [SerializeField] private int baseUpgradeCost = 100; //upgrade stuff

    private Transform target;
    private float timeUntilFire;

    private LineRenderer line;
    private DistanceJoint2D raybeam;

    //upgrade stuff
    private float damagePerSecondBase;
    private float targetingRangeBase;
    private int level = 1;
<<<<<<< Updated upstream
=======

    private float damageOverTimeTimer;
    private int fireDoTDamage = 1;
>>>>>>> Stashed changes

    private void Start()
    {
        raybeam = gameObject.AddComponent<DistanceJoint2D>();
        line = GetComponent<LineRenderer>();

        raybeam.enabled = false;
        line.enabled = false;

        //Upgrade stuff
        damagePerSecondBase = damagePerSecond;
        targetingRangeBase = targetingRange;
        upgradeButton.onClick.AddListener(Upgrade); //anytime you click the upgrade button, calls the upgrade method
    }


    private void Update()
    {
        line.SetPosition(0, firingPoint.position); //Create the line at the fire point

        if (target == null)
        {
            
            FindTarget();
            return;
        }

        //RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
            StopRaybeam(); //If no targets are in range, stop the raybeam cast
        }
        else //if there are targets in range, shoot
        {
            timeUntilFire += Time.deltaTime;
            damageOverTimeTimer = 2;
            FireRaybeam(target); //Display the raybeam at the target in range

            if (timeUntilFire >= 1f / damagePerSecond)
            {               
                Solarbeam();//Deal damage to enemy                
            }
            //if (damageOverTimeTimer > 0)
            //{
            //    FireDamage();
            //    damageOverTimeTimer -= Time.deltaTime;
            //}
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

    private void FireDamage()
    {
        target.gameObject.GetComponent<EnemyHealth>().TakeDamage(fireDoTDamage);
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

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(CalculateCost());

        level++;

        damagePerSecond = CalculateDamagePerSecond();
        targetingRange = CalculateTargetingRange();

        CloseUpgradeUI();
        Debug.Log("New BPS: " + damagePerSecond + "\nNew Range: " + targetingRange + "\nNew Cost: " + CalculateCost());
<<<<<<< Updated upstream
=======

        if (level == 2)
        {
            Debug.Log("Level 2");
        }

        if (level == 3)
        {
            Debug.Log("Level 3");
        }
>>>>>>> Stashed changes
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateDamagePerSecond()
    {
        return damagePerSecondBase * Mathf.Pow(level, 0.6f);
    }
    private float CalculateTargetingRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
