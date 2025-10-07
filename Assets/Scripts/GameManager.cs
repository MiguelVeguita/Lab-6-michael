using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public CharacterData SelectedCharacter { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �Esta es la l�nea clave!
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSelectedCharacter(CharacterData character)
    {
        SelectedCharacter = character;
    }
}