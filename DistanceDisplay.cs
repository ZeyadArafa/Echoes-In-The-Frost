using UnityEngine;
using TMPro;

public class DistanceDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform droneTransform; // Drone's Transform (DroneMain)
    [SerializeField] private TextMeshProUGUI distanceTextUI; // UI TextMeshPro element
    [SerializeField] private CharacterSpawner characterSpawner; // Reference to CharacterSpawner

    [Header("Settings")]
    [SerializeField] private float displayRange = 800f; // Distance threshold (800 units)

    private GameObject[] characters;

    // Public property to expose the distance text
    public string DistanceText
    {
        get
        {
            return distanceTextUI != null ? distanceTextUI.text : "Error: Distance Text UI not assigned";
        }
    }

    void Awake()
    {
        // Validate references
        if (droneTransform == null)
        {
            Debug.LogError("DistanceDisplay: Drone Transform not assigned. Disabling script.", this);
            enabled = false;
            return;
        }
        if (distanceTextUI == null)
        {
            Debug.LogError("DistanceDisplay: Distance Text UI element not assigned. Disabling script.", this);
            enabled = false;
            return;
        }
        if (characterSpawner == null)
        {
            Debug.LogError("DistanceDisplay: CharacterSpawner not assigned. Disabling script.", this);
            enabled = false;
            return;
        }

        // Ensure UI is visible
        distanceTextUI.gameObject.SetActive(true);
    }

    void Start()
    {
        // Initialize with spawned characters
        RefreshCharacterList();
    }

    void Update()
    {
        if (!enabled || droneTransform == null || distanceTextUI == null || characterSpawner == null)
        {
            if (distanceTextUI != null)
            {
                distanceTextUI.text = "Error: Missing References";
            }
            return;
        }

        // Refresh character list to handle dynamic changes
        RefreshCharacterList();

        float nearestDistance = float.MaxValue;
        GameObject nearestStrandedPerson = null;

        // Find the nearest unsaved character
        foreach (GameObject character in characters)
        {
            if (character != null)
            {
                CharacterBehavior behavior = character.GetComponent<CharacterBehavior>();
                if (behavior != null && !behavior.IsSaved)
                {
                    float distance = Vector3.Distance(droneTransform.position, character.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestStrandedPerson = character;
                    }
                }
            }
        }

        // Update UI text
        if (nearestStrandedPerson != null)
        {
            if (nearestDistance <= displayRange)
            {
                distanceTextUI.text = $"Distance: {nearestDistance:F2} m";
            }
            else
            {
                distanceTextUI.text = "OUT OF RANGE";
            }
        }
        else
        {
            distanceTextUI.text = "No stranded persons found";
        }
    }

    private void RefreshCharacterList()
    {
        characters = characterSpawner.spawnedCharacters;
        if (characters == null || characters.Length == 0)
        {
            Debug.LogWarning("DistanceDisplay: No characters available from CharacterSpawner.", this);
        }
    }
}