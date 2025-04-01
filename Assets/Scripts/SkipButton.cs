using UnityEngine;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    private Button _button;
    private DialogueManager _dialogueManager;

    void Start()
    {
        _dialogueManager = FindFirstObjectByType<DialogueManager>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SkipDialog);
    }

    private void SkipDialog()
    {
        while (!_dialogueManager.ChoiceButtonsPanel.activeInHierarchy)
        {
            _dialogueManager.ContinueStory(_dialogueManager.ChoiceButtonsPanel.activeInHierarchy);
        }
    }
}
