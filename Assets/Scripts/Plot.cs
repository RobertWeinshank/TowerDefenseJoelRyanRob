using UnityEngine;

public class Plot : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
 
    private GameObject tower;
    private Color startColor;

    //Set plot color to starting color
    private void Start()
    {
        startColor = sr.color;
    }

    //If the mouse hovers over the plot, change the color
    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    //Chagne color back to starting color after exiting plot
    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    //On mouse click, call this function
    private void OnMouseDown()
    {
        Debug.Log("Build tower here" + name);
        //If plot already has a tower, ignore 
        if (tower != null) return;
        
        //creates the tower by getting the tower you want, places it on the plot position in the correct rotation)
        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("YOURE BROKE");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
         
    }
}
