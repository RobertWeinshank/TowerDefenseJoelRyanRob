using System.Collections;
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
    //[SerializeField] private UnityEngine.UI.Button upgradeButton2;
    [SerializeField] private SpriteRenderer towerSpriteRenderer;
    [SerializeField] private SpriteRenderer towerBaseRenderer;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float damagePerSecond = 1f;
    [SerializeField] private int raybeamDamage = 1;
    [SerializeField] private int baseUpgradeCost = 100; //upgrade stuff

    [Header("Audio")]
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip fireClip;


    public Sprite baseTowerSprite;
    public Sprite upgrade1TowerSprite;
    public Sprite upgrade2TowerSprite;

    public Sprite upgrade2BaseSprite;

    private Transform target;
    private float timeUntilFire;

    private LineRenderer line;
    private DistanceJoint2D raybeam;

    //upgrade stuff
    private float damagePerSecondBase;
    private float targetingRangeBase;
    private int level = 1;
    private bool isFiring = false;

    private float damageOverTimeTimer;
    private int fireDoTDamage = 1;

    private void Start()
    {
        raybeam = gameObject.AddComponent<DistanceJoint2D>();
        line = GetComponent<LineRenderer>();

        raybeam.enabled = false;
        line.enabled = false;

        //Upgrade stuff
        damagePerSecondBase = damagePerSecond;
        targetingRangeBase = targetingRange;
        upgradeButton.onClick.AddListener(UpgradePath1); //anytime you click the upgrade button, calls the upgrade method
        //upgradeButton2.onClick.AddListener(UpgradePath2);
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        line.SetPosition(0, firingPoint.position); //Create the line at the fire point

        if (target == null)
        {
            if (isFiring)
            {
                StopRaybeam();
            }
            target = null;
            FindTarget();
            return;
        }

        //RotateTowardsTarget();
        line.SetPosition(0, firingPoint.position);

        if (!CheckTargetIsInRange())
        {
            target = null;
            if (isFiring)
            {
                StopRaybeam(); //If no targets are in range, stop the raybeam cast
            }
             
        }
        else //if there are targets in range, shoot
        {
            timeUntilFire += Time.deltaTime;
            damageOverTimeTimer = 2;
            //FireRaybeam(target); //Display the raybeam at the target in range
            if (!isFiring)
            {
                StartRaybeam(target);
            }
            else
            {
                raybeam.connectedAnchor = target.position;
                line.SetPosition(1, target.position);
            }

            if (timeUntilFire >= 1f / damagePerSecond)
            {               
                Solarbeam();//Deal damage to enemy                
            }
            if (level == 3)
            {
                if (damageOverTimeTimer > 0)
                {
                    WaitForSeconds();
                    damageOverTimeTimer -= Time.deltaTime;
                }
            }

        }
    }

    private void Solarbeam()
    {
        target.gameObject.GetComponent<EnemyHealth>().TakeDamage(raybeamDamage); //call enemeyHealth script to deal raybeam damage
        EnemyHealth enemyHealth = target.gameObject.GetComponent<EnemyHealth>();
        //Debug.Log(target.gameObject.GetComponent<EnemyHealth>().hitPoints + "Hitpoints");
        timeUntilFire = 0f; //Reset fire time
        if (target.gameObject.GetComponent<EnemyHealth>().hitPoints == 0 || target.gameObject.GetComponent<EnemyHealth>().isDestroyed)
        {
            StopRaybeam();//Stop the raybeam if the target's hp is 0 or is destroyed
            target = null;
            return;
        }

        //FireDamage();
        timeUntilFire = 0f;

        // Re-verify immediately if FireDamage finished them off
        if (target == null || target.gameObject == null || enemyHealth.hitPoints <= 0 || enemyHealth.isDestroyed)
        {
            StopRaybeam();
            target = null;
        }
    }

    private void FireDamage()
    {
        if (target != null && target.gameObject != null)
        {
            EnemyHealth enemyHealth = target.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(fireDoTDamage);
            }
        }
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask); //Takes the turret positin, range, direction (our position in vector2), distance from target, and layermask

        if (hits.Length > 0 && hits[0].transform != null)
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
        isFiring = false;
        raybeam.enabled = false;
        line.enabled = false;

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void StartRaybeam(Transform hit)
    {
        isFiring = true;
        raybeam.enabled = true;
        raybeam.connectedAnchor = hit.position;
        line.enabled = true;
        line.SetPosition(1, hit.position);

        if (audioSource != null && fireClip != null)
        {
            audioSource.clip = fireClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void OpenUpgradeUI()
    {
        if (upgradeUI == null) return;

        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    /*
     * IF YOU NEED TO CHANGE INCREASE DAMAGE UPGRADE LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
    public void UpgradePath1() //Ramping Damage
    {
        
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }
        towerSpriteRenderer.sprite = upgrade2TowerSprite;
        towerBaseRenderer.sprite = upgrade2BaseSprite;
        LevelManager.main.SpendCurrency(CalculateCost());

        level = 2;

        damagePerSecond = CalculateDamagePerSecond();
        targetingRange = CalculateTargetingRange();

        CloseUpgradeUI();
        Destroy(upgradeUI);
        //Debug.Log("New BPS: " + damagePerSecond + "\nNew Range: " + targetingRange + "\nNew Cost: " + CalculateCost());
        Debug.Log("Ramping damage");

    }
    /*
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * IF YOU NEED TO CHANGE INCREASE DAMAGE UPGRADE LOOK HERE
     * 
     */



    /*
     * IF YOU NEED TO CHANGE DoT UPGRADE LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
    public void UpgradePath2() //Damage over Time
    {
        
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }
        towerSpriteRenderer.sprite = upgrade2TowerSprite;
        towerBaseRenderer.sprite = upgrade2BaseSprite;
        LevelManager.main.SpendCurrency(CalculateCost());

        level = 3;

        //damagePerSecond = CalculateDamagePerSecond();
        //targetingRange = CalculateTargetingRange();

        CloseUpgradeUI();
        Destroy(upgradeUI);
        //Debug.Log("New BPS: " + damagePerSecond + "\nNew Range: " + targetingRange + "\nNew Cost: " + CalculateCost());
        Debug.Log("Damage over time");
    }
    /*
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * IF YOU NEED TO CHANGE DoT UPGRADE LOOK HERE
     * 
     */


    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    /*
     * IF YOU NEED TO CHANGE DPS (FIRE SPEED) LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
    private float CalculateDamagePerSecond()
    {
        return damagePerSecondBase * Mathf.Pow(level, 0.6f);
    }
    /*
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * IF YOU NEED TO CHANGE DPS (FIRE SPEED) LOOK HERE
     * 
     */



    /*
     * IF YOU NEED TO CHANGE TARGETING RANGE LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
    private float CalculateTargetingRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }
    /*
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * IF YOU NEED TO CHANGE TARGETING RANGE LOOK HERE
     * 
     */

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1f);
        FireDamage();

    }
private void OnDrawGizmosSelected()
    {
        //Handles.color = Color.cyan;
        //Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
