using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public CharacterData[] allCharacters;
    private DoublyLinkedList<CharacterData> characterList;
    private DoublyLinkedListNode<CharacterData> currentNode;

    [Header("UI Elements")]
    public TextMeshProUGUI characterNameText;
    public Image characterIconImage;
    public GameObject characterDisplayParent;

    private GameObject currentCharacterModel;

    void Start()
    {
        characterList = new DoublyLinkedList<CharacterData>();
        foreach (var character in allCharacters)
        {
            characterList.Add(character);
        }

        if (characterList.Head != null)
        {
            currentNode = characterList.Head;
            UpdateCharacterDisplay();
        }
    }

    public void NextCharacter()
    {
        if (currentNode != null)
        {
            currentNode = currentNode.Next;
            UpdateCharacterDisplay();
        }
    }

    public void PreviousCharacter()
    {
        if (currentNode != null)
        {
            currentNode = currentNode.Previous;
            UpdateCharacterDisplay();
        }
    }

    private void UpdateCharacterDisplay()
    {
        if (currentCharacterModel != null)
        {
            Destroy(currentCharacterModel);
        }

        characterNameText.text = currentNode.Data.characterName;
        characterIconImage.sprite = currentNode.Data.characterIcon;
        currentCharacterModel = Instantiate(currentNode.Data.characterPrefab, characterDisplayParent.transform);
    }
    public void SelectCharacter()
    {
        if (currentNode != null && GameManager.Instance != null)
        {
            GameManager.Instance.SetSelectedCharacter(currentNode.Data);

           // SceneManager.LoadScene("GameScene");
        }
    }
}