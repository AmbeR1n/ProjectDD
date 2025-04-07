using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private CharacterSO[] _charactersArray;

    private CharacterSO _currentCharacter;
    private Sprite _emotionSprite;

    private void Start()
    {
        _charactersArray = Resources.LoadAll<CharacterSO>("Characters");
    }

    public void ChangeEmotion(string characterName, string currentExpression)
    {
        bool isNameEmpty = string.IsNullOrWhiteSpace(characterName);

        if (isNameEmpty) return;
        
        FindCharacterByName(characterName);
        FindEmotionByName(currentExpression);
        SetEmotion(_emotionSprite);
    }

    private void SetEmotion(Sprite sprite)
    {
        if (sprite == null)
        {
            SetTransparent(true);
            return;
        }

        var image = GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
            SetTransparent(false);
        }
    }

    private void SetTransparent(bool isTransparent)
    {
        var image = GetComponent<Image>();
        if (image != null)
        {
            Color color = image.color;
            color.a = isTransparent ? 0f : 1f;
            image.color = color;
        }
    }

    private void FindCharacterByName(string characterName)
    {
        foreach (var character in _charactersArray)
        {
            if (character.characterName == characterName)
            {
                _currentCharacter = character;
                return;
            }
        }
        Debug.LogWarning($"Character with name {characterName} not found.");
        _currentCharacter = null;
    }

    private void FindEmotionByName(string expressionName)
    {
        if (_currentCharacter == null)
        {
            _emotionSprite = null;
            return;
        }

        foreach (var sprite in _currentCharacter.emotions)
        {
            string name = sprite.name + "_0";
            if (name == expressionName)
            {
                _emotionSprite = sprite;
                return;
            }
        }
        Debug.LogWarning($"Expression with name {expressionName} not found.");
        _emotionSprite = null;
    }
}
