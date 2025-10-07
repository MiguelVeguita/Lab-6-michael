using UnityEngine;


public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    
    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Se encontró más de una instancia de " + typeof(T) + ". Destruyendo la nueva.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this as T;
        }
    }
}