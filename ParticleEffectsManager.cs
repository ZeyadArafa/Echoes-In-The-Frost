using UnityEngine;

public class ParticleEffectsManager : MonoBehaviour
{
    [Header("Particle System References")]
    [SerializeField] private ParticleSystem snowEffect; // Assign in Inspector
    [SerializeField] private ParticleSystem fogEffect;  // Assign in Inspector
    [SerializeField] private ParticleSystem fireEffectPrefab; // Assign fire prefab in Inspector

    [Header("References")]
    [SerializeField] private GameObject drone; // Assign drone in Inspector
    [SerializeField] private DroneHealth droneHealth; // Assign DroneHealth in Inspector

    private ParticleSystem fireEffectInstance;
    private Rigidbody droneRb;
    private bool isDroneFlying = true;
    private bool hasCrashed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Validate references
        if (!drone || !droneHealth || !snowEffect || !fogEffect || !fireEffectPrefab)
        {
            Debug.LogError("ParticleEffectsManager: Missing required references or prefabs.");
            enabled = false;
            return;
        }

        droneRb = drone.GetComponent<Rigidbody>();
        if (droneRb == null)
        {
            Debug.LogError("ParticleEffectsManager: No Rigidbody found on drone.");
            enabled = false;
            return;
        }

        // Instantiate fire effect but keep it inactive
        fireEffectInstance = Instantiate(fireEffectPrefab, drone.transform.position, Quaternion.identity, drone.transform);
        fireEffectInstance.gameObject.SetActive(false);

        // Start snow and fog
        snowEffect.Play();
        fogEffect.Play();

        // Subscribe to drone crash event
        droneHealth.OnDroneCrashed += OnDroneCrashed;
    }

    void OnDestroy()
    {
        if (droneHealth != null)
            droneHealth.OnDroneCrashed -= OnDroneCrashed;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCrashed || droneRb == null) return;

        // Check if drone is flying (velocity or altitude above ground)
        bool wasFlying = isDroneFlying;
        isDroneFlying = droneRb.linearVelocity.magnitude > 0.1f || drone.transform.position.y > 10f;

        // Keep snow/fog centered on drone (for local effect)
        snowEffect.transform.position = drone.transform.position;
        fogEffect.transform.position = drone.transform.position;

        if (isDroneFlying != wasFlying)
        {
            if (isDroneFlying)
            {
                if (!snowEffect.isPlaying) snowEffect.Play();
                if (!fogEffect.isPlaying) fogEffect.Play();
            }
            else
            {
                snowEffect.Pause();
                fogEffect.Pause();
            }
        }
    }

    private void OnDroneCrashed()
    {
        hasCrashed = true;
        // Stop snow and fog
        if (snowEffect != null) snowEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        if (fogEffect != null) fogEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        // Activate and play fire effect at drone's position
        if (fireEffectInstance != null)
        {
            fireEffectInstance.transform.position = drone.transform.position;
            fireEffectInstance.gameObject.SetActive(true);
            fireEffectInstance.Play();
        }
    }
}
