using System.Collections;
using UnityEngine;

public class Plot : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
 
    public GameObject towerObj;
    public GameObject seedPrefab;
    public Turret turret;
    public TurretSlowmo slowmo;
    public SolarRayTower solar;
    public TreeHugger tree;
    private Color startColor;
    private float seedTime;


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
        if(UIManager.main.IsHoveringUI())
        {
            return;
        }
        
        //Debug.Log("Build tower here" + name);
        //If plot already has a tower, ignore 
        if (towerObj != null)
        {
            if (towerObj.GetComponent<Turret>())
            {
                //Debug.Log("THIS IS A BASIC TOWER");
                turret.OpenUpgradeUI();
            }
            else if (towerObj.GetComponent<TurretSlowmo>())
            {
                //Debug.Log("THIS IS A SLOWMO TOWER");
                slowmo.OpenUpgradeUI();
            }
            else if (towerObj.GetComponent<SolarRayTower>())
            {
                solar.OpenUpgradeUI();
            }
            else if (towerObj.GetComponent<TreeHugger>())
            {
                tree.OpenUpgradeUI();
            }
            
            
            return;
        }
        
        
        //creates the tower by getting the tower you want, places it on the plot position in the correct rotation)
        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            //Debug.Log("YOURE BROKE");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);
        StartCoroutine(WaitForSeed());
    }

    IEnumerator WaitForSeed()
    {
        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        seedPrefab = Instantiate(seedPrefab, this.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(3f);
        towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
        slowmo = towerObj.GetComponent<TurretSlowmo>();
        solar = towerObj.GetComponent<SolarRayTower>();
        tree = towerObj.GetComponent<TreeHugger>();
    }

    public Vector3 getPlot()
    {
        return transform.position;
    }
}
