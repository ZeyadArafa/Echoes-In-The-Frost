using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class DroneHealth : MonoBehaviour
{
    public int maxHits = 3;
    public float damageVelocityThreshold = 3f;
    public float immunityDuration = 5f;
    private int currentHits = 0;
    private bool isImmune = false;
    private bool isCrashed = false;
    private Rigidbody rb;

    // Event for crash (subscribe ParticleEffectsManager to this)
    public event System.Action OnDroneCrashed;

    // Add a field for the UI Text element
    [SerializeField] private TextMeshProUGUI missionFailedText;

    // Public getters for HUD
    public int CurrentHits => currentHits;
    public int MaxHits => maxHits;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHits = 0;
        isImmune = false;
        isCrashed = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isCrashed || isImmune) return;
        if (rb == null) return;
        float relVel = collision.relativeVelocity.magnitude;
        if (relVel > damageVelocityThreshold)
        {
            currentHits++;
            Debug.Log($"Drone hit! Total hits: {currentHits}");
            if (currentHits >= maxHits)
            {
                Crash();
            }
            else
            {
                StartCoroutine(ImmunityCoroutine());
            }
        }
    }

    private IEnumerator ImmunityCoroutine()
    {
        isImmune = true;
        Debug.Log("Drone is immune to damage for 5 seconds.");
        yield return new WaitForSeconds(immunityDuration);
        isImmune = false;
        Debug.Log("Drone is no longer immune.");
    }

    private void Crash()
    {
        isCrashed = true;
        Debug.Log("Mission Failed !");
        // Trigger fire effect via event
        if (OnDroneCrashed != null)
            OnDroneCrashed.Invoke();
        // Show Mission Failed message
        StartCoroutine(MissionFailedRoutine());
    }

    private IEnumerator MissionFailedRoutine()
    {
        // Display message on UI
        if (missionFailedText != null)
        {
            missionFailedText.text = "Mission Failed !";
            missionFailedText.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(10f);
        // Restart scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}