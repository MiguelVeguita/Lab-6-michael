using UnityEngine;
using TMPro;

public class FrameCountdownUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] float stepSeconds = 1f;

    bool running = false;
    float t = 0f;

    void Awake()
    {
        if (!label) label = GetComponent<TextMeshProUGUI>();
        if (label) label.text = "";
    }

    void OnEnable()
    {
        if (running && label) label.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!running || label == null) return;

        t += Time.deltaTime;

        if (t <= 1f * stepSeconds) label.text = "3";
        else if (t <= 2f * stepSeconds) label.text = "2";
        else if (t <= 3f * stepSeconds) label.text = "1";
        else if (t <= 4f * stepSeconds) label.text = "GO!";
        else StopCountdown();
    }

    public void StartCountdown(float step = 1f)
    {
        Debug.Log("Ga");

        stepSeconds = step;
        t = 0f;
        running = true;
        if (label) { label.gameObject.SetActive(true); label.text = "3"; }
    }

    public void StopCountdown()
    {
        running = false;
        if (label) { label.text = ""; label.gameObject.SetActive(false); }
    }
}
