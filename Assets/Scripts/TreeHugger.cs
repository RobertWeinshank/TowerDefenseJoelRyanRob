using UnityEngine;

public class TreeHugger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;


    [Header("Attribute")]
    [SerializeField] private int healthRestored = 5;
    void Start()
    {
        LevelManager.main.ChangeHealth(-healthRestored);
        //need to change it so health can't go past 100 hp max (or however high we want it).
    }
}
