using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Componentes Visuales")]
    public GameObject visuals; // Arrastra aqu� el hijo "Visuals"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // La l�gica de avisar al manager no cambia.
            RaceManager.Instance.CheckpointCrossed(this);
        }
    }

    // Funci�n para encender la parte visual
    public void Activate()
    {
        visuals.SetActive(true);
    }

    // Funci�n para apagar la parte visual
    public void Deactivate()
    {
        visuals.SetActive(false);
    }
}