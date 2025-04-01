using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public List<Sprite> emotions;
    public string characterName;

    public void ChangeEmotion(string currentExpression)
    {
        foreach (var sprite in emotions)
        {
            if (sprite.name == currentExpression)
            {
                SetEmotion(sprite);
                return;
            }
        }
        Debug.LogWarning($"Expression with name {currentExpression} not found.");
    }

    public void SetTransparent(bool isTransparent)
    {
        var image = GetComponent<Image>();
        if (image != null)
        {
            Color color = image.color;
            color.a = isTransparent ? 0f : 1f;
            image.color = color;
        }
    }

    private void SetEmotion(Sprite sprite)
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
        }
        else
        {
            Debug.LogError("Image component not found on the GameObject.");
        }
    }
}
