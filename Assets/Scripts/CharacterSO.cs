using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character")]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public Sprite[] emotions;
}
