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

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float attackSpeed = 4f;
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int baseUpgradeCost = 100; //upgrade stuff

    private float timeUntilFire;

    //upgrade stuff
    private float attackSpeedBase;
    private float targetingRangeBase;
    private int level = 1;
    private bool slowEnemy = true;

    private void Start()
    {
        //Upgrade stuff
        attackSpeedBase = attackSpeed;
        targetingRangeBase = targetingRange;
        upgradeButton.onClick.AddListener(UpgradePath1); //anytime you click the upgrade button, calls the upgrade method
        upgradeButton2.onClick.AddListener(UpgradePath2);
    }

    void Update()
    {

        if (level == 3)
        {
            FreezeEnemies();
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
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>(); //Gets the enemy movement script of any enemy hit by the raycast

                EnemyHealth eh = hits[i].transform.GetComponent<EnemyHealth>(); //Damages the enemy overtime
                //Debug.Log("Enemy taking rain damage");
                if (level == 3)
                {
                    em.UpdateSpeed(0.5f);
                    if (!slowEnemy)
                    {
                        StartCoroutine(ResetEnemeySpeed(em));
                    }
                }
                else if (level == 2)
                {
                    //StartCoroutine(ResetEnemeySpeed(em)); //pass the method resetEnemySpeed
                    eh.TakeDamage(damage);
                }
                else
                {
                    em.UpdateSpeed(0.5f);
                    eh.TakeDamage(damage);
                    if (!slowEnemy)
                    {
                        StartCoroutine(ResetEnemeySpeed(em));
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
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void UpgradePath1() //Lethal Damage
    {
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(CalculateCost());

        level = 2;

        attackSpeed = CalculateAttackSpeed();
        targetingRange = CalculateTargetingRange();

        CloseUpgradeUI();

        Debug.Log("Lethal");
    }
    public void UpgradePath2() //Super Slow
    {
        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(CalculateCost());

        level = 3;

        attackSpeed = CalculateAttackSpeed();
        targetingRange = CalculateTargetingRange();

        CloseUpgradeUI();

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            slowEnemy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
           slowEnemy = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        //Handles.color = Color.cyan;
        //Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
