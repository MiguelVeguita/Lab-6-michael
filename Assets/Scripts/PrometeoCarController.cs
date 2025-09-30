using UnityEngine;
using UnityEngine.InputSystem;

public class PrometeoCarController : MonoBehaviour
{
    // CAR SETUP
    [Header("Configuración del Vehículo")]
    [Range(20, 250)]
    public int maxSpeed = 120;
    [Range(10, 120)]
    public int maxReverseSpeed = 50;
    [Range(1, 10)]
    public int accelerationMultiplier = 3;
    [Range(10, 45)]
    public int maxSteeringAngle = 27;
    [Range(0.1f, 1f)]
    public float steeringSpeed = 0.6f;
    [Range(100, 600)]
    public int brakeForce = 400;
    [Range(1, 10)]
    public int decelerationMultiplier = 2;
    public Vector3 bodyMassCenter;

    // WHEELS
    [Header("Ruedas (Colliders y Meshes)")]
    public GameObject frontLeftMesh;
    public WheelCollider frontLeftCollider;
    [Space(10)]
    public GameObject frontRightMesh;
    public WheelCollider frontRightCollider;
    [Space(10)]
    public GameObject rearLeftMesh;
    public WheelCollider rearLeftCollider;
    [Space(10)]
    public GameObject rearRightMesh;
    public WheelCollider rearRightCollider;

    // Private Variables
    private Rigidbody carRigidbody;
    private float carSpeed;
    private float steerInput;
    private float throttleInput; // 1 para acelerar, -1 para reversa, 0 para nada
    private bool isAccelerating;
    private bool isReversing;

    void Start()
    {
        carRigidbody = gameObject.GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = bodyMassCenter;
    }

    void FixedUpdate()
    {
        // Calcula la velocidad actual del auto
        carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 60) / 1000;

        // Determina la aceleración final basada en los inputs
        if (isAccelerating)
        {
            throttleInput = 1f;
        }
        else if (isReversing)
        {
            throttleInput = -1f;
        }
        else
        {
            throttleInput = 0f;
        }

        HandleMotor();
        HandleSteering();
    }

    void Update()
    {
        AnimateWheelMeshes();
    }

    // --- MÉTODOS PÚBLICOS PARA EL INPUT SYSTEM ---

    public void OnMove(InputAction.CallbackContext context)
    {
        // Lee el valor X del joystick para la dirección
        steerInput = context.ReadValue<Vector2>().x;
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        // context.ReadValueAsButton() devuelve true si el botón está presionado
        isAccelerating = context.ReadValueAsButton();
    }

    public void OnReverse(InputAction.CallbackContext context)
    {
        isReversing = context.ReadValueAsButton();
    }

    // --- LÓGICA DEL VEHÍCULO ---

    private void HandleMotor()
    {
        float motorTorque = throttleInput * accelerationMultiplier * 50f;

        // Si no se está acelerando ni frenando, se aplica una deceleración gradual
        if (throttleInput == 0 && carRigidbody.linearVelocity.magnitude > 0.1f)
        {
            frontLeftCollider.brakeTorque = frontRightCollider.brakeTorque = rearLeftCollider.brakeTorque = rearRightCollider.brakeTorque = decelerationMultiplier * 50f;
            frontLeftCollider.motorTorque = frontRightCollider.motorTorque = rearLeftCollider.motorTorque = rearRightCollider.motorTorque = 0;
        }
        else
        {
            // Quita la fuerza de freno para poder mover el auto
            frontLeftCollider.brakeTorque = frontRightCollider.brakeTorque = rearLeftCollider.brakeTorque = rearRightCollider.brakeTorque = 0;

            // Acelerar
            if (carSpeed < maxSpeed && throttleInput > 0)
            {
                ApplyMotorTorque(motorTorque);
            }
            // Reversa
            else if (Mathf.Abs(carSpeed) < maxReverseSpeed && throttleInput < 0)
            {
                // Si el auto se mueve hacia adelante, primero frena
                if (carRigidbody.linearVelocity.z > 0.1f)
                {
                    ApplyBrakes();
                }
                else
                {
                    ApplyMotorTorque(motorTorque);
                }
            }
            // Si se alcanza la velocidad máxima, se deja de aplicar fuerza
            else
            {
                ApplyMotorTorque(0);
            }
        }
    }

    private void HandleSteering()
    {
        float targetSteerAngle = steerInput * maxSteeringAngle;
        // Interpola suavemente hacia el ángulo de giro deseado
        frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, targetSteerAngle, steeringSpeed);
        frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, targetSteerAngle, steeringSpeed);
    }

    private void ApplyMotorTorque(float torque)
    {
        frontLeftCollider.motorTorque = torque;
        frontRightCollider.motorTorque = torque;
        rearLeftCollider.motorTorque = torque;
        rearRightCollider.motorTorque = torque;
    }

    private void ApplyBrakes()
    {
        frontLeftCollider.brakeTorque = brakeForce;
        frontRightCollider.brakeTorque = brakeForce;
        rearLeftCollider.brakeTorque = brakeForce;
        rearRightCollider.brakeTorque = brakeForce;
    }

    void AnimateWheelMeshes()
    {
        AnimateSingleWheel(frontLeftCollider, frontLeftMesh.transform);
        AnimateSingleWheel(frontRightCollider, frontRightMesh.transform);
        AnimateSingleWheel(rearLeftCollider, rearLeftMesh.transform);
        AnimateSingleWheel(rearRightCollider, rearRightMesh.transform);
    }

    void AnimateSingleWheel(WheelCollider collider, Transform meshTransform)
    {
        collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        meshTransform.position = pos;
        meshTransform.rotation = rot;
    }
}