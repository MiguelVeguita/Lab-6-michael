using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    [Tooltip("La velocidad de rotación en grados por segundo.")]
    public float rotationSpeed = 50f;

    [Tooltip("El eje sobre el cual rotará el objeto. (0, 1, 0) para rotar como un trompo.")]
    public Vector3 rotationAxis = Vector3.up;

    public static event Action PowerUp;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUp?.Invoke();
            this.gameObject.SetActive(false);
        }
        
    }
}
