using UnityEngine;
using UnityEngine.InputSystem;

public class PrometeoCarController : MonoBehaviour
{
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

    [Header("Luces del Vehículo")]
    public GameObject reverseLights;

    [Header("Sonido")]
    public AudioSource drivingSoundSource;

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

    private Rigidbody carRigidbody;
    private float carSpeed;
    private float steerInput;
    private float throttleInput;
    private bool isAccelerating;
    private bool isReversing;
    private GamepadRumble rumbleManager;

    void Start()
    {
        carRigidbody = gameObject.GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = bodyMassCenter;

        GameObject gameManager = GameObject.Find("GameManager");
        if (gameManager != null)
        {
            rumbleManager = gameManager.GetComponent<GamepadRumble>();
        }

        if (reverseLights != null)
        {
            reverseLights.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance != null && !GameManager.Instance.CanDrive())
        {
            throttleInput = 0f;
            ApplyMotorTorque(0);
            ApplyBrakes();
            return;
        }
        carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 60) / 1000;

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
        HandleReverseLights();
        HandleDrivingSound();
    }

    private void HandleDrivingSound()
    {
        if (drivingSoundSource == null) return;

        if (isAccelerating)
        {
            if (!drivingSoundSource.isPlaying)
            {
                drivingSoundSource.Play();
            }
        }
        else
        {
            drivingSoundSource.Stop();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        steerInput = context.ReadValue<Vector2>().x;
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        isAccelerating = context.ReadValueAsButton();
    }

    public void OnReverse(InputAction.CallbackContext context)
    {
        isReversing = context.ReadValueAsButton();
    }

    private void HandleReverseLights()
    {
        if (reverseLights != null)
        {
            reverseLights.SetActive(isReversing);
        }
    }

    private void HandleMotor()
    {
        float motorTorque = throttleInput * accelerationMultiplier * 50f;

        if (throttleInput == 0 && carRigidbody.linearVelocity.magnitude > 0.1f)
        {
            frontLeftCollider.brakeTorque = frontRightCollider.brakeTorque = rearLeftCollider.brakeTorque = rearRightCollider.brakeTorque = decelerationMultiplier * 50f;
            frontLeftCollider.motorTorque = frontRightCollider.motorTorque = rearLeftCollider.motorTorque = rearRightCollider.motorTorque = 0;
        }
        else
        {
            frontLeftCollider.brakeTorque = frontRightCollider.brakeTorque = rearLeftCollider.brakeTorque = rearRightCollider.brakeTorque = 0;

            if (carSpeed < maxSpeed && throttleInput > 0)
            {
                ApplyMotorTorque(motorTorque);
            }
            else if (Mathf.Abs(carSpeed) < maxReverseSpeed && throttleInput < 0)
            {
                if (carRigidbody.linearVelocity.z > 0.1f)
                {
                    ApplyBrakes();
                }
                else
                {
                    ApplyMotorTorque(motorTorque);
                }
            }
            else
            {
                ApplyMotorTorque(0);
            }
        }
    }

    private void HandleSteering()
    {
        float targetSteerAngle = steerInput * maxSteeringAngle;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pared")
        {
            Debug.Log("choque");

            float low = 0.8f;
            float high = 0.8f;
            float duration = 0.5f;

            rumbleManager.RumblePulse(low, high, duration);
        }
        if (rumbleManager != null && collision.relativeVelocity.magnitude > 10)
        {
            Debug.Log("choque");
            float low = 0.8f;
            float high = 0.8f;
            float duration = 0.5f;

            rumbleManager.RumblePulse(low, high, duration);
        }
    }
}