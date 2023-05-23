using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    #region VARTIABLES
    public static PathGenerator Instance;

    [Tooltip("Cantidad de aros en el camino.")]
    public int numberOfRings;

    [Tooltip("Separación entre los aros en el eje X.")]
    public float ringSpacingX;

    [Tooltip("Separación entre los aros en el eje Z.")]
    public float ringSpacingZ;

    [Tooltip("Número máximo de puntos por encima del punto original en el eje Y.")]
    public int maxPointsAbove;

    [Tooltip("Número máximo de puntos por debajo del punto original en el eje Y.")]
    public int maxPointsBelow;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    public List<Vector3> GeneratePath()
    {
        List<Vector3> ringPositions = new List<Vector3>();

        float xOffset = 0f;
        float zOffset = 0f;
        float yOffset = 0f;

        for (int i = 0; i < numberOfRings; i++)
        {
            Vector3 ringPosition = new Vector3(i * ringSpacingX + xOffset, yOffset, i * ringSpacingZ + zOffset);

            // Controlar la variación en el eje Y
            int pointsAbove = Random.Range(0, maxPointsAbove + 1);
            int pointsBelow = Random.Range(0, maxPointsBelow + 1);
            float newY = yOffset + (pointsAbove - pointsBelow);
            ringPosition.y = newY;

            ringPositions.Add(ringPosition);
        }

        if (ringPositions.Count > 0)
        {
            Vector3 lastRingPosition = ringPositions[ringPositions.Count - 1];
            Vector3 gemPosition = new Vector3(lastRingPosition.x, lastRingPosition.y + 1f, lastRingPosition.z + 100f);
            FlightLevel.Instance.SetGemPosition(gemPosition);
        }

        return ringPositions;
    }
}
