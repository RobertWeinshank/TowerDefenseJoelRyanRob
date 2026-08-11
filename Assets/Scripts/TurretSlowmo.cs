using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretSlowmo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private UnityEngine.UI.Button upgradeButton;
    [SerializeField] private UnityEngine.UI.Button upgradeButton2;
    [SerializeField] private SpriteRenderer towerSpriteRenderer;
    [SerializeField] private SpriteRenderer vineSpriteRenderer;

    [Header("Audio Settings")]
    [SerializeField] public AudioClip slowmoClip; // Drag your slow-mo / pulse sound here!
    private AudioSource audioSource;               // Captured automatically in Start()

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 2f;
    [SerializeField] private float attackSpeed = 1.2f;
    [SerializeField] private float freezeTime = 0.3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int baseUpgradeCost = 40; //upgrade stuff

    public Sprite baseTowerSprite;
    public Sprite upgrade1TowerSprite;
    public Sprite upgrade2TowerSprite;

    public Sprite baseVineSprite;
    public Sprite upgrade1VineSprite;
    public Sprite upgrade2VineSprite;

    private float timeUntilFire;

    //upgrade stuff
    private float attackSpeedBase;
    private float targetingRangeBase;
    private int level = 1;
    public bool slowEnemy = true;

    public VineEnterExit enterExit;

    // Cooldown tracker to prevent Level 3 from spamming the audio card 60 times a second
    private float audioCooldownTimer; 

    private void Start()
    {
        //Upgrade stuff
        attackSpeedBase = attackSpeed;
        targetingRangeBase = targetingRange;
        upgradeButton.onClick.AddListener(UpgradePath1); //anytime you click the upgrade button, calls the upgrade method
        upgradeButton2.onClick.AddListener(UpgradePath2);

        // FIX: Automatically grabs the AudioSource attached to this Prefab clone!
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
<<<<<<< Updated upstream
        // Track the audio cooldown over time
        if (audioCooldownTimer > 0)
        {
            audioCooldownTimer -= Time.deltaTime;
        }
=======
       
>>>>>>> Stashed changes

        if (level == 3)
        {
            //FreezeEnemies();
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / attackSpeed)
            {
                FreezeEnemies();
                timeUntilFire = 0f;
            }
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / attackSpeed)
            {
                FreezeEnemies();
                timeUntilFire = 0f;
            }
        }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask); //Takes the turret positin, range, direction (our position in vector2), distance from target, and layermask

        if (hits.Length > 0)
        {
            // AUDIO TRIGGER: Play the sound effect once per firing pulse
            // If level == 3, the cooldown ensures it only plays once every 0.3 seconds instead of tearing up the speakers
            if (audioSource != null && slowmoClip != null && audioCooldownTimer <= 0)
            {
                audioSource.PlayOneShot(slowmoClip);
                audioCooldownTimer = (level == 3) ? 0.3f : 0.05f; 
            }

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>(); //Gets the enemy movement script of any enemy hit by the raycast

                EnemyHealth eh = hits[i].transform.GetComponent<EnemyHealth>(); //Damages the enemy overtime
                //Debug.Log("Enemy taking rain damage");
                if (level == 3)
                {
                    em.UpdateSpeed(0.5f);
                    if (!enterExit.inVines)
                    {
                        StartCoroutine(ResetEnemeySpeed(em));
                        //em.ResetSpeed();
                    }
                }
                else if (level == 2)
                {
                    //StartCoroutine(ResetEnemeySpeed(em)); //pass the method resetEnemySpeed
                    eh.TakeDamage(damage);
                }
                else
                {
                    //em.UpdateSpeed(0.5f);
                    eh.TakeDamage(damage);
                    if (!slowEnemy)
                    {
                        //StartCoroutine(ResetEnemeySpeed(em));
                    }
                }
            }
        }
    }

    private IEnumerator ResetEnemeySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();
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

    public void UpgradePath1() //Lethal Damage
    {
        towerSpriteRenderer.sprite = upgrade1TowerSprite;
        vineSpriteRenderer.sprite = upgrade1VineSprite;

        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(CalculateCost());

        level = 2;

        attackSpeed = CalculateAttackSpeed();
        targetingRange = CalculateTargetingRange();

        CloseUpgradeUI();
        Destroy(upgradeUI);

        Debug.Log("Lethal");
    }
    public void UpgradePath2() //Super Slow
    {
        towerSpriteRenderer.sprite = upgrade2TowerSprite;
        vineSpriteRenderer.sprite = upgrade2VineSprite;

        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(CalculateCost());

        level = 3;

        attackSpeed = CalculateAttackSpeed();
        targetingRange = CalculateTargetingRange();

        CloseUpgradeUI();
        Destroy(upgradeUI);

        Debug.Log("Super Slow");
    }
    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateAttackSpeed()
    {
        return attackSpeedBase * Mathf.Pow(level, 0.6f);
    }
    private float CalculateTargetingRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == 6)
    //    {
    //        slowEnemy = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == 6)
    //    {
    //       slowEnemy = false;
    //    }
    //}
    private void OnDrawGizmosSelected()
    {
        //Handles.color = Color.cyan;
        //Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
