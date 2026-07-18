using UnityEngine;
using UnityEngine.XR;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int damage = 1;

    //point we want to move to
    private Transform target;
    private int pathIndex = 0;

    private float baseSpeed;

    private void Start()
    {
        baseSpeed = moveSpeed;
        animator = GetComponent<Animator>();
        target = LevelManager.main.path[pathIndex];
    }

    private void Update()
    {
        //If the enemy's position is on the targeted path location, increase pathIndex 
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            

            //If the enemy makes it to the end of the path, destroy the enemy; else set target's location equal to the next target on the path
            if (pathIndex == LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke(); //Call onEnemyDestroy method in Enemyspawner to destroy the enemy
                LevelManager.main.ChangeHealth(damage); //Damage Gaia (the player)
                //Debug.Log("PLAYER TOOK DAMAGE");
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex]; //If they aren't at the end of the path, move to the next point
            }
        }
    }

    private void FixedUpdate()
    {
        //Move the enemy towards the target
        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    //Change the enemy's movement speed
    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    //Reset the enemy's movement speed
    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }

    public void EnemyHit()
    {
        animator.SetBool("isHurt", true);
    }

    public void EndHit()
    {
        animator.SetBool("isHurt", false);
    }
}
