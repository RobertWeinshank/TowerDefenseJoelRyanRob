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
    [SerializeField] private UnityEngine.UI.Button upgradeButton2;
    [SerializeField] private SpriteRenderer towerSpriteRenderer;
    [SerializeField] private SpriteRenderer towerBaseRenderer;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float damagePerSecond = 1f;
    [SerializeField] private int raybeamDamage = 1;
    [SerializeField] private int baseUpgradeCost = 100;

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

        damagePerSecondBase = damagePerSecond;
        targetingRangeBase = targetingRange;
        upgradeButton.onClick.AddListener(UpgradePath1); 
        upgradeButton2.onClick.AddListener(UpgradePath2);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // 1. If target was completely destroyed, clean up the beam and find a new one
        if (target == null || target.gameObject == null)
        {
            if (isFiring)
            {
                StopRaybeam();
            }
            target = null;
            FindTarget();
            return;
        }

        line.SetPosition(0, firingPoint.position);

        // 2. Stick to the current target unless they walk completely out of range
        if (!CheckTargetIsInRange())
        {
            target = null;
            if (isFiring)
            {
                StopRaybeam();
            }
        }
        else 
        {
            RotateTowardsTarget();

            timeUntilFire += Time.deltaTime;
            
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
                Solarbeam(); 
            }
<<<<<<< Updated upstream
=======
            if (level == 3)
            {
                if (damageOverTimeTimer > 0)
                {
                    WaitForSeconds();
                    damageOverTimeTimer -= Time.deltaTime;
                }
            }

>>>>>>> Stashed changes
        }
    }

    private void Solarbeam()
    {
<<<<<<< Updated upstream
        if (target == null || target.gameObject == null) return;
=======
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
>>>>>>> Stashed changes

        EnemyHealth enemyHealth = target.gameObject.GetComponent<EnemyHealth>();
        
        if (enemyHealth != null)
        {
            if (enemyHealth.hitPoints <= 0 || enemyHealth.isDestroyed)
            {
                StopRaybeam();
                target = null;
                return;
            }

            FireDamage();
            timeUntilFire = 0f;

            // Re-verify immediately if FireDamage finished them off
            if (target == null || target.gameObject == null || enemyHealth.hitPoints <= 0 || enemyHealth.isDestroyed)
            {
                StopRaybeam();
                target = null; 
            }
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
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        // FIXED: Explicitly checks the first index element of the raycast collection
        if (hits.Length > 0 && hits[0].transform != null)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
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

    private void StopRaybeam()
    {
        isFiring = false; 
        raybeam.enabled = false;
        line.enabled = false;

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
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
        if (UIManager.main != null) UIManager.main.SetHoveringState(false);
    }

<<<<<<< Updated upstream
    public void UpgradePath1() 
    {
        if (LevelManager.main == null || CalculateCost() > LevelManager.main.currency) return;
=======
    /*
     * IF YOU NEED TO CHANGE INCREASE DAMAGE UPGRADE LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
    public void UpgradePath1() //Ramping Damage
    {
        towerSpriteRenderer.sprite = upgrade1TowerSprite;
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }
>>>>>>> Stashed changes

        LevelManager.main.SpendCurrency(CalculateCost());
        level++;

        damagePerSecond = CalculateDamagePerSecond();
        targetingRange = CalculateTargetingRange();

        CloseUpgradeUI();
<<<<<<< Updated upstream
=======
        Destroy(upgradeUI);
        //Debug.Log("New BPS: " + damagePerSecond + "\nNew Range: " + targetingRange + "\nNew Cost: " + CalculateCost());
        Debug.Log("Ramping damage");

>>>>>>> Stashed changes
    }
    /*
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * IF YOU NEED TO CHANGE INCREASE DAMAGE UPGRADE LOOK HERE
     * 
     */

<<<<<<< Updated upstream
    public void UpgradePath2() 
    {
        if (LevelManager.main == null || CalculateCost() > LevelManager.main.currency) return;
=======


    /*
     * IF YOU NEED TO CHANGE DoT UPGRADE LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
    public void UpgradePath2() //Damage over Time
    {
        towerSpriteRenderer.sprite = upgrade2TowerSprite;
        towerBaseRenderer.sprite = upgrade2BaseSprite;
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }
>>>>>>> Stashed changes

        LevelManager.main.SpendCurrency(CalculateCost());
        level++; 
        
        CloseUpgradeUI();
<<<<<<< Updated upstream
=======
        Destroy(upgradeUI);
        //Debug.Log("New BPS: " + damagePerSecond + "\nNew Range: " + targetingRange + "\nNew Cost: " + CalculateCost());
        Debug.Log("Damage over time");
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
    
=======
    /*
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * IF YOU NEED TO CHANGE DPS (FIRE SPEED) LOOK HERE
     * 
     */



    /*
     * IF YOU NEED TO CHANGE TARGETING RANGE LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
>>>>>>> Stashed changes
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
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
}
