using UnityEngine;

public class PropellerController : MonoBehaviour
{
    [Header("Propeller References")]
    [Tooltip("Assign the Transform components of all four propeller objects (e.g., FL, RL, RR, FR props).")]
    [SerializeField] private Transform[] propellerReferences = new Transform[4]; // Array for 4 propellers

    [Header("Rotation Settings")]
    [Tooltip("Maximum rotation speed in degrees per second at maximum lift force (25.0).")]
    [SerializeField] private float maxRotationSpeed = 1200f;

    [Tooltip("Time in seconds to smoothly accelerate/decelerate propellers.")]
    [SerializeField] private float spinSmoothTime = 0.15f;

    [Tooltip("Local axis around which propellers rotate (e.g., Vector3.forward for Z-axis).")]
    [SerializeField] private Vector3 rotationAxis = Vector3.forward; // Default to Z-axis for most propeller models

    [Tooltip("If true, alternates rotation direction for adjacent propellers (e.g., FL/RR clockwise, FR/RL counterclockwise).")]
    [SerializeField] private bool alternateDirections = false;

    [Header("References")]
    [Tooltip("Reference to the Move script on DroneMain for liftForce control.")]
    [SerializeField] private Move moveScript;

    [Tooltip("Reference to the AudioManager for controlling propeller sound.")]
    [SerializeField] private AudioManager audioManager;

    private float[] currentRotationSpeeds = new float[4]; // Store speed for each propeller
    private float[] rotationVelocities = new float[4];    // Store velocity for smooth damping
    private int[] directionMultipliers = new int[4];      // 1 or -1 for rotation direction

    // Public getter for HUD (average speed of all propellers)
    public float CurrentRotationSpeed => CalculateAverageSpeed();

    void Start()
    {
        // Validate initial setup
        if (!ValidateSetup())
        {
            Debug.LogError("PropellerController: Initialization failed due to missing or invalid references. Disabling script.", this);
            enabled = false;
            return;
        }

        // Set up rotation directions
        SetupRotationDirections();
    }

    void Update()
    {
        if (moveScript == null || propellerReferences == null || audioManager == null)
            return;

        // Get current lift force from Move script
        float liftForce = moveScript.liftForce;

        // Determine if SPACE bar is pressed for lift
        bool isSpacePressed = Input.GetKey(KeyCode.Space);

        // Calculate normalized lift (0 to 1) based on the range 5.0 to 25.0
        float normalizedLift = Mathf.InverseLerp(5f, 25f, Mathf.Clamp(liftForce, 5f, 25f));
        float targetSpeed = isSpacePressed ? normalizedLift * maxRotationSpeed : 0f;

        // Update rotation and sound for all four propellers
        for (int i = 0; i < propellerReferences.Length; i++)
        {
            if (propellerReferences[i] != null)
            {
                // Smoothly interpolate to target speed
                currentRotationSpeeds[i] = Mathf.SmoothDamp(
                    currentRotationSpeeds[i],
                    targetSpeed,
                    ref rotationVelocities[i],
                    spinSmoothTime
                );

                // Rotate the propeller around the specified axis, applying direction multiplier
                float rotationAmount = currentRotationSpeeds[i] * Time.deltaTime * directionMultipliers[i];
                propellerReferences[i].Rotate(rotationAxis, rotationAmount, Space.Self);
            }
        }

        // Control propeller sound based on collective lift
        audioManager.ControlPropellerSound(isSpacePressed, normalizedLift);
    }

    /// <summary>
    /// Validates the initial setup of the script, checking for required references.
    /// </summary>
    /// <returns>True if setup is valid, false otherwise.</returns>
    private bool ValidateSetup()
    {
        bool isValid = true;

        if (moveScript == null)
        {
            Debug.LogError("PropellerController: Move script not assigned.", this);
            isValid = false;
        }

        if (audioManager == null)
        {
            Debug.LogError("PropellerController: AudioManager not assigned.", this);
            isValid = false;
        }

        if (propellerReferences == null || propellerReferences.Length != 4)
        {
            Debug.LogError("PropellerController: PropellerReferences array must be assigned and contain exactly 4 elements.", this);
            isValid = false;
        }
        else
        {
            for (int i = 0; i < propellerReferences.Length; i++)
            {
                if (propellerReferences[i] == null)
                {
                    Debug.LogError($"PropellerController: PropellerReferences[{i}] is not assigned.", this);
                    isValid = false;
                }
            }
        }

        if (rotationAxis == Vector3.zero)
        {
            Debug.LogError("PropellerController: Rotation Axis cannot be zero. Defaulting to Z-axis.", this);
            rotationAxis = Vector3.forward;
        }

        return isValid;
    }

    /// <summary>
    /// Sets up the rotation direction for each propeller based on alternateDirections setting.
    /// </summary>
    private void SetupRotationDirections()
    {
        if (alternateDirections)
        {
            // FL (0) and RR (2) rotate one way (clockwise)
            directionMultipliers[0] = 1; // FL
            directionMultipliers[2] = 1; // RR
            // RL (1) and FR (3) rotate the other way (counterclockwise)
            directionMultipliers[1] = -1; // RL
            directionMultipliers[3] = -1; // FR
        }
        else
        {
            // All propellers rotate in the same direction
            for (int i = 0; i < directionMultipliers.Length; i++)
            {
                directionMultipliers[i] = 1;
            }
        }
    }

    /// <summary>
    /// Calculates the average rotation speed across all propellers for HUD display.
    /// </summary>
    /// <returns>The average rotation speed in degrees per second.</returns>
    private float CalculateAverageSpeed()
    {
        if (propellerReferences == null || propellerReferences.Length == 0) return 0f;

        float totalSpeed = 0f;
        int validPropellers = 0;

        for (int i = 0; i < propellerReferences.Length; i++)
        {
            if (propellerReferences[i] != null)
            {
                totalSpeed += Mathf.Abs(currentRotationSpeeds[i]); // Use absolute value for average
                validPropellers++;
            }
        }

        return validPropellers > 0 ? totalSpeed / validPropellers : 0f;
    }
}