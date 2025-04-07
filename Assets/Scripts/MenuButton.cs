using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    private Button _button;
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SceneToMenu);      
    }
    
     private void OnDestroy()
    {
        _button.onClick.RemoveListener(SceneToMenu);
    }

    private void SceneToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
