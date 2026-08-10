using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroductoryStory : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
