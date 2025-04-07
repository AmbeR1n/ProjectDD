using Ink.Runtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class DialogueManager : MonoBehaviour
{
    public bool DialogPlay { get; private set; }
    public Story CurrentStory => _currentStory;
    private const string dialogueProgressKey = "CurrentState";

    public GameObject DialoguePanel;
    public TextMeshProUGUI DialogueText;
    public TextMeshProUGUI NameText;
    public GameObject NameParent;
    public GameObject ChoiceButtonsPanel;

    private Story _currentStory;
    private Background _currentBackground;
    private Character _currentCharacter;

    private TextAsset _incJson;
    private TextMeshProUGUI _dialogueText;
    private GameObject _choiceButton;

    private List<TextMeshProUGUI> _choiceButtonText = new();

    [Inject]
    public void Construct(DialogueInstaller dialogueInstaller)
    {
        DialoguePanel = dialogueInstaller.DialoguePanel;
        _dialogueText = dialogueInstaller.DialogueText;
        NameText = dialogueInstaller.NameText;

        ChoiceButtonsPanel = dialogueInstaller.ChoiceButtonsPanel;
        _choiceButton = dialogueInstaller.ChoiceButton;

        _incJson = dialogueInstaller.IncJson;
    }

    private void Awake()
    {
        _currentStory = new Story(_incJson.text);
        NameParent = NameText.transform.parent.gameObject;
        NameParent.SetActive(false);
        DialoguePanel.SetActive(false);
    }

    void Start()
    {
        _currentBackground = FindFirstObjectByType<Background>();
        _currentCharacter = FindFirstObjectByType<Character>();

        StartDialogue();
    }

    public void StartDialogue()
    {

        DialogPlay = true;
        DialoguePanel.SetActive(true);

        if (PlayerPrefs.GetString(dialogueProgressKey) != "")
        {
            _dialogueText.text = _currentStory.currentText;
            ChangeCharacter();
            ShowChoiceButton();
        }
        else
            ContinueStory();
    }

    public void ContinueStory(bool choiceBefore = false)
    {
        if (_currentStory.canContinue)
        {
            ShowDialogue();
            ShowChoiceButton();
            Debug.Log($"Current character: { _currentStory.variablesState["CharacterName"]} with expression: {_currentStory.variablesState["CharacterExpression"]}");
            Debug.Log($"Current background: { _currentStory.variablesState["BG"]}");
        }
        else if (!choiceBefore)
            ExitDialogue();
    }   

    private void ShowDialogue()
    {
        _dialogueText.text = _currentStory.Continue();
        ChangeCharacter();
        ChangeBackground();
    }

    private void ChangeCharacter()
    {
        string characterName = (string)_currentStory.variablesState["CharacterName"];
        string expressionName = (string)_currentStory.variablesState["CharacterExpression"];

        _currentCharacter.ChangeEmotion(characterName, expressionName);

        NameText.text = characterName;

        NameParent.SetActive(!string.IsNullOrWhiteSpace(characterName));
    }

    private void ChangeBackground()
    {
        var backgroundName = (string)_currentStory.variablesState["BG"];
        _currentBackground.ChangeBackground(backgroundName);
    }

    private void ShowChoiceButton()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;
        ChoiceButtonsPanel.SetActive(currentChoices.Count != 0);

        if (currentChoices.Count <= 0)
            return;

        ClearChoiceButton();

        for (int i = 0; i < currentChoices.Count; i++)
        {
            GameObject choice = Instantiate(_choiceButton);
            choice.GetComponent<ButtonAction>().Index = i;
            choice.transform.SetParent(ChoiceButtonsPanel.transform);

            TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.text = currentChoices[i].text;
            _choiceButtonText.Add(choiceText);

        }
    }

    private void ClearChoiceButton()
    {
        foreach (Transform child in ChoiceButtonsPanel.transform)
            Destroy(child.gameObject);
        _choiceButtonText.Clear();
    }

    public void ChoiceButtonAction(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory(true);
    }

    private void ExitDialogue()
    {
        DialogPlay = false;
        DialoguePanel.SetActive(false);

        MovingNextScene();
    }

    private void MovingNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex <= SceneManager.sceneCount)
            SceneManager.LoadScene(nextSceneIndex);
    }

    public void UpdateDialogue()
    {
        _dialogueText.text = _currentStory.currentText;
        ChangeCharacter();
        ChangeBackground();
        ShowChoiceButton();
    }
}
