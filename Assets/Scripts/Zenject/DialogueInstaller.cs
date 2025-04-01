using Ink.Runtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueInstaller : MonoBehaviour
{
    [HideInInspector] public GameObject DialoguePanel;
    [HideInInspector] public TextMeshProUGUI DialogueText;
    [HideInInspector] public TextMeshProUGUI NameText;
    [HideInInspector] public GameObject ChoiceButtonsPanel;

    public TextAsset IncJson;
    public GameObject ChoiceButton;
    public DialogueManager DialogueManager;
}
