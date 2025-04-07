using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputReader : MonoBehaviour, InputSystem_Actions.IDialogueActions
{
    [SerializeField] private InputSystem_Actions _inputActions;
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
        if (IsPointerOverIgnoredUI())
            return;

        if (context.started && _dialogueManager.DialogPlay)
            _dialogueManager.ContinueStory(_dialogueManager.ChoiceButtonsPanel.activeInHierarchy);
    }

    private bool IsPointerOverIgnoredUI()
{
    // Create a new PointerEventData for the current frame
    PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
    {
        position = Mouse.current.position.ReadValue() // Get the current mouse position
    };

    // Perform a raycast to detect UI elements under the pointer
    var results = new System.Collections.Generic.List<RaycastResult>();
    EventSystem.current.RaycastAll(pointerEventData, results);

    // Check if any of the hit UI elements should be ignored
    foreach (var result in results)
    {
        if (result.gameObject.CompareTag("IgnoreClick"))
        {
            return true; // Ignore this click
        }
    }

    return false; // Allow the click
}
}
