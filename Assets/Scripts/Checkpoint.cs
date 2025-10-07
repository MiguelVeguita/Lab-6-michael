using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Componentes Visuales")]
    public GameObject visuals; // Arrastra aquí el hijo "Visuals"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // La lógica de avisar al manager no cambia.
            RaceManager.Instance.CheckpointCrossed(this);
        }
    }

    // Función para encender la parte visual
    public void Activate()
    {
        visuals.SetActive(true);
    }

    // Función para apagar la parte visual
    public void Deactivate()
    {
        visuals.SetActive(false);
    }
}