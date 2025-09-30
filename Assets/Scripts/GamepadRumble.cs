using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GamepadRumble : MonoBehaviour
{
    private Coroutine stopRumbleCoroutine;
    public void RumblePulse(float lowFrequency, float highFrequency, float duration)
    {
        if (Gamepad.current == null)
        {
            return;
        }

        if (stopRumbleCoroutine != null)
        {
            StopCoroutine(stopRumbleCoroutine);
        }

        Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
        stopRumbleCoroutine = StartCoroutine(StopRumbleAfterDelay(duration));
    }
    private IEnumerator StopRumbleAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);

        StopRumble();
    }
    public void StopRumble()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }
    private void OnDisable()
    {
        StopRumble();
    }
}