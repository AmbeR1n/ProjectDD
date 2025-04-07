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
            if (_dialogueManager.CurrentStory.canContinue)
            {
                _dialogueManager.ContinueStory(_dialogueManager.DialoguePanel.activeInHierarchy);
            }
            else
            {
                break;
            }
        }
    }
}
