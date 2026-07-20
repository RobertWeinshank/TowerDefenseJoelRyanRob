using UnityEngine;

public class PathCheckpoint : MonoBehaviour
{

    public bool onPlot = false;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PumpJack"))
        {
            Debug.Log("Enemy Pass Checkpoint");
            onPlot = true;
        }
        //Debug.Log("Enemy Pass Checkpoint");
    }
}
 