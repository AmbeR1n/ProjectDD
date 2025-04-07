using UnityEngine;

[CreateAssetMenu(fileName = "Background", menuName = "ScriptableObjects/Background")]
public class BackgroundSO : ScriptableObject
{
    public string backgroundName;
    public Sprite backgroundImage;
    public Color backgroundColor = Color.white;
}   
