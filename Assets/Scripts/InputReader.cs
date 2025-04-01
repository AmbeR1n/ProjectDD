using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, InputSystem_Actions.IDialogueActions
{
    InputSystem_Actions _inputActions;
    DialogueManager _dialogueManager;

    private void OnEnable()
    {
        _dialogueManager = FindAnyObjectByType<DialogueManager>();

        if (_inputActions != null)
            return;

        _inputActions = new InputSystem_Actions();
        _inputActions.Dialogue.SetCallbacks(this);
        _inputActions.Dialogue.Enable();
    }

    private void OnDisable() =>
        _inputActions.Dialogue.Disable();

    public void OnNextPhrase(InputAction.CallbackContext context)
    {
        if (context.started && _dialogueManager.DialogPlay)
            _dialogueManager.ContinueStory(_dialogueManager.ChoiceButtonsPanel.activeInHierarchy);
    }
}
