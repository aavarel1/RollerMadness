using UnityEngine;
using UnityEngine.SceneManagement;  // Import the SceneManagement namespace

public class SceneLoader : MonoBehaviour
{
    // Method to load the scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

