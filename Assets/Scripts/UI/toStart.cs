using UnityEngine;
using UnityEngine.SceneManagement;

public class toStart : MonoBehaviour
{
    public string sceneName = "titleScreen";

    public void ChangeSceneByName(string titleScreen)
    {
        SceneManager.LoadScene(titleScreen);
    }
}