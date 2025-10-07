using UnityEngine;

public class AIOpponentController : MonoBehaviour
{
    [Header("References")]
    private Transform _player;
    public Transform[] _waypoints;
    private int _currentWaypoint = 0;

    [Header("Driving Settings")]
    public float _speed = 20f;
    public float _turnSpeed = 5f;
    public float _waypointThreshold = 5f;

    [Header("Aggression Settings")]
    [Range(0f, 1f)] public float _aggression = 0.5f;
    public float _ramRange = 8f;
    public float _ramCooldown = 3f;
    private float _ramTimer = 0f;
    public float Speed { get => _speed; set => _speed = value; }

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void setWaypoints(Transform[] waypoints)
    {
        _waypoints = waypoints;
    }

    public void setPlayerTransform(Transform playerTransform)
    {
        _player = playerTransform;
    }
    void FixedUpdate()
    {
        if (GameManager.Instance != null && !GameManager.Instance.CanDrive())
        {
            if (_rb != null)
            {
                _rb.linearVelocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }
            return;
        }

        if (_waypoints.Length == 0) 
            return;

        Transform targetWaypoint = _waypoints[_currentWaypoint];
        Vector3 targetDir = (targetWaypoint.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(targetDir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _turnSpeed * Time.fixedDeltaTime);

        _rb.MovePosition(_rb.position + transform.forward * _speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < _waypointThreshold) {
            _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
        }

        if (_player != null) {
            Vector3 toPlayer = _player.position - transform.position;
            float distanceToPlayer = toPlayer.magnitude;

            _ramTimer -= Time.fixedDeltaTime;

            if (distanceToPlayer < _ramRange && _ramTimer <= 0f) {
                float decisionScore = _aggression * (_ramRange - distanceToPlayer);

                if (decisionScore > 2f) {
                    Vector3 ramDir = (_player.position - transform.position).normalized;
                    Quaternion ramRotation = Quaternion.LookRotation(ramDir, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, ramRotation, _turnSpeed * 2f * Time.fixedDeltaTime);

                    _rb.MovePosition(_rb.position + transform.forward * (_speed * 1.2f) * Time.fixedDeltaTime);

                    _ramTimer = _ramCooldown;
                }
            }
        }
    }
}