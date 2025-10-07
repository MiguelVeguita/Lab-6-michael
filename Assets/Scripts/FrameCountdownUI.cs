using UnityEngine;
using TMPro;
using System;

public class FrameCountdownUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] float stepSeconds = 1f;
    [Header("Luces de Salida")]
    public Renderer[] countdownLights; // Aquí arrastrarás tus luces
    public Color redColor = Color.red;
    public Color yellowColor = Color.yellow;
    public Color greenColor = Color.green;
    public Color offColor = Color.black;
    bool running = false;
    float t = 0f;
    public static event Action GoRacing;

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

        if (t <= 1f * stepSeconds)
        {
            label.text = "3";
            SetLightsColor(redColor); // <--- AÑADE ESTO
        }
        else if (t <= 2f * stepSeconds)
        {
            label.text = "2";
            SetLightsColor(yellowColor); // <--- AÑADE ESTO
        }
        else if (t <= 3f * stepSeconds)
        {
            label.text = "1";
            SetLightsColor(yellowColor); // <--- AÑADE ESTO (amarillo de nuevo, como en semáforos reales)
        }
        else if (t <= 4f * stepSeconds)
        {
            label.text = "GO!";
            SetLightsColor(greenColor); // <--- AÑADE ESTO
        }
        else
        {
            StopCountdown();
        }
    }

    public void StartCountdown(float step = 1f)
    {
        Debug.Log("Ga");

        stepSeconds = step;
        t = 0f;
        running = true;

        if (label) { label.gameObject.SetActive(true); label.text = "3"; }
    }
    void SetLightsColor(Color color, bool shouldEmit = true)
    {
        if (countdownLights == null) return;

        foreach (Renderer lightRenderer in countdownLights)
        {
            lightRenderer.material.color = color;
            if (shouldEmit)
            {
                lightRenderer.material.EnableKeyword("_EMISSION");
                lightRenderer.material.SetColor("_EmissionColor", color);
            }
            else
            {
                lightRenderer.material.DisableKeyword("_EMISSION");
            }
        }
    }
    public void StopCountdown()
    {
        running = false;
        if (label) { label.text = ""; label.gameObject.SetActive(false); }
        SetLightsColor(offColor, false);
        GoRacing?.Invoke();

    }
}
