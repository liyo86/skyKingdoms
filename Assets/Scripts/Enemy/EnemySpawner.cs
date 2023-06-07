using System.Collections.Generic;
using Generators;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    
    public float treeThreshold;

    [Range(1f, 10f)]
    public float minTreeDistance;
    
    public GameObject enemyPrefab;
    public LayerMask terrainLayer;

    private void Awake()
    {
        Instance = this;
    }

    public void CanStart ()
    {
        GenerateEnemies ();
    }
    
    // Genero una flor por cada arbol del terreno
    void GenerateEnemies()
{
    Dictionary<Vector3, bool> treePositions = new Dictionary<Vector3, bool>();
    for (int i = 0; i < TerrainGenerator.instance.maxZ; i++)
    {
        for (int j = 0; j < TerrainGenerator.instance.maxX; j++)
        {
            Vector3 worldPos = new Vector3(i, 0f, j);

            Collider[] hitColliders = Physics.OverlapSphere(worldPos, treeThreshold, terrainLayer);

            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag(nameof(Tree)) && !treePositions.ContainsKey(hit.transform.position))
                {
                    Collider[] platformColliders = Physics.OverlapSphere(hit.transform.position, minTreeDistance);
                    bool isOnPlatform = false;
                    foreach (Collider platformCollider in platformColliders)
                    {
                        if (platformCollider.CompareTag(Constants.PLATFORM_TAG))
                        {
                            isOnPlatform = true;
                            break;
                        }
                    }

                    if (isOnPlatform)
                    {
                        Debug.Log("hay una flor sobre una plataforma");
                        continue;
                    }

                    treePositions.Add(hit.transform.position, true);
                    float minX = Random.Range(1, minTreeDistance);
                    float minZ = Random.Range(1, minTreeDistance);

                    Vector3 enemyPos = new Vector3(hit.transform.position.x + minX, 0f, hit.transform.position.z + minZ);

                    // Comprobamos que la posición del enemigo esté dentro de los límites del terreno
                    if (enemyPos.x >= 0 && enemyPos.x < TerrainGenerator.instance.maxX &&
                        enemyPos.z >= 0 && enemyPos.z < TerrainGenerator.instance.maxZ)
                    {
                        GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity) as GameObject;
                        enemy.transform.rotation = Quaternion.Euler(0f, Random.Range(-90, 90), 0f);
                    }
                    else
                    {
                        enemyPos = new Vector3(hit.transform.position.x + 0.5f, 0f, hit.transform.position.z + 0.5f);
                        GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity) as GameObject;
                        enemy.transform.rotation = Quaternion.Euler(0f, Random.Range(-90, 90), 0f);
                    }

                    break;
                }
            }
        }
    }
}
}
