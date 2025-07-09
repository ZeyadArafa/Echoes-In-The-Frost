using UnityEngine;

public class FirstAidKit : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasCollided;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("FirstAidKit missing Rigidbody component!");
        }
        hasCollided = false; // Reset for each new kit
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided && rb != null)
        {
            hasCollided = true;
            float impactVelocity = rb.linearVelocity.magnitude;
            Debug.Log("Impact Velocity: " + impactVelocity); // Debug output
            if (impactVelocity > 8f)
            {
                Destroy(gameObject);
            }
        }
    }
}