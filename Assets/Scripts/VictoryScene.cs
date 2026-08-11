using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScene : MonoBehaviour
{
    public void OnPracticeAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

