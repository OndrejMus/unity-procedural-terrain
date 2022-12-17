using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // The size of the terrain
    public int terrainSize = 256;

    // The height of the terrain
    public int terrainHeight = 20;

    // The seed for the random number generator
    public int seed = 0;

    // The noise map for the terrain
    private float[,] noiseMap;

    // Use this for initialization
    void Start () {
        GenerateTerrain();
    }

    // Generates the terrain
    void GenerateTerrain() {
        // Create a new random number generator with the specified seed
        System.Random rand = new System.Random(seed);

        // Create a new noise map with the specified size
        noiseMap = new float[terrainSize, terrainSize];

        // Loop through each point in the noise map
        for (int x = 0; x < terrainSize; x++) {
            for (int y = 0; y < terrainSize; y++) {
                // Generate a random noise value
                noiseMap[x, y] = (float)rand.NextDouble() * terrainHeight;
            }
        }

        // Create the terrain object
        Terrain terrain = gameObject.AddComponent<Terrain>();

        // Set the terrain data
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = terrainSize;
        terrainData.size = new Vector3(terrainSize, terrainHeight, terrainSize);
        terrainData.SetHeights(0, 0, noiseMap);
        terrain.terrainData = terrainData;
    }
}
