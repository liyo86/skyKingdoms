using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMazeTerrain : MonoBehaviour
{
    public static TerrainGenerator instance;
    
    [Header("Configuration")]
    //Terrain
    public int width;
    public int height;
    private Terrain terrain;
    
    [Header("Terrain Material")]
    //Materials
    public Material terrainMaterial;

    // Limites
    public float minX = 0f;
    public float maxX;
    public float minZ = 0f;
    public float maxZ;
    public float minY = 0f;
    public float maxY;
    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
        void GenerateTerrain()
        { 
            terrain = gameObject.AddComponent<Terrain>();
            if (terrain.terrainData == null) {
                terrain.terrainData = new TerrainData();
            }
            
            int groundLayer = LayerMask.NameToLayer("Ground");
            if (groundLayer != -1)
            {
                terrain.gameObject.layer = groundLayer;
            }
    
            terrain.terrainData = GenerateTerrainData(terrain.terrainData);
            terrain.materialTemplate = terrainMaterial;
            
            // Terrain Collider
            TerrainCollider terrainCollider = gameObject.AddComponent<TerrainCollider>();
            terrainCollider.terrainData = terrain.terrainData;
        }
    
        TerrainData GenerateTerrainData(TerrainData terrainData)
        {
            terrainData.heightmapResolution = width + 1;
            terrainData.size = new Vector3(width, 600, height);
            terrainData.SetHeights(0, 0, GenerateHeights());
            return terrainData;
        }
    
        float[,] GenerateHeights()
        {
            float[,] heights = new float[width, height];
    
            // Genera plataformas aleatorias
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (Random.Range(0f, 1f) < 0.1f)
                    {
                        heights[i, j] = Random.Range(0, 1);
                    }
                }
            }
            
            return heights;
        }
}
