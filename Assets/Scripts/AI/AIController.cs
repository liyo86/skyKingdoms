using Generators;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public TerrainGenerator terrainGenerator; // Referencia al generador de terrenos
    public int plantCount = 5; // Número de plantas que el personaje debe eliminar
    public float moveSpeed = 5f; // Velocidad de movimiento del personaje
    public float jumpForce = 5f; // Fuerza de salto del personaje

    private GameObject[] plants; // Array de las plantas en el escenario
    private GameObject[] platforms; // Array de las plataformas en el escenario
    private GameObject gem; // Referencia a la gema

    private int plantsDestroyed = 0; // Contador de plantas eliminadas
    private int currentPlatformIndex = 0; // Índice de la plataforma actual
    private bool isJumping = false; // Indicador si el personaje está saltando

    private void Start()
    {
        // Obtener referencias a las plantas, plataformas y la gema del generador de terrenos
        plants = GameObject.FindGameObjectsWithTag("Plant");
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        gem =  GameObject.FindGameObjectWithTag("Gem");
    }

    private void Update()
    {
        if (plantsDestroyed < plantCount)
        {
            // Si aún quedan plantas por destruir, buscar la planta más cercana y dirigirse hacia ella
            GameObject closestPlant = GetClosestPlant();

            if (closestPlant != null)
            {
                // Mover hacia la planta más cercana
                Vector3 direction = closestPlant.transform.position - transform.position;
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);

                // Saltar si está cerca de la planta y no está saltando actualmente
                if (direction.magnitude < 1f && !isJumping)
                {
                    Jump();
                }
            }
        }
        else
        {
            // Si todas las plantas han sido destruidas, dirigirse hacia la gema
            if (gem != null)
            {
                // Mover hacia la gema
                Vector3 direction = gem.transform.position - transform.position;
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);

                // Saltar si está cerca de la gema y no está saltando actualmente
                if (direction.magnitude < 1f && !isJumping)
                {
                    Jump();
                }
            }
        }
    }

    private GameObject GetClosestPlant()
    {
        GameObject closestPlant = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject plant in plants)
        {
            float distance = Vector3.Distance(transform.position, plant.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlant = plant;
            }
        }

        return closestPlant;
    }

    private void Jump()
    {
        // Saltar hacia la siguiente plataforma
        if (currentPlatformIndex < platforms.Length)
        {
            Vector3 platformPosition = platforms[currentPlatformIndex].transform.position;
            Vector3 jumpDirection = platformPosition - transform.position;

            // Aplicar fuerza de salto
            GetComponent<Rigidbody>().AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);

            // Actualizar el índice de la plataforma actual
            currentPlatformIndex++;

            // Marcar como saltando
            isJumping = true;
            Invoke("ResetJump", 1f); // Reiniciar el indicador de salto después de 1 segundo
        }
    }

    private void ResetJump()
    {
        isJumping = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detectar colisión con las plantas
        if (other.CompareTag("Plant"))
        {
            Destroy(other.gameObject);
            plantsDestroyed++;
        }
    }
}
