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
    [SerializeField] private UnityEngine.UI.Button sellButton;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Audio Settings")]
    [SerializeField] public AudioClip ambientLoopClip; // Drag your constant/always-on water loop here!
    [SerializeField] public AudioClip shootClip;       // Drag your bullet fire/splash sound here!

    private AudioSource ambientAudioSource; // Plays the always-on loop
    private AudioSource weaponAudioSource;  // Plays the shot sounds

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

<<<<<<< Updated upstream
=======
    private AudioSource ambientAudioSource;
    private AudioSource weaponAudioSource;

    private Plot plot;

    //public TowerHealth th;

>>>>>>> Stashed changes
    private void Start()
    {
        //Upgrade stuff
        bulletsPerSecondBase = bulletsPerSecond;
        targetingRangeBase = targetingRange;
        upgradeButton.onClick.AddListener(UpgradePath1);
<<<<<<< Updated upstream
        upgradeButton2.onClick.AddListener(UpgradePath2);
=======
        upgradeButton2.onClick.AddListener(UpgradePath2);//anytime you click the upgrade button, calls the upgrade method
        sellButton.onClick.AddListener(SellTower);
        Plot plot = GetComponent<Plot>();
>>>>>>> Stashed changes

        // FIX: Grabs both attached AudioSources cleanly
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
        else 
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
        // AUDIO TRIGGER: Plays the firing sound independently from the weapon speaker
        if (weaponAudioSource != null && shootClip != null)
        {
            weaponAudioSource.PlayOneShot(shootClip);
        }

        if (level == 3)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity); 
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
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity); 
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            bulletScript.SetTarget(target);
        }
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask); 

        if (hits.Length > 0)
        {
            target = hits[0].transform; 
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg + -90f; 

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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

<<<<<<< Updated upstream
    public void UpgradePath1() 
    {
        if (CalculateCost() > LevelManager.main.currency) return;
=======
    /*
     * IF YOU NEED TO CHANGE MACHINE GUN UPGRADE LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
    public void UpgradePath1() // Higher Fire Rate
    {
        spriteRenderer.sprite = upgrade1TowerSprite;
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }

>>>>>>> Stashed changes
        LevelManager.main.SpendCurrency(CalculateCost());
        level = 5;
        bulletsPerSecond = CalculateBulletsPerSecond();
<<<<<<< Updated upstream
=======
        targetingRange = CalculateTargetingRange() / 3f;

>>>>>>> Stashed changes
        CloseUpgradeUI();
    }
    /*
     * ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
     * IF YOU NEED TO CHANGE MACHINE GUN UPGRADE LOOK HERE
     * 
     */

<<<<<<< Updated upstream
    public void UpgradePath2() 
    {
        if (CalculateCost() > LevelManager.main.currency) return;
=======

    /*
     * IF YOU NEED TO CHANGE SNIPER UPGRADE LOOK HERE
     * vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv 
     */
    public void UpgradePath2() // Sniper
    {
        spriteRenderer.sprite = upgrade2TowerSprite;
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }

>>>>>>> Stashed changes
        LevelManager.main.SpendCurrency(CalculateCost());
        level = 3;
<<<<<<< Updated upstream
        bulletsPerSecond = CalculateBulletsPerSecond()/8f;
        targetingRange = CalculateTargetingRange()*5f;
=======

        bulletsPerSecond = CalculateBulletsPerSecond() / 3.5f;
        targetingRange = CalculateTargetingRange() * 1.75f;

>>>>>>> Stashed changes
        CloseUpgradeUI();
    }
<<<<<<< Updated upstream

    private int CalculateCost() => Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    private float CalculateBulletsPerSecond() => bulletsPerSecondBase * Mathf.Pow(level, 0.6f);
    private float CalculateTargetingRange() => targetingRangeBase * Mathf.Pow(level, 0.4f);

=======
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
>>>>>>> Stashed changes
    private IEnumerator ResetEnemeySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(.5f);
        em.ResetSpeed();
    }
<<<<<<< Updated upstream
=======

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

>>>>>>> Stashed changes
}
