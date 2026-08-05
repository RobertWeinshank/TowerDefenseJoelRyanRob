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

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float damagePerSecond = 1f;
    [SerializeField] private int raybeamDamage = 1;
    [SerializeField] private int baseUpgradeCost = 100;

    [Header("Audio")]
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip fireClip;

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
        }
    }

    private void Solarbeam()
    {
        if (target == null || target.gameObject == null) return;

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
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        if (UIManager.main != null) UIManager.main.SetHoveringState(false);
    }

    public void UpgradePath1() 
    {
        if (LevelManager.main == null || CalculateCost() > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateCost());
        level++;

        damagePerSecond = CalculateDamagePerSecond();
        targetingRange = CalculateTargetingRange();

        CloseUpgradeUI();
    }

    public void UpgradePath2() 
    {
        if (LevelManager.main == null || CalculateCost() > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateCost());
        level++; 
        
        CloseUpgradeUI();
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
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
}
