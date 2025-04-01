using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [SerializeField] private List<Sprite> backgroundSprites;

    public void ChangeBG(string bgName)
    {
        foreach (var sprite in backgroundSprites)
        {
            if (sprite.name == bgName)
            {
                SetBackground(sprite);
                return;
            }
        }
        Debug.LogWarning($"Background with name {bgName} not found.");
    }
    private void SetBackground(Sprite sprite)
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