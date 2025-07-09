using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    public Animator animator;
    public GameObject drone;
    public float detectionDistance = 50f;
    public float pickupDistance = 10f;
    private bool isWaving;
    private bool isSaved;
    private GameObject targetKit;
    private bool isTurning;
    private bool isWalking;
    private bool isPickingUp;

    // Public property to expose isSaved
    public bool IsSaved
    {
        get { return isSaved; }
    }

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not assigned on " + gameObject.name);
        }
        // Try to find the drone if not assigned
        if (drone == null)
        {
            drone = GameObject.FindWithTag("Drone");
            if (drone == null)
            {
                Debug.LogError("[CharacterBehavior] Drone reference not assigned and could not be found in the scene! Please assign the drone GameObject or set its tag to 'Drone'.");
            }
        }
        animator.SetBool("IsSaved", false);
    }

    void Update()
    {
        if (isSaved) return;
        if (drone == null) return; // Prevent errors if drone is still not found

        float distanceToDrone = Vector3.Distance(transform.position, drone.transform.position);
        Debug.Log("Distance to Drone: " + distanceToDrone + ", IsWaving: " + isWaving);

        // Drone proximity detection
        if (distanceToDrone <= detectionDistance && !isWaving && targetKit == null)
        {
            isWaving = true;
            animator.SetBool("IsWaving", true);
            Debug.Log("Starting Waving");
        }
        else if (distanceToDrone > detectionDistance && isWaving)
        {
            isWaving = false;
            animator.SetBool("IsWaving", false);
            Debug.Log("Stopping Waving");
        }

        // First Aid Kit interaction
        if (targetKit != null)
        {
            float distanceToKit = Vector3.Distance(transform.position, targetKit.transform.position);
            Debug.Log("Distance to Kit: " + distanceToKit + ", IsTurning: " + isTurning + ", IsWalking: " + isWalking);

            if (distanceToKit <= pickupDistance)
            {
                Debug.Log("Within pickup distance");
                if (!isTurning && !isWalking && !isPickingUp)
                {
                    isTurning = true;
                    animator.SetBool("IsTurning", true);
                    Debug.Log("Starting Turning");
                    transform.rotation = Quaternion.LookRotation(Vector3.right); // Always turn right
                }
                else if (isTurning && animator.GetCurrentAnimatorStateInfo(0).IsName("Injured Turn Right") &&
                         animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    isTurning = false;
                    isWalking = true;
                    animator.SetBool("IsTurning", false);
                    animator.SetBool("IsWalking", true);
                    Debug.Log("Starting Walking");
                }
                else if (isWalking && distanceToKit < 1f)
                {
                    isWalking = false;
                    isPickingUp = true;
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsPickingUp", true);
                    Debug.Log("Starting Pickup");
                }
                else if (isPickingUp && animator.GetCurrentAnimatorStateInfo(0).IsName("Pickup") &&
                         animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
                {
                    isPickingUp = false;
                    isSaved = true;
                    Destroy(targetKit);
                    animator.SetBool("IsPickingUp", false);
                    animator.SetBool("IsSaved", true);
                    Debug.Log("Saved, Returning to Sit");
                }
            }
            else
            {
                ResetToInitialState();
            }
        }
    }

    public void DetectFirstAidKit(GameObject kit)
    {
        Debug.Log($"{gameObject.name} DetectFirstAidKit called with kit {kit.name}");
        if (!isSaved && targetKit == null)
        {
            targetKit = kit;
            if (isWaving)
            {
                isWaving = false;
                animator.SetBool("IsWaving", false);
                Debug.Log("Stopping Waving for Kit");
            }
        }
    }

    private void ResetToInitialState()
    {
        isTurning = false;
        isWalking = false;
        isPickingUp = false;
        targetKit = null;
        animator.SetBool("IsWaving", false);
        animator.SetBool("IsTurning", false);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsPickingUp", false);
        Debug.Log("Reset to Initial State");
    }
}