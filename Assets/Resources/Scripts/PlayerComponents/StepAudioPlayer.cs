using UnityEngine;

public class StepAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _stepClips;
    [SerializeField] private float _stepDistance = 1f;
    Vector3 _lastPosition;
    float _distanceTraveled = 0f;
    void Update()
    {
        Vector3 movement = transform.position - _lastPosition;
        movement.y = 0; // Ignore vertical movement
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
