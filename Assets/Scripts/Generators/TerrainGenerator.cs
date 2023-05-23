using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    #region VARIABLES
    public static TerrainGenerator instance;
    
    [Header("Configuration")]
    //Terrain
    public int width;
    public int height;
    private Terrain terrain;

    // Limites
    public float minX = 0f;
    public float maxX = 100f;
    public float minZ = 0f;
    public float maxZ = 100f;
    public float minY = 0f;
    public float maxY = 100f;
    public float limit = 10f; // limites para que al instanciar no coincida con los bordes
    
    [Header("Trees")]
    //Tree
    public int treeCount;
    public GameObject treePrefab;

        
    [Header("Grass")]
    //Grass
    public int grassCount;
    public GameObject grassPrefab;

            
    [Header("Mountains")]
    //Stone border
    public List<GameObject> stoneList;

    [Header("Plattforms")] public GameObject Plattform;
            
    [Header("Gems")]
    //Gems
    public GameObject gemPrefab;
    public int gemCount;
    
            
    [Header("River")]
    //River
    public float riverDepth = 2f;
    public GameObject water;
    public List<GameObject> waterGrassList = new List<GameObject>();

    [Header("Terrain Material")]
    //Materials
    public Material terrainMaterial;
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GenerateTerrain();
        GenerateMountains();
        GenerateRiver();
        GenerateTrees();
        GenerateGrass();
        GeneratePlatforms();
        
        EnemySpawner.instance.CanStart();
    }
    #endregion

    #region GENERATOR
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

    void GenerateTrees()
    {
        for (int i = 0; i < treeCount; i++)
        {
            float x = Random.Range(minX + limit, maxX - limit);
            float z = Random.Range(minZ + limit, maxZ - limit);
            float y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));

            Vector3 pos = new Vector3(x, y, z);

            float checkRadious = 5f;

            Collider[] hitColliders = Physics.OverlapSphere(pos, checkRadious);
            foreach (Collider hit in hitColliders)
            {
                if (!hit.CompareTag("Mountain") && 
                    !hit.CompareTag("Water") &&
                    !hit.CompareTag("Dragon"))
                {
                    Instantiate(treePrefab, pos, Quaternion.identity);
                }
            }
        }
    }
    
    void GenerateMountains()
    {
        int distanceBetween = 5;
        
        //Primero colocamos la valla arriba
        for (int i = 1; i < maxZ; i++)
        {
            GameObject fenceObj = new GameObject();
            fenceObj.transform.position = new Vector3(i, 0, maxZ);
            int index = Random.Range(0, 4);
            stoneList[index].transform.localScale = new Vector3(10, Random.Range(10, 20), 10);
            GameObject mountain = Instantiate(stoneList[index], fenceObj.transform) as GameObject;
            i += distanceBetween; 
        }
        
        //Abajo
        for (int i = 1; i < maxX; i++)
        {
            GameObject fenceObj = new GameObject();
            fenceObj.transform.position = new Vector3(i, 0, 0);
            int index = Random.Range(0, 4);
            stoneList[index].transform.localScale = new Vector3(10, Random.Range(10, 20), 10);
            GameObject mpuntain = Instantiate(stoneList[index], fenceObj.transform) as GameObject;
            i += distanceBetween; 
        }
        
        //Derecha
        for (int i = 0; i < maxZ; i++)
        {
            GameObject fenceObj = new GameObject();
            fenceObj.transform.position = new Vector3(maxX, 0, i);
            fenceObj.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            int index = Random.Range(0, 4);
            stoneList[index].transform.localScale = new Vector3(10, Random.Range(10, 20), 10);
            GameObject mountain = Instantiate(stoneList[index], fenceObj.transform) as GameObject;
            i += distanceBetween; 
        }
        
        //Izquierda
        for (int i = 0; i < maxZ; i++)
        {
            GameObject fenceObj = new GameObject();
            fenceObj.transform.position = new Vector3(0, 0, i);
            fenceObj.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            int index = Random.Range(0, 4);
            stoneList[index].transform.localScale = new Vector3(10, Random.Range(10, 20), 10);
            GameObject mountain = Instantiate(stoneList[index], fenceObj.transform) as GameObject;
            i += distanceBetween; 
        }

    }
    
    void GenerateRiver()
{
    float z = 0f;
    int segments = 20;
    float curveStrength = 0.5f;

    // Primera posición del río
    float x = Random.Range(minX + 2 , maxX - 2); // Para que no pise los bordes
    Vector3 currentPoint = new Vector3(x, 0f, z);
    Instantiate(water, currentPoint, Quaternion.identity);

    Vector3 previousPoint = currentPoint;
    Vector3 direction = Vector3.forward;
    int generatedSegments = 0; // Variable para contar los segmentos generados
    int currentGrass = waterGrassList.Count; 

    // Curva del rio
    while (generatedSegments < (maxZ / segments))
    {
        // Posición aleatoria a lo largo del eje z y x 
        float nextZ = Random.Range(minZ, maxZ);
        float nextx = Random.Range(minX, maxX);
        Vector3 nextPoint = new Vector3(nextx, 0f, nextZ);

        // Calcular el punto intermedio para curvar el río --> NO FUNCIONA
        Vector3 midPoint = Vector3.Lerp(currentPoint, nextPoint, 0.5f);
        float curveOffset = Random.Range(-curveStrength, curveStrength);
        Vector3 curveVector = Vector3.Cross(direction, Vector3.up) * curveOffset;
        midPoint += curveVector;

        // Crear los segmentos del río
        for (int i = 1; i <= segments; i++)
        {
            float t = (float)i / segments;
            Vector3 point = Vector3.Lerp(previousPoint, midPoint, t);
            point += (midPoint - currentPoint) * t;
            point += (nextPoint - midPoint) * t;
            point = new Vector3(point.x, point.y - 0.3f, point.z);

            Instantiate(water, point, Quaternion.identity);
      
            Instantiate(waterGrassList[Random.Range(0, currentGrass)], new Vector3(point.x - 5f, point.y + 0.3f, point.z), Quaternion.identity);
            Instantiate(waterGrassList[Random.Range(0, currentGrass)], new Vector3(point.x, point.y + 0.3f, point.z + 5f), Quaternion.identity);
        }

        // Actualizar las variables para el siguiente segmento del río
        previousPoint = midPoint - curveVector;
        currentPoint = nextPoint;
        direction = currentPoint - previousPoint;
        z = nextZ;
        generatedSegments++; // Incrementar el contador de segmentos generados
    }
}
    
    void GenerateGrass()
    {
        for (int i = 0; i < grassCount; i++)
        {
            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);
            float y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));

            Vector3 pos = new Vector3(x, y, z);
            float checkRadious = 1f;

            Collider[] hitColliders = Physics.OverlapSphere(pos, checkRadious);
            
            foreach (Collider hit in hitColliders)
            {
                if (!hit.CompareTag("Tree") && !hit.CompareTag("Mountain") && !hit.CompareTag("Water"))
                {
                    Instantiate(grassPrefab, new Vector3(x, y, z), Quaternion.identity);
                }
            }
        }
    }
    
    void GeneratePlatforms()
{
    int platformCount = 5;

    // Radio para comprobar si chocamos con algún otro objeto
    float platformRadius = 5f;

    // Altura actual del terreno
    float currentY = Terrain.activeTerrain.SampleHeight(new Vector3(0, 0, 0)) + 0.5f;

    float currentZ = Random.Range(minZ + limit, maxZ - limit);
    float currentX = Random.Range(minX + limit, maxX - limit);

    GameObject lastPlatform = null;

    for (int i = 0; i < platformCount; i++)
    {
        currentY += 0.5f;
        currentZ += 6f;

        float y = currentY;
        float z = currentZ;
        float x = currentX;

        // Compruebo colisiones
        bool objectFound = false;
        int maxAttempts = 5; // Número máximo de intentos antes de eliminar el objeto con el que choca
        int attempts = 0;

        do
        {
            attempts++;
            x = Random.Range(currentX - 2f, currentX + 2f);

            Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x, currentY, currentZ), platformRadius);

            objectFound = false;

            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("Mountain"))
                {
                    objectFound = true;
                    break; // Detener la generación de plataformas si choca contra una montaña
                }
                else if (hit.CompareTag("Tree"))
                {
                    objectFound = true;

                    if (attempts >= maxAttempts)
                    {
                        objectFound = false;
                        Destroy(hit.gameObject);
                    }
                }
            }
        } while (objectFound && attempts < maxAttempts);

        currentX = x;

        GameObject newPlatform = Instantiate(Plattform, new Vector3(x, y, z), Quaternion.identity) as GameObject;
        lastPlatform = newPlatform;
    }

    if (lastPlatform != null)
    {
        Vector3 gemPosition = new Vector3(lastPlatform.transform.position.x, lastPlatform.transform.position.y + 3f,
            lastPlatform.transform.position.z);
        Instantiate(gemPrefab, gemPosition, Quaternion.identity);
    }
}

    #endregion
}
