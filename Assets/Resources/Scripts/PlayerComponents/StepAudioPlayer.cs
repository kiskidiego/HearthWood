using UnityEngine;

[RequireComponent(typeof(GroundChecker))]
public class StepAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private float _stepDistance = 2.5f;
    GroundChecker _groundChecker;
    Vector3 _lastPosition;
    float _distanceTraveled = 0f;

    void Awake()
    {
        _groundChecker = GetComponent<GroundChecker>();
    }

    void Start()
    {
        _lastPosition = transform.position;
    }

    void Update()
    {
        if (!_groundChecker.IsGrounded()) return;

        Vector3 movement = transform.position - _lastPosition;
        _distanceTraveled += movement.magnitude;
        _lastPosition = transform.position;

        if (_distanceTraveled >= _stepDistance)
        {
            PlayStepSound();
            _distanceTraveled = 0f;
        }
    }
    public void PlayStepSound()
    {
        if (_stepClips.Length == 0) return;

        int randomIndex = Random.Range(0, _stepClips.Length);
        _audioSource.PlayOneShot(_stepClips[randomIndex]);
    }
}
