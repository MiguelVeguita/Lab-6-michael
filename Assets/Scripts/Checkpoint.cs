using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Componentes Visuales")]
    public GameObject visuals; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RaceManager.Instance.CheckpointCrossed(this);
        }
    }

    public void Activate()
    {
        visuals.SetActive(true);
    }

    public void Deactivate()
    {
        visuals.SetActive(false);
    }
}