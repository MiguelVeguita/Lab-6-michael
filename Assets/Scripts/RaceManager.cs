using UnityEngine;
using System.Collections.Generic;

public class RaceManager : Singleton<RaceManager>
{
    [Header("Configuración de Checkpoints")]
    public List<Checkpoint> checkpoints;

    private int currentCheckpointIndex = 0;
    private int currentLap = 1;

    void Start()
    {
        // Al empezar, apagamos todos los checkpoints...
        for (int i = 0; i < checkpoints.Count; i++)
        {
            checkpoints[i].Deactivate();
        }

        // ...y encendemos solo el primero.
        checkpoints[0].Activate();

        Debug.Log("Carrera iniciada. Ve al primer checkpoint.");
    }

    public void CheckpointCrossed(Checkpoint checkpoint)
    {
        // Si el checkpoint cruzado es el correcto...
        if (checkpoints[currentCheckpointIndex] == checkpoint)
        {
            Debug.Log($"¡Correcto! Checkpoint {currentCheckpointIndex + 1} alcanzado.");

            // Apagamos el checkpoint actual.
            checkpoints[currentCheckpointIndex].Deactivate();

            // Si era la meta...
            if (currentCheckpointIndex == checkpoints.Count - 1)
            {
                Debug.Log($"¡Vuelta {currentLap} completada!");
                currentLap++;
                currentCheckpointIndex = 0; // Reiniciamos para la siguiente vuelta
                Debug.Log("Inicia la siguiente vuelta. Ve al primer checkpoint.");
            }
            else
            {
                // Si no, avanzamos al siguiente.
                currentCheckpointIndex++;
            }

            // Activamos el siguiente checkpoint (sea el próximo o el primero de la nueva vuelta).
            checkpoints[currentCheckpointIndex].Activate();
            Debug.Log($"Ve al checkpoint número {currentCheckpointIndex + 1}.");
        }
        else
        {
            // La lógica para checkpoints incorrectos no cambia.
            Debug.Log($"¡Incorrecto! Debes ir al checkpoint {currentCheckpointIndex + 1}.");
        }
    }
}