using UnityEngine;
using UnityEngine.SceneManagement;
public class GameButtons : MonoBehaviour
{
    public void ExitGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
