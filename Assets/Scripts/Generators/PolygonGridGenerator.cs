using UnityEngine;

public class PolygonGridGenerator : MonoBehaviour
{
    public GameObject polygonPrefab;
    public int gridWidth = 100;
    public int gridHeight = 100;
    public float spacing = 1f;
    public float rotation = 0f;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Calcular la posición del polígono
                Vector3 position = new Vector3(x * spacing, -3, z * spacing);

                // Instanciar un nuevo polígono en la posición calculada
                GameObject newPolygon = Instantiate(polygonPrefab, position, Quaternion.Euler(0, rotation, 0), transform);

                // Ajustar el nombre del polígono en la jerarquía para una mejor organización
                newPolygon.name = $"Polygon ({x}, {z})";
            }
        }
    }
}