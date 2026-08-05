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
    [SerializeField] private int baseUpgradeCost = 100; //upgrade stuff

    [Header("Audio")]
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip fireClip;

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
        upgradeButton2.onClick.AddListener(UpgradePath2);
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
{
    line.SetPosition(0, firingPoint.position);

    if (target == null)
    {
        FindTarget();
        return;
    }

    if (!CheckTargetIsInRange())
    {
        target = null;
        
        // Only trigger the stop logic ONCE when target drops out of range
        if (isFiring)
        {
            StopRaybeam();
        }
    }
    else 
    {
        timeUntilFire += Time.deltaTime;
        damageOverTimeTimer = 2;
        
        // FIXED: Only trigger the start logic ONCE when firing begins
        if (!isFiring)
        {
            StartRaybeam(target);
        }
        else
        {
            // Continuously update the laser positions while active
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
        if (target.gameObject.GetComponent<EnemyHealth>().hitPoints == 0 || target.gameObject.GetComponent<EnemyHealth>().isDestroyed)
    {
        StopRaybeam(); // Safely resets audio trackers when enemy dies
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

 private void StartRaybeam(Transform hit)
{
    isFiring = true; // Flips the state tracker on
    raybeam.enabled = true;
    raybeam.connectedAnchor = hit.position;
    line.enabled = true;
    line.SetPosition(1, hit.position);
 
    if (audioSource != null && fireClip != null)
    {
        Debug.Log("LASER AUDIO STARTING!"); // This will now fire cleanly!
        audioSource.clip = fireClip;
        audioSource.loop = true; 
        audioSource.Play();
    }
}

private void StopRaybeam()
{
    isFiring = false; // Flips the state tracker off
    raybeam.enabled = false;
    line.enabled = false;

    if (audioSource != null && audioSource.isPlaying)
    {
        Debug.Log("LASER AUDIO STOPPING!");
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
        UIManager.main.SetHoveringState(false);
    }

    public void UpgradePath1() //Ramping Damage
    {
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(CalculateCost());

        level = 2;

        damagePerSecond = CalculateDamagePerSecond();
        targetingRange = CalculateTargetingRange();

        CloseUpgradeUI();
        //Debug.Log("New BPS: " + damagePerSecond + "\nNew Range: " + targetingRange + "\nNew Cost: " + CalculateCost());
        Debug.Log("Ramping damage");

    }

    public void UpgradePath2() //Damage over Time
    {
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(CalculateCost());

        level = 3;

        //damagePerSecond = CalculateDamagePerSecond();
        //targetingRange = CalculateTargetingRange();

        CloseUpgradeUI();
        //Debug.Log("New BPS: " + damagePerSecond + "\nNew Range: " + targetingRange + "\nNew Cost: " + CalculateCost());
        Debug.Log("Damage over time");
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
        //Handles.color = Color.cyan;
        //Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
