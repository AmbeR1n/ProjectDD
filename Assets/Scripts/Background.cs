using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [SerializeField] private List<Sprite> backgroundSprites;
    private Sprite _currentBackground;

    private void Start()
    {
        backgroundSprites = Resources.LoadAll<Sprite>("Backgrounds").ToList();
        
        if (backgroundSprites.Count == 0)
        {
            Debug.LogError("No background sprites found in Resources/Backgrounds.");
            return;
        }   
    }
    
    public void ChangeBackground(string bgName)
    {
        FindBackgroundByName(bgName);
        SetBackground(_currentBackground);
    }

    private void SetBackground(Sprite sprite)
    {
        if (sprite == null) return;

        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
            return;
        }
        Debug.LogError("Image component not found on the GameObject.");
    }

    private void FindBackgroundByName(string bgName)
    {
        foreach (var background in backgroundSprites)
        {
            string name = bgName + "_0";
            if (background.name == name)
            {
                _currentBackground = background;
                return;
            }
        }
        _currentBackground = null;
        Debug.LogWarning($"Background with name {bgName} not found.");
    }
}