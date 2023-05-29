using System.Collections;
using Managers;
using UnityEngine;

namespace AI.Player_Controller
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class AIController : MonoBehaviour, IAttackCooldown
    {
        public bool IsDefenseCooldown => isDefenseCooldown;
        public float DefenseCooldown => defenseCooldown;
        public bool IsShootCooldown => isShootCooldown;
        public float ShootCooldown => shootCooldown;
        public bool IsSpecialAttackCooldown => isSpecialAttackCooldown;
        public float SpecialAttackCooldown => specialAttackCooldown;
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f; // Velocidad de movimiento del personaje
        [SerializeField] private float jumpForce = 5f; // Fuerza de salto del personaje

        [Header("Spell Settings")]
        [SerializeField] private GameObject spellPrefab;
        [SerializeField] private Transform spellSpawn;

        [Header("Cooldown Settings")]
        [SerializeField] private float defenseCooldown; // Tiempo de reutilización de la defensa
        [SerializeField] private float specialAttackCooldown; // Tiempo de reutilización del ataque especial
        [SerializeField] private float shootCooldown; // Tiempo de reutilización del disparo

        private GameObject[] plants; // Array de las plantas en el escenario
        private GameObject[] platforms; // Array de las plataformas en el escenario
        private GameObject gem; // Referencia a la gema
        private Animator animator;
        private Rigidbody rb;

        private int plantsDestroyed = 0; // Contador de plantas eliminadas
        private int currentPlatformIndex = 0; // Índice de la plataforma actual
        private bool isJumping = false; // Indicador si el personaje está saltando
        private bool isGrounded = true;
        private bool isShooting = false;

        private bool isDefenseCooldown = false;
        private bool isSpecialAttackCooldown = false;
        private bool isShootCooldown = false;

        private bool canAttack = true;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            StartCoroutine(CloseDialogue());
        }

        private IEnumerator CanStart()
        {
            yield return new WaitForSeconds(0.5f);

            // Obtener referencias a las plantas, plataformas y la gema
            plants = GameObject.FindGameObjectsWithTag(Constants.ENEMY_TAG);
            platforms = GameObject.FindGameObjectsWithTag(Constants.PLATFORM_TAG);
        }

        private void Update()
        {
            if (plants == null || plants.Length == 0)
                return;

            if (isShooting) return;

            if (MyLevelManager.Instance.enemyCount < 5)
            {
                // Si aún quedan plantas por destruir, buscar la planta más cercana y dirigirse hacia ella
                GameObject closestPlant = GetClosestPlant();

                if (closestPlant != null)
                {
                    // Mover hacia la planta más cercana
                    Vector3 direction = (closestPlant.transform.position - transform.position).normalized;
                    rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
                    transform.LookAt(closestPlant.transform);

                    // Disparar si tengo la planta cerca
                    if (Vector3.Distance(transform.position, closestPlant.transform.position) <= 5f)
                    {
                        Attack();
                    }
                }
            }
            else if (MyLevelManager.Instance.enemyCount >= 5)
            {
                gem = GameObject.FindGameObjectWithTag(Constants.GEM_TAG);
                if (gem == null) return; // Si entro quiere decir que ya se ha instanciado la gema

                // Mover hacia la primera plataforma
                GameObject closestPlatform = GetClosestPlatform();

                if (closestPlatform != null)
                {
                    // Mover hacia la plataforma más cercana
                    Vector3 direction = (closestPlatform.transform.position - transform.position).normalized;
                    rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
                    transform.LookAt(closestPlatform.transform);

                    // Saltar si tengo la planta cerca
                    if (Vector3.Distance(transform.position, closestPlatform.transform.position) <= 4f)
                    {
                        if (isJumping) return;
                        Jump();
                        Destroy(closestPlatform);
                        currentPlatformIndex++; // Incrementar el índice de la plataforma actual
                    }
                }
                else
                {
                    // No hay más plataformas, mover hacia la gema
                    if (gem != null)
                    {
                        Vector3 direction = (gem.transform.position - transform.position).normalized;
                        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
                        transform.LookAt(gem.transform);
                    }
                }
            }
        }

        private void LateUpdate()
        {
            animator.SetFloat("speed", rb.velocity.magnitude);
            animator.SetBool("walk", rb.velocity.magnitude > 0f && !isShooting);
            animator.SetBool("jump", isJumping);
            animator.SetBool("land", isGrounded);
        }

        private IEnumerator CloseDialogue()
        {
            yield return new WaitForSeconds(3f);

            MyDialogueManager.Instance.HideDialogBox();

            StartCoroutine(CanStart());
        }

        private GameObject GetClosestPlant()
        {
            GameObject closestPlant = null;
            float closestDistance = float.MaxValue;

            foreach (GameObject plant in plants)
            {
                if (plant != null) // Verificar si la planta aún existe
                {
                    float distance = Vector3.Distance(transform.position, plant.transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlant = plant;
                    }
                }
            }

            return closestPlant;
        }

        private GameObject GetClosestPlatform()
        {
            if (currentPlatformIndex >= platforms.Length)
                return null;

            return platforms[currentPlatformIndex];
        }

        private void Jump()
        {
            isJumping = true;
            // Aplicar fuerza de salto
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            isGrounded = false;
            Invoke("ResetJump", 1f); // Reiniciar el indicador de salto después de 1 segundo
        }

        private void ResetJump()
        {
            isJumping = false;
            isGrounded = true;
        }

        private void Attack()
        {
            if (isShootCooldown) return;
            isShootCooldown = true;
            isShooting = true;

            animator.SetTrigger("shoot");
            StartCoroutine(Shoot());

            StartCoroutine(ShootCooldownTimer());
        }

        private IEnumerator ShootCooldownTimer()
        {
            yield return new WaitForSeconds(shootCooldown);
            isShootCooldown = false;
        }

        private IEnumerator Shoot()
        {
            yield return new WaitForSeconds(0.5f);

            MyAudioManager.Instance.PlaySfx("fireVoice");

            Instantiate(spellPrefab, spellSpawn.position, spellSpawn.rotation);

            isShooting = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Detectar colisión con las plantas
            if (other.CompareTag(Constants.ENEMY_TAG))
            {
                PlayerHealth.Instance.AddDamage(10);
            }
        }
    }
}
