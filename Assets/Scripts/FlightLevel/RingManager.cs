using UnityEngine;
using System.Collections.Generic;

public class RingManager : MonoBehaviour
{
    private GameObject ringPrefab;

    private GameObject plusOne;

    private void Awake()
    {
        ringPrefab = GameObject.FindGameObjectWithTag("Ring");
        plusOne = GameObject.FindGameObjectWithTag("PlusOne");
    }

    void Start()
    {
        SpawnRings();
    }

    void SpawnRings()
    {
        List<Vector3> pathPoints = PathGenerator.Instance.GeneratePath();

        foreach (Vector3 point in pathPoints)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            GameObject ringNew = Instantiate(ringPrefab, point, rotation);
            ringNew.GetComponent<MeshRenderer>().enabled = true;
        }
        
        ringPrefab.SetActive(false);
    }
}