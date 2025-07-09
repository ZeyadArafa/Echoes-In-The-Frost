using UnityEngine;

public class TerrainTextureRandomizer : MonoBehaviour
{
    public Terrain terrain; // Terrain component, assigned in Inspector
    public TerrainLayer snowLayer; // Snow terrain layer, assigned in Inspector
    public TerrainLayer mountainLayer; // Mountain terrain layer, assigned in Inspector

    void Start()
    {
        // Ensure terrain is assigned
        if (terrain == null)
        {
            terrain = GetComponent<Terrain>(); // Try to get Terrain from this GameObject
            if (terrain == null)
            {
                terrain = Object.FindFirstObjectByType<Terrain>(); // Find Terrain in scene
                if (terrain == null)
                {
                    Debug.LogError("No Terrain found in the scene or assigned in Inspector!");
                    return;
                }
            }
        }

        // Verify terrain layers
        if (snowLayer == null || mountainLayer == null)
        {
            Debug.LogError("SnowLayer or MountainLayer not assigned in Inspector!");
            return;
        }

        // Apply random textures with blending at game start
        ApplyRandomTextures();
    }

    void ApplyRandomTextures()
    {
        TerrainData terrainData = terrain.terrainData;
        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;

        // Create a 3D alphamap array for 2 layers (snow at index 0, mountain at index 1)
        float[,,] alphamap = new float[width, height, 2];

        // Use Perlin noise for natural blending
        float noiseScale = 0.05f; // Adjust this value to control noise frequency
        float snowBias = 0.8f; // Bias toward 80% snow coverage

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Generate Perlin noise based on position
                float noiseValue = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
                // Bias the noise to ensure ~80% snow
                float blendedValue = Mathf.Lerp(0.1f, 0.9f, noiseValue) * snowBias;

                // Assign weights based on noise
                if (blendedValue >= 0.8f)
                {
                    alphamap[x, y, 0] = 1f; // Full snow
                    alphamap[x, y, 1] = 0f; // No mountain
                }
                else
                {
                    alphamap[x, y, 0] = Mathf.Clamp01(blendedValue); // Partial snow
                    alphamap[x, y, 1] = 1f - Mathf.Clamp01(blendedValue); // Partial mountain
                }
            }
        }

        // Apply the alphamap to the terrain
        terrainData.SetAlphamaps(0, 0, alphamap);
        terrain.Flush(); // Ensure terrain updates visually
    }
}