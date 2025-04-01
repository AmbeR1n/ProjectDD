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
    public GameObject ChoiceButtonsPanel;

    private Story _currentStory;
    private Background _currentBackground;
    private TextAsset _incJson;
    private TextMeshProUGUI _dialogueText;
    private GameObject _choiceButton;
    //private SaveLoadService _saveLoadService;

    private List<TextMeshProUGUI> _choiceButtonText = new();
    private List<Character> _characters = new();

    [Inject]
    public void Construct(DialogueInstaller dialogueInstaller)//, SaveLoadService saveLoadService)
    {
        DialoguePanel = dialogueInstaller.DialoguePanel;
        _dialogueText = dialogueInstaller.DialogueText;
        NameText = dialogueInstaller.NameText;

        ChoiceButtonsPanel = dialogueInstaller.ChoiceButtonsPanel;
        _choiceButton = dialogueInstaller.ChoiceButton;

        _incJson = dialogueInstaller.IncJson;
        //_saveLoadService = saveLoadService;
    }

    private void Awake() =>
        _currentStory = new Story(_incJson.text);

    private Dictionary<string, int> _expressionIndexMapCharacter = new()
    {
        { "tol_1", 0 },
        { "tol_2", 1 },
        { "mak_1", 2 },
        { "mak_2", 3 },
        { "mak_3", 4 },
    };

    void Start()
    {
        _currentBackground = FindFirstObjectByType<Background>();

        foreach (var character in FindObjectsByType<Character>(FindObjectsSortMode.None))
            _characters.Add(character);

        StartDialogue();
    }

    public void StartDialogue()
    {
        //_saveLoadService.LoadData();

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
        }
        else if (!choiceBefore)
            ExitDialogue();
    }   

    private void ShowDialogue()
    {
        _dialogueText.text = _currentStory.Continue();
        //_saveLoadService.SaveData();
        ChangeCharacter();
        ChangeBackground();
    }

    private void ChangeCharacter()
    {
        string characterName = (string)_currentStory.variablesState["CharacterName"];
        string expressionName = (string)_currentStory.variablesState["CharacterExpression"];

        NameText.text = characterName;
        var index = _characters.FindIndex(character => character.characterName.Contains(characterName));

        if (index != -1)
        {
            if (string.IsNullOrEmpty(expressionName))
                _characters[index].SetTransparent(true);
            else if (_expressionIndexMapCharacter.TryGetValue(expressionName, out int expressionIndex))
            {
                _characters[index].SetTransparent(false);
                _characters[index].ChangeEmotion(expressionName);
            }
            else
                Debug.LogWarning($"��������� '{expressionName}' �� ������� � �������!");
        }
    }

    private void ChangeBackground()
    {
        var bgKey = (string)_currentStory.variablesState["BG"];
        _currentBackground.ChangeBG(bgKey);
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
}
