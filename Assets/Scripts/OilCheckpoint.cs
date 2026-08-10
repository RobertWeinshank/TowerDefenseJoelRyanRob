
using UnityEngine;

public class OilCheckpoint : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject oilRigPlot;
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
            Instantiate(oilRig, oilRigPlot.transform.position, Quaternion.identity);
            
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
