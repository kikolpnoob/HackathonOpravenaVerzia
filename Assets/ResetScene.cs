using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    public void ResetActiveScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
