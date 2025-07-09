using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs; // Assign Steve, Pete, Kate prefabs
    public int characterCount = 3;
    private Terrain terrain;
    [HideInInspector] public GameObject[] spawnedCharacters; // Add this line

    void Start()
    {
        terrain = Object.FindAnyObjectByType<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("No Terrain found in the scene!");
            return;
        }
        if (characterPrefabs.Length < characterCount)
        {
            Debug.LogError("Not enough character prefabs assigned!");
            return;
        }
        spawnedCharacters = new GameObject[characterCount]; // Add this line
        SpawnCharacters();
    }

    void SpawnCharacters()
    {
        for (int i = 0; i < characterCount; i++)
        {
            Vector3 randomPosition = GetRandomSpawnPosition();
            GameObject character = Instantiate(characterPrefabs[i], randomPosition, Quaternion.identity);
            spawnedCharacters[i] = character; // Add this line
            CharacterBehavior behavior = character.GetComponent<CharacterBehavior>();
            if (behavior != null && behavior.animator != null)
            {
                behavior.animator.applyRootMotion = true;
                behavior.animator.SetBool("IsSaved", false);
                behavior.animator.SetBool("IsWaving", false);
                behavior.animator.SetBool("IsTurning", false);
                behavior.animator.SetBool("IsWalking", false);
                behavior.animator.SetBool("IsPickingUp", false);
            }
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        int maxAttempts = 100;
        for (int i = 0; i < maxAttempts; i++)
        {
            float x = Random.Range(0, terrain.terrainData.size.x);
            float z = Random.Range(0, terrain.terrainData.size.z);
            Vector3 position = new Vector3(x, 0, z);
            position.y = terrain.SampleHeight(position) + 0.1f; // Slight offset to avoid sinking
            if (GetSlopeAtPosition(position) < 10f)
            {
                return position;
            }
        }
        Debug.LogWarning("Could not find valid spawn position after " + maxAttempts + " attempts!");
        return Vector3.zero; // Fallback (should be rare)
    }

    float GetSlopeAtPosition(Vector3 position)
    {
        Vector3 normal = terrain.terrainData.GetInterpolatedNormal(
            position.x / terrain.terrainData.size.x,
            position.z / terrain.terrainData.size.z
        );
        return Vector3.Angle(normal, Vector3.up); // Slope in degrees
    }
}