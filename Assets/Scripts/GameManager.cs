using UnityEngine;
using System.Collections;

public enum RacePhase { PreRace, Countdown, Go, Racing }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public CharacterData SelectedCharacter { get; private set; }

    public RacePhase Phase { get; private set; } = RacePhase.PreRace;
    public int CurrentCountdown { get; private set; } = 0;
    public bool GoShown { get; private set; } = false;

    public event System.Action<int> OnCountdownTick;
    public event System.Action OnGo;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void SetSelectedCharacter(CharacterData character) => SelectedCharacter = character;

    public void StartRaceCountdown(float stepSeconds = 1f)
    {
        if (Phase != RacePhase.PreRace) return;
        StartCoroutine(Co_Countdown(stepSeconds));
    }

    private IEnumerator Co_Countdown(float stepSeconds)
    {
        Phase = RacePhase.Countdown;
        GoShown = false;

        for (int t = 3; t >= 1; t--)
        {
            CurrentCountdown = t;
            OnCountdownTick?.Invoke(t);
            yield return new WaitForSeconds(stepSeconds);
        }

        Phase = RacePhase.Go;
        CurrentCountdown = 0;
        OnGo?.Invoke();
        GoShown = true;

        yield return new WaitForSeconds(stepSeconds * 0.6f);

        Phase = RacePhase.Racing;
    }

    public bool CanDrive() => Phase == RacePhase.Racing;
}
