using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;

    public bool isDestroyed = false;
    
    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        EnemyMovement em = transform.GetComponent<EnemyMovement>();
        em.EnemyHit();

        //If the enemy's health is 0 and isn't destroyed
        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke(); //Call onEnemyDestroy method in Enemyspawner to destroy the enemy
            LevelManager.main.IncreaseCurrency(currencyWorth); //Increase the player's currency after
            isDestroyed = true; //Set isDestroyed to true to prevent method from being called multiple times
            Destroy(gameObject);
        }
    }
}
