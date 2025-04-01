using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoadButton : MonoBehaviour
{
    private Button _button;
    private SaveLoadService _saveLoadService;

    [Inject]
    public void Construct(SaveLoadService saveLoadService)
    {
        _saveLoadService = saveLoadService;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(LoadDialog);
    }

    private void LoadDialog()
    {
        _saveLoadService.LoadData();
    }
    private void OnDestroy()
    {
        _button.onClick.RemoveListener(LoadDialog);
    }
}
