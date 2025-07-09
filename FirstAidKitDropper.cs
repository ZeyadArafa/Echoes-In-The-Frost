using UnityEngine;

public class FirstAidKitDropper : MonoBehaviour
{
    public GameObject firstAidKitPrefab; // Assign the First Aid Kit prefab
    public Transform dropPoint; // Assign a child GameObject under DroneMain as the drop location
    public GameObject[] characters; // Assign Steve, Pete, and Kate
    public CharacterSpawner characterSpawner; // Assign in Inspector

    void Start()
    {
        if (characterSpawner != null)
        {
            characters = characterSpawner.spawnedCharacters;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Drop kit with Enter key
        {
            DropFirstAidKit();
        }
    }

    void DropFirstAidKit()
    {
        if (firstAidKitPrefab != null)
        {
            GameObject kit = Instantiate(firstAidKitPrefab, dropPoint.position, Quaternion.identity);
            Rigidbody kitRb = kit.GetComponent<Rigidbody>();
            if (kitRb != null)
            {
                // Inherit drone's velocity
                Rigidbody droneRb = GetComponent<Rigidbody>();
                if (droneRb != null)
                {
                    kitRb.linearVelocity = droneRb.linearVelocity;
                }
                else
                {
                    kitRb.linearVelocity = Vector3.zero; // Default to zero if no Rigidbody
                }

                // Find the closest character within pickupDistance
                CharacterBehavior closest = null;
                float minDist = float.MaxValue;
                foreach (GameObject character in characters)
                {
                    if (character != null)
                    {
                        CharacterBehavior behavior = character.GetComponent<CharacterBehavior>();
                        if (behavior != null && !behavior.IsSaved) // Add IsSaved property if needed
                        {
                            float distance = Vector3.Distance(character.transform.position, kit.transform.position);
                            if (distance < behavior.pickupDistance && distance < minDist)
                            {
                                minDist = distance;
                                closest = behavior;
                            }
                        }
                    }
                }
                if (closest != null)
                {
                    closest.DetectFirstAidKit(kit);
                }
            }
        }
    }
}