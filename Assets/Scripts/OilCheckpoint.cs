using UnityEditor.Build;
using UnityEngine;

public class OilCheckpoint : MonoBehaviour
{
    [Header("References")]
<<<<<<< Updated upstream
    [SerializeField] private Plot oilRigPlot;
=======
    [SerializeField] private GameObject oilRigPlot;
>>>>>>> Stashed changes
    [SerializeField] private GameObject oilRig;

    [Header("Attributes")]
    public bool onPlot = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PumpJack") && !onPlot)
        {
            Debug.Log("PumpJack Enter");
            Destroy(collision.gameObject);
            //EnemyHealth eh = collision.GetComponent<EnemyHealth>();
            //eh.TakeDamage(20);
            EnemySpawner.onEnemyDestroy.Invoke();
            onPlot = true;
<<<<<<< Updated upstream
            Instantiate(oilRig, oilRigPlot.getPlot(), Quaternion.identity);
=======
            Instantiate(oilRig, oilRigPlot.transform.position, Quaternion.identity);
>>>>>>> Stashed changes
            
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PumpJackEnemy") && onPlot)
        {
            onPlot = false;
        }
    }
}
