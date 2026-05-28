using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

[RequireComponent(typeof(AudioSource))]
public class SplineAudioTracker : MonoBehaviour
{
    [SerializeField] private SplineContainer _spline;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _updateInterval = 0.05f;

    private AudioSource _audioSource;
    private float _nextUpdateTime;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (_playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) _playerTransform = player.transform;
        }

        UpdateAudioPosition();
    }

    private void Update()
    {
        if (_spline == null || _playerTransform == null) return;

        if (Time.time >= _nextUpdateTime)
        {
            UpdateAudioPosition();
            _nextUpdateTime = Time.time + _updateInterval;
        }
    }

    private void UpdateAudioPosition()
    {
        float3 playerLocalPos = _spline.transform.InverseTransformPoint(_playerTransform.position);

        SplineUtility.GetNearestPoint(
            _spline.Spline, 
            playerLocalPos, 
            out float3 nearestLocalPoint, 
            out float t
        );

        Vector3 nearestWorldPoint = _spline.transform.TransformPoint(nearestLocalPoint);

        transform.position = nearestWorldPoint;
    }
}