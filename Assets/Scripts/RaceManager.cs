using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class RaceManager : Singleton<RaceManager>
{
    [Header("Configuración de Carrera")]
    public float timeLimitInSeconds = 120f; 
    public GameObject winPanel; 
    public GameObject defeatPanel;
    public GameObject TimerTextPanel;

    public TextMeshProUGUI timerText;

    [Header("Configuración de Checkpoints")]
    public List<Checkpoint> checkpoints;

    private int currentCheckpointIndex = 0;
    private int currentLap = 1;
    private float currentTime;
    public bool raceIsActive = false;

    void Start()
    {
        winPanel.SetActive(false);
        defeatPanel.SetActive(false);
        for (int i = 0; i < checkpoints.Count; i++)
        {
            checkpoints[i].Deactivate();
        }

        checkpoints[0].Activate();
        currentTime = timeLimitInSeconds;
        Debug.Log("Carrera iniciada. Ve al primer checkpoint.");
    }
    private void OnEnable()
    {
        Meta.ganaste += WinRace;
        Meta.perdiste += LoseRace;
        FrameCountdownUI.GoRacing += Go;
        Item.PowerUp += TimeAdd;
    }

    private void OnDisable()
    {
        Meta.ganaste -= WinRace;
        Meta.perdiste -= LoseRace;
        FrameCountdownUI.GoRacing -= Go;
        Item.PowerUp -= TimeAdd;
    }
    void Update()
    {
        if (raceIsActive)
        {
            Debug.Log("aaaa");
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else 
            {
                currentTime = 0;
                LoseRace();
            }

            DisplayTime(currentTime);
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void WinRace()
    {
        raceIsActive = false;
        winPanel.SetActive(true); 
        Debug.Log("¡GANASTE!");
         Time.timeScale = 0f;
    }

    void LoseRace()
    {
        raceIsActive = false; 
        defeatPanel.SetActive(true); 
        Debug.Log("¡PERDISTE!");
         Time.timeScale = 0f;
    }
    void Go()
    {
        raceIsActive= true;
        TimerTextPanel.SetActive(true);
    }
    void TimeAdd()
    {
        currentTime = currentTime + 5;
    }
    public void CheckpointCrossed(Checkpoint checkpoint)
    {
        if (!raceIsActive) return;

        if (checkpoints[currentCheckpointIndex] == checkpoint)
        {
            Debug.Log($"¡Correcto! Checkpoint {currentCheckpointIndex + 1} alcanzado.");

            checkpoints[currentCheckpointIndex].Deactivate();

            if (currentCheckpointIndex == checkpoints.Count - 1)
            {
               // WinRace();
                /*Debug.Log($"¡Vuelta {currentLap} completada!");
                currentLap++;
                currentCheckpointIndex = 0;
                Debug.Log("Inicia la siguiente vuelta. Ve al primer checkpoint.");*/
            }
            else
            {
                currentCheckpointIndex++;
            }

            checkpoints[currentCheckpointIndex].Activate();
            Debug.Log($"Ve al checkpoint número {currentCheckpointIndex + 1}.");
        }
        else
        {
            Debug.Log($"¡Incorrecto! Debes ir al checkpoint {currentCheckpointIndex + 1}.");
        }
    }
}