using UnityEngine;

public class AIManager : MonoBehaviour
{
    public Transform[] Waypoints;
    public Transform[] AISpawnPoints;

    private GameObject[] AIs;

    public GameObject AIPrefab;
    //public GameObject PlayerPrefab;

    public GameObject CharacterSelection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnAI();
    }

    private void SpawnAI()
    {
        AIs = new GameObject[AISpawnPoints.Length];

        for (int i = 0; i < AISpawnPoints.Length; i++)
        {
            AIs[i] = Instantiate(AIPrefab, AISpawnPoints[i]);
            AIs[i].GetComponent<AIOpponentController>().setWaypoints(Waypoints);
            AIs[i].GetComponent<AIOpponentController>().setPlayerTransform(CharacterSelection.GetComponent<CharacterSelection>().GetCharacterPrefab().GetComponent<Transform>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
