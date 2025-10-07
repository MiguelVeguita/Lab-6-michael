using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour
{
    public GameObject panel;
    public static event Action ganaste;
    public static event Action perdiste;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0;
            ganaste?.Invoke();
        }
        if (other.CompareTag("com"))
        {
            Time.timeScale = 0;
            perdiste?.Invoke();

        }
    }
    public void RestartScene()
    {
        SceneManager.LoadScene("GameFinal");
    }

}
