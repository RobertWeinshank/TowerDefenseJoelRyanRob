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
    public Transform target;
    public Transform[] targetPath;
    public int pathIndex = 0;
    public LevelManager.PathType pathType = LevelManager.PathType.Primary;

    private float baseSpeed;

    public bool started = false;

    private void Start()
    {
        baseSpeed = moveSpeed;
        animator = GetComponent<Animator>();
        //target = LevelManager.main.path[pathIndex];
    }

    public void SetPathType(LevelManager.PathType _pathType)
    {
        pathType = _pathType;
        targetPath = LevelManager.main.GetPath(pathType);
        //Debug.Log("Receive path " + targetPath);
        //for (int i = 0; i < targetPath.Length; ++i)
        //{
        //    Debug.Log("    Transform[" + i + "]: " + targetPath[i]);
        //}
        target = targetPath[pathIndex];
        started = true;
        //Debug.Log("Started: " + started);
    } 

    private void Update()
    {
        if (!started)
        {
            return;
        }
        Debug.Log(started);

        //If the enemy's position is on the targeted path location, increase pathIndex
        float dist = Vector2.Distance(target.position, transform.position);
        //Debug.Log("targetIndex " + pathIndex + " distance: " + dist);
        if (dist <= 0.1f)
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
                target = targetPath[pathIndex]; //If they aren't at the end of the path, move to the next point
            }
        }
    }

    private void FixedUpdate()
    {
        if (!started)
        {
            return;
        }

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
