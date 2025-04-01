using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    public int Index;
    private Button _button;
    private DialogueManager _dialogueManager;
    private UnityAction _clickAction;

    void Start()
    {
        _button = GetComponent<Button>();
        _dialogueManager = FindAnyObjectByType<DialogueManager>();
        _clickAction = new UnityAction(() => _dialogueManager.ChoiceButtonAction(Index));
        _button.onClick.AddListener(_clickAction);
    }
}
