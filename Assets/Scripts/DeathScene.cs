using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour
{
    public void OnRestartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnGiveUpButton()
    {
        Application.Quit();
    }
}
