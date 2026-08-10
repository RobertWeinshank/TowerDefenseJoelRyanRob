using UnityEngine;

public class VineEnterExit : MonoBehaviour
{
    public bool inVines = false;

    //public TurretSlowmo slowmo;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        inVines = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inVines = false;
    }
}
