using System.Collections;
using Managers;
using UnityEngine;

namespace AI.Player_Controller
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class AIController : MonoBehaviour, IAttackCooldown
    {
        #region VARIABLES
        public bool IsDefenseCooldown => isDefenseCooldown;
        public float DefenseCooldown => defenseCooldown;
        public bool IsShootCooldown => isShootCooldown;
        public float ShootCooldown => shootCooldown;
        public bool IsSpecialAttackCooldown => isSpecialAttackCooldown;
        public float SpecialAttackCooldown => specialAttackCooldown;
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f; // Velocidad de movimiento del personaje
        [SerializeField] private float jumpForce = 5f; // Fuerza de salto del personaje
        [SerializeField] private float jumpForwardForce = 5f; // Fuerza de salto del personaje

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
        
        private int currentPlatformIndex = 0; // Índice de la plataforma actual
        private bool isJumping = false; // Indicador si el personaje está saltando
        private bool hasJumpedToGem;
        private bool isGrounded = true;

        private bool isDefenseCooldown = false;
        private bool isSpecialAttackCooldown = false;
        private bool isShootCooldown = false;

        private bool isAttacking = false;
        private bool canAttack;
        private bool canMove;
        private bool isChangingDirection;
        private float randomAngle;
        private Vector3 direction;
        private GameObject actualPlant;
        
        private float noMovementTime = 0f;
        private const float noMovementThreshold = 0.1f;
        private const float evadeObstacleTimeThreshold = 1f;
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            StartCoroutine(CloseDialogue());
        }

        private void Update()
        {
            if (isAttacking) canMove = false;
            
            if (plants == null || plants.Length == 0 || isAttacking || isChangingDirection)
                return;
            
            CheckNoMovementWhileWalking();
            
            CheckStates();
        }
        
        private void FixedUpdate()
        {
            if (plants == null || plants.Length == 0)
                return;
            
            if (canMove)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

                rb.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }

        private void LateUpdate()
        {
            animator.SetFloat("speed", rb.velocity != Vector3.zero ? 1f : 0f);
            animator.SetBool("walk", canMove);
            animator.SetBool("jump", isJumping);
            animator.SetBool("land", isGrounded);
        }
        #endregion
        
        #region IA CONFIGURATION
        private IEnumerator CanStart()
        {
            yield return new WaitForSeconds(1f);

            plants = GameObject.FindGameObjectsWithTag(Constants.ENEMY_TAG);
            platforms = GameObject.FindGameObjectsWithTag(Constants.PLATFORM_TAG);
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
                if (plant != null)
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
        
        private void Move()
        {
            canMove = true;
            canAttack = true;
        }

        private void CheckObstacle()
        {
            RaycastHit hit;
            
            if (Physics.SphereCast(transform.position, 0.5f, direction, out hit, 3f))
            {
                if (MyLevelManager.Instance.enemyCount < 5)
                {
                    if (hit.collider.CompareTag(Constants.TREE_TAG) ||
                        hit.collider.CompareTag(Constants.PLATFORM_TAG))
                    {
                        StartCoroutine(EvadeObstacle(hit.point));
                    }    
                } else if (MyLevelManager.Instance.enemyCount >= 5)
                {
                    if (hit.collider.CompareTag(Constants.TREE_TAG) ||
                        hit.collider.CompareTag(Constants.ENEMY_TAG))
                    {
                        StartCoroutine(EvadeObstacle(hit.point));
                    } 
                }
            }
        }
        
        private void CheckNoMovementWhileWalking()
        {
            if (canMove)
            {
                if (rb.velocity.magnitude < noMovementThreshold)
                {
                    noMovementTime += Time.deltaTime;
                }
                else
                {
                    noMovementTime = 0f;
                }
            }

            if (noMovementTime > evadeObstacleTimeThreshold)
            {
                CheckObstacle();
            }
        }
        
        private IEnumerator EvadeObstacle(Vector3 obstaclePosition)
        {
            isChangingDirection = true;

            Vector3 obstacleDirection = (obstaclePosition - transform.position).normalized;
            Vector3 rightDirection = Vector3.Cross(Vector3.up, obstacleDirection).normalized;
            
            Vector3 evadePoint = obstaclePosition + rightDirection * 2f;
            
            direction = (evadePoint - transform.position).normalized;
            
            yield return new WaitForSeconds(2f);

            isChangingDirection = false;
        }
        
        #endregion
        
        #region IA STATES

        private void CheckStates()
        {
            if (MyLevelManager.Instance.enemyCount < 5)
            {
                DefeatFlowerState();
            }
            else if (MyLevelManager.Instance.enemyCount >= 5)
            {
                GetGemState();
            }
        }

        private void DefeatFlowerState()
        {
            actualPlant = GetClosestPlant();

            if (actualPlant != null)
            {
                // Mover hacia la planta más cercana
                direction = (actualPlant.transform.position - transform.position).normalized;

                Move();
                
                CheckObstacle();
                    
                // Disparar si tengo la planta cerca
                if (Vector3.Distance(transform.position, actualPlant.transform.position) <= 5f)
                {
                    if(canAttack)
                        Attack();
                }
            }
        }

        private void GetGemState()
        {
            gem = GameObject.FindGameObjectWithTag(Constants.GEM_TAG);
            if (gem == null) return;

            // Mover hacia la primera plataforma
            GameObject closestPlatform = GetClosestPlatform();

            if (closestPlatform != null)
            {
                // Mover hacia la plataforma más cercana
                direction = (closestPlatform.transform.position + closestPlatform.transform.up * 0.5f - transform.position).normalized;
                
                Move();
                
                CheckObstacle();
                    
                // Salto si tengo la plataforma cerca
                if (Vector3.Distance(transform.position, closestPlatform.transform.position) <= 5f)
                {
                    if (isJumping) return;
                    
                    Jump();
                    StartCoroutine(nameof(WaitForNextPlatform));
                }
            }
            else
            {
                if (hasJumpedToGem) return;
             
                if (gem != null)
                {
                    if (isJumping) return;
                    StartCoroutine(nameof(WaitAndJumpToGem));
                }
            }
        }

        private IEnumerator WaitForNextPlatform()
        {
            yield return new WaitForSeconds(1f);
            currentPlatformIndex++;
        }

        private IEnumerator WaitAndJumpToGem()
        {
            yield return new WaitForSeconds(1f);
            
            hasJumpedToGem = true;
            
            //canMove = false;
            
            rb.AddForce(Vector3.up, ForceMode.Impulse);
        }
        
        #endregion

        #region ACTIONS
        private void Jump()
        {
            isJumping = true;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

            isGrounded = false;
            
            Invoke(nameof(ResetJump), 1f);
        }

        private void ResetJump()
        {
            isJumping = false;
            isGrounded = true;
        }

        private void Attack()
        {
            if (isShootCooldown) return;
            isAttacking = true;
            isShootCooldown = true;

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

           // MyAudioManager.Instance.PlaySfx("fireVoice");

            Instantiate(spellPrefab, spellSpawn.position, spellSpawn.rotation);

            yield return new WaitForSeconds(1f);
            
            isAttacking = false;
            
            canMove = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!this.isActiveAndEnabled) return;
            
            if (other.CompareTag(Constants.ENEMY_TAG))
            {
                PlayerHealth.Instance.AddDamage(10);
            } else if (other.CompareTag(Constants.GEM_TAG))
            {
                canMove = false;
            }
        }
        #endregion
    }
}
