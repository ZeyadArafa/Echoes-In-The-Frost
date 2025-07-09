using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource snowBlizzardSource;
    [SerializeField] private AudioSource propellerSoundSource;

    [Header("Propeller Sound Settings")]
    [SerializeField] private float minPitch = 0.8f; // Minimum pitch when not spinning
    [SerializeField] private float maxPitch = 1.2f; // Maximum pitch at max speed
    [SerializeField] private float minVolume = 0.0f; // Volume when not spinning
    [SerializeField] private float maxVolume = 1.0f; // Volume at max speed

    private void Start()
    {
        // Validate audio sources
        if (backgroundMusicSource == null || snowBlizzardSource == null || propellerSoundSource == null)
        {
            Debug.LogError("AudioManager: One or more AudioSource components are not assigned. Disabling script.", this);
            enabled = false;
            return;
        }

        // Start background music and blizzard sound
        if (!backgroundMusicSource.isPlaying) backgroundMusicSource.Play();
        if (!snowBlizzardSource.isPlaying) snowBlizzardSource.Play();
    }

    /// <summary>
    /// Controls the propeller sound based on whether the drone is lifting and its speed.
    /// </summary>
    /// <param name="isSpinning">True if the SPACE bar is pressed.</param>
    /// <param name="normalizedSpeed">Normalized speed (0 to 1) based on liftForce.</param>
    public void ControlPropellerSound(bool isSpinning, float normalizedSpeed)
    {
        if (propellerSoundSource == null) return;

        if (isSpinning)
        {
            if (!propellerSoundSource.isPlaying) propellerSoundSource.Play();

            // Adjust pitch and volume based on normalized speed
            float pitch = Mathf.Lerp(minPitch, maxPitch, normalizedSpeed);
            float volume = Mathf.Lerp(minVolume, maxVolume, normalizedSpeed);

            propellerSoundSource.pitch = pitch;
            propellerSoundSource.volume = volume;
        }
        else
        {
            if (propellerSoundSource.isPlaying) propellerSoundSource.Stop();
        }
    }
}