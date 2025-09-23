using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Selection/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public GameObject characterPrefab;
    public Sprite characterIcon;
    // Puedes a�adir m�s datos como estad�sticas, habilidades, etc.
}