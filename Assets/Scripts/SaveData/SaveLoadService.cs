using UnityEngine;

public class SaveLoadService : ISaveLoadService
{
    private const string dialogueProgressKey = "CurrentState";

    private DialogueManager _dialogueManager;
    private string _jsonData;

    public SaveLoadService(DialogueInstaller dialogueInstaller)
    {
        _dialogueManager = dialogueInstaller.DialogueManager;
    }

    public void SaveData()
    {
        _jsonData = _dialogueManager.CurrentStory.state.ToJson();
        PlayerPrefs.SetString(dialogueProgressKey, _jsonData);
    }

    public void LoadData()
    {
        if (PlayerPrefs.GetString(dialogueProgressKey) != "")
        {
            _jsonData = PlayerPrefs.GetString(dialogueProgressKey);
            _dialogueManager.CurrentStory.state.LoadJson(_jsonData);
        }
    }
}
