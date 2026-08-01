using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public bool destroyTower = false;

    public void DestroyTowerUnit()
    {
        Destroy(gameObject);
    }
}
