using UnityEngine;

public class GameSpeedButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HalfSpeedButton()
    {
        Time.timeScale = .5f;
    }

    public void FullSpeedButton()
    {
        Time.timeScale = 1f;
    }

    public void DoubleSpeedButton()
    {
        Time.timeScale = 2f;
    }
}
