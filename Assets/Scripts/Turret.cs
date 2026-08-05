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
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private UnityEngine.UI.Button upgradeButton;
    [SerializeField] private UnityEngine.UI.Button upgradeButton2;

    [Header("Audio Settings")]
    [SerializeField] public AudioClip ambientLoopClip; // Drag your constant/always-on water loop here!
    [SerializeField] public AudioClip shootClip;       // Drag your bullet fire/splash sound here!

    private AudioSource ambientAudioSource; // Plays the always-on loop
    private AudioSource weaponAudioSource;  // Plays the shot sounds

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bulletsPerSecond = 1f;
    [SerializeField] private int baseUpgradeCost = 100; //upgrade stuff

    private Transform target;
    private float timeUntilFire;

    //upgrade stuff
    private float bulletsPerSecondBase;
    private float targetingRangeBase;
    private int level = 1;

    private void Start()
    {
        //Upgrade stuff
        bulletsPerSecondBase = bulletsPerSecond;
        targetingRangeBase = targetingRange;
        upgradeButton.onClick.AddListener(UpgradePath1);
        upgradeButton2.onClick.AddListener(UpgradePath2);

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
            bulletScript.ChangeDamage(5);
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
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void UpgradePath1() 
    {
        if (CalculateCost() > LevelManager.main.currency) return;
        LevelManager.main.SpendCurrency(CalculateCost());
        level = 5;
        bulletsPerSecond = CalculateBulletsPerSecond();
        CloseUpgradeUI();
    }

    public void UpgradePath2() 
    {
        if (CalculateCost() > LevelManager.main.currency) return;
        LevelManager.main.SpendCurrency(CalculateCost());
        level = 3;
        bulletsPerSecond = CalculateBulletsPerSecond()/8f;
        targetingRange = CalculateTargetingRange()*5f;
        CloseUpgradeUI();
    }

    private int CalculateCost() => Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    private float CalculateBulletsPerSecond() => bulletsPerSecondBase * Mathf.Pow(level, 0.6f);
    private float CalculateTargetingRange() => targetingRangeBase * Mathf.Pow(level, 0.4f);

    private IEnumerator ResetEnemeySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(.5f);
        em.ResetSpeed();
    }
}
