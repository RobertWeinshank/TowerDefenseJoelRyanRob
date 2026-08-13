using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask towerMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private UnityEngine.UI.Button upgradeButton;
    [SerializeField] private UnityEngine.UI.Button upgradeButton2;
    //[SerializeField] private UnityEngine.UI.Button sellButton;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Audio Settings")]
    [SerializeField] public AudioClip ambientLoopClip;
    [SerializeField] public AudioClip shootClip;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bulletsPerSecond = 1f;
    [SerializeField] private int baseUpgradeCost = 40; //upgrade stuff
    [SerializeField] private int baseSellCost = 20;

    public Sprite baseTowerSprite;
    public Sprite upgrade1TowerSprite;
    public Sprite upgrade2TowerSprite;

    private Transform target;
    private float timeUntilFire;

    //upgrade stuff
    private float bulletsPerSecondBase;
    private float targetingRangeBase;
    private int level = 1;

    private AudioSource ambientAudioSource;
    private AudioSource weaponAudioSource;

    private Plot plot;

    //public TowerHealth th;

    private void Start()
    {
        //Upgrade stuff
        bulletsPerSecondBase = bulletsPerSecond;
        targetingRangeBase = targetingRange;
        upgradeButton.onClick.AddListener(UpgradePath1);
        upgradeButton2.onClick.AddListener(UpgradePath2);//anytime you click the upgrade button, calls the upgrade method
        //sellButton.onClick.AddListener(SellTower);
        Plot plot = GetComponent<Plot>();

        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length >= 2)
        {
            ambientAudioSource = sources[0];
            weaponAudioSource = sources[1];
        }
        else if (sources.Length == 1)
        {
            // Fallback safety if only one is attached
            weaponAudioSource = sources[0];
            Debug.LogWarning("Please add a SECOND Audio Source to the " + gameObject.name + " prefab for the ambient loop.");
        }

        // Start playing the ambient sound automatically as soon as the turret is built/spawned
        StartAmbientLoop();
    }

    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else //if there are targets in range, shoot
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bulletsPerSecond)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void StartAmbientLoop()
    {
        if (ambientAudioSource != null && ambientLoopClip != null)
        {
            ambientAudioSource.clip = ambientLoopClip;
            ambientAudioSource.loop = true; // Make it run endlessly
            ambientAudioSource.playOnAwake = false;
            ambientAudioSource.Play();
        }
    }
    private void Shoot()
    {
        if (weaponAudioSource != null && shootClip != null)
        {
            weaponAudioSource.PlayOneShot(shootClip);
        }

        //Debug.Log("PEW PEW");
        if (level == 3)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity); //create a bullet at the bullet firing point
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();

            /*
             * IF YOU NEED TO CHANGE SNIPER DAMAGE LOOK HERE
             * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
             */
            bulletScript.ChangeDamage(5);
            /*
             * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
             * IF YOU NEED TO CHANGE SNIPER DAMAGE LOOK HERE
             * 
             */
            bulletScript.SetTarget(target);
        }
        else
        {
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity); //create a bullet at the bullet firing point
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            bulletScript.SetTarget(target);
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
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg + -90f; // Get the angle between the target and turret (in both x and y) and multiply it by rad2

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);// slowley rotates the turret instead of having it snap to target / back to center
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    public void OpenUpgradeUI()
    {
        if (upgradeUI == null) return;

        upgradeUI.SetActive(true);
    }

    public void SellTower()
    {
        if (upgradeUI == null) return;

        //Plot plot = .GetComponent<Plot>();
        //plot.EmptyPlot();
        //Destroy(gameObject);
        //RaycastHit2D[] towersHits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, towerMask);

        //if (towersHits.Length > 0)
        //{
        //    th.DestroyTowerUnit();
        //    for (int i = 0; i < towersHits.Length; i++)
        //    {
        //        RaycastHit2D hit = towersHits[i];
        //        TowerHealth th = hit.transform.GetComponent<TowerHealth>();
        //        th.DestroyTowerUnit();
        //    }
        //}
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    /*
     * IF YOU NEED TO CHANGE MACHINE GUN UPGRADE LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
    public void UpgradePath1() // Higher Fire Rate
    {
        
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }
        spriteRenderer.sprite = upgrade1TowerSprite;
        LevelManager.main.SpendCurrency(CalculateCost());

        level = 5;

        bulletsPerSecond = CalculateBulletsPerSecond();
        targetingRange = CalculateTargetingRange() / 3f;

        CloseUpgradeUI();
        Destroy(upgradeUI);
        Debug.Log("Machine Gun");
    }
    /*
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * IF YOU NEED TO CHANGE MACHINE GUN UPGRADE LOOK HERE
     * 
     */


    /*
     * IF YOU NEED TO CHANGE SNIPER UPGRADE LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
    public void UpgradePath2() // Sniper
    {
        
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }
        spriteRenderer.sprite = upgrade2TowerSprite;
        LevelManager.main.SpendCurrency(CalculateCost());

        level = 3;

        bulletsPerSecond = CalculateBulletsPerSecond() / 4f;
        targetingRange = CalculateTargetingRange() * 1.2f;

        CloseUpgradeUI();
        Destroy(upgradeUI);
        Debug.Log("Sniper");
    }
    /*
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * IF YOU NEED TO CHANGE SNIPER UPGRADE LOOK HERE
     * 
     */
    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    /*
     * IF YOU NEED TO CHANGE BULLET SPEED LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
    private float CalculateBulletsPerSecond()
    {
        return bulletsPerSecondBase * Mathf.Pow(level, 0.6f);
    }
    /*
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * IF YOU NEED TO CHANGE BULLET SPEED LOOK HERE
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
    private IEnumerator ResetEnemeySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(.5f);

        em.ResetSpeed();
    }

    private void OnDrawGizmosSelected()
    {
        //Handles.color = Color.cyan;
        //Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Plot"))
    //    {
    //        collision.transform.GetComponent<Plot>();
    //    }
            
    //}

}
