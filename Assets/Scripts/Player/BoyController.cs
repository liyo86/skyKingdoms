using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class BoyController : MonoBehaviour, IAttackCooldown
    {
        #region INSPECTOR VARIABLES
        [Header("CONFIGURATION\n")]
        [Tooltip("Velocidad del personaje.")]
        [SerializeField]
        private float speed;
    
        [Tooltip("Rotación del personaje.")]
        [SerializeField]
        private float rotationSpeed;
    
        [Tooltip("Fuerza del salto.")]
        [SerializeField]
        private float jumpHeight;
    
        [Tooltip("Máscara de capa utilizada para verificar si el personaje está en el suelo.")]
        [SerializeField]
        private LayerMask groundLayerMask;
  
        [Tooltip("Distancia máxima para considerar que el personaje está en el suelo.")]
        [SerializeField]
        private float groundCheckDistance = 0.1f;
    
        [Tooltip("Prefab del hechizo de fuego.")]
        [SerializeField]
        private GameObject spellPrefab;
    
        [Tooltip("Prefab del hechizo de defensa.")]
        [SerializeField]
        private GameObject defensePrefab;
    
        [Tooltip("Prefab del hechizo especial.")]
        [SerializeField]
        private GameObject specialAttackPrefab;
    
        [Tooltip("Transform para instanciar los ataques.")]
        [SerializeField]
        private Transform spellSpawn;
    
        [Tooltip("Transform para instanciar los ataques especiales.")]
        [SerializeField]
        private Transform specialSpellSpawn;
        
        [Tooltip("Offset Collider para interact.")]
        public Collider OffsetCollider;
        

        //CoolDown
        public float defenseCooldown; // Tiempo de reutilización de la defensa
    
        public float specialAttackCooldown; // Tiempo de reutilización del ataque especial
    
        public float shootCooldown; // Tiempo de reutilización del disparo

        public bool isDefenseCooldown = false;
        public bool isSpecialAttackCooldown = false;
        public bool isShootCooldown = false;
    
        public bool IsDefenseCooldown => isDefenseCooldown;
        public float DefenseCooldown => defenseCooldown;
        public bool IsShootCooldown => isShootCooldown;
        public float ShootCooldown => shootCooldown;
        public bool IsSpecialAttackCooldown => isSpecialAttackCooldown;
        public float SpecialAttackCooldown => specialAttackCooldown;
        #endregion

        #region REFERENCES
        private Rigidbody rb;
        private Animator animator;
        private Vector3 movement;
        #endregion
    
        #region PRIVATE VARIABLES
        private bool _damaged;
        private bool isJumping;
        private bool isShooting;
        private bool isShootingInAir;
        private bool isGrounded = true;
        private bool isDefending;
        private float gravity = 9.8f; // Valor de la gravedad
        private Coroutine shootCoroutine;
        private float loadTime = 1f;
        private GameObject currentSpecialAttack;
        private Rigidbody rbCurrentSpecialAttack;
        private bool r2Triggered;
        private static readonly int Specialattack = Animator.StringToHash("specialattack");
        private static readonly int Releasespecial = Animator.StringToHash("releasespecial");
        #endregion

        #region PUBLIC VARIABLES
        public static BoyController Instance;

        public bool CanMove { get; set; } = false;

        public bool IsDefending => isDefending;
        
        #endregion
    
        #region UNITY METHODS
        private void Awake()
        {
            Instance = this;
        
            rb = GetComponentInChildren<Rigidbody>();
            animator = GetComponentInChildren<Animator>();

        }
        
        private void Update()
        {
            if (Interaction.Instance.IsInteracting) return;
            
            if (!CanMove) return;
            CheckGrounded();

            // Verificar si el personaje está en el aire y se debe disparar
            if (!isGrounded && isShootingInAir)
            {
                movement = Vector2.zero;
                isShooting = true;
                animator.SetTrigger("shoot");

                // Iniciar la animación de disparo y esperar a que termine antes de continuar
                shootCoroutine = StartCoroutine(ShootAndWait());
                
                isShootingInAir = false; // Reiniciar la bandera para evitar disparos continuos en el aire
            }
        }

        private void FixedUpdate()
        {
            if (!CanMove) return;
            FixedControls();   
        }

        private void LateUpdate()
        {
            if (!CanMove) return;
            animator.SetFloat("speed", movement.magnitude);
            animator.SetBool("walk", movement.magnitude > 0f);
            animator.SetBool("jump", isJumping && !isShooting);
            animator.SetBool("land", isGrounded);
        }
        
        void FixedControls()
        {
            // Movimiento del personaje
            Vector3 direction = new Vector3(movement.x, 0, movement.y);

            // Calcula la dirección del movimiento relativa a la cámara
            Vector3 relativeDirection = Camera.main.transform.TransformDirection(direction);
            relativeDirection.y = 0;
            relativeDirection.Normalize();

            // MOVIMIENTO
            Vector3 move = relativeDirection * speed * Time.deltaTime;
            rb.MovePosition(rb.position + move);

            // ROTACION
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(relativeDirection, Vector3.up);
                rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                if (r2Triggered && currentSpecialAttack != null)
                    rbCurrentSpecialAttack.rotation = Quaternion.Slerp(rbCurrentSpecialAttack.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // MOVIMIENTO DEL ATAQUE ESPECIAL
            if (r2Triggered && currentSpecialAttack != null)
            {
                rbCurrentSpecialAttack.MovePosition(specialSpellSpawn.position);
                rbCurrentSpecialAttack.MoveRotation(specialSpellSpawn.rotation);
            }

        }
        #endregion

        #region SHOOT
        public void SetShootPermormed()
        {
            if (!CanMove || isShootCooldown) return;
            isShootCooldown = true;

            //if (isGrounded)
            //{
            movement = Vector2.zero;
            isShooting = true;
            animator.SetTrigger("shoot");
            StartCoroutine(nameof(Shoot));
                
            StartCoroutine(ShootCooldownTimer());
        }
        
        private IEnumerator ShootCooldownTimer()
        {
            yield return new WaitForSeconds(shootCooldown);
            isShootCooldown = false;
        }

        IEnumerator Shoot()
        {
            yield return new WaitForSeconds(0.5f);

          //  MyAudioManager.Instance.PlaySfx("fireVoice");

            GameObject attack = Instantiate(spellPrefab, spellSpawn.position, spellSpawn.rotation);

            attack.SetActive(true);
            
            StartCoroutine(nameof(LaunchAttack), attack);
            
            isShooting = false;
        }
        
        private IEnumerator ShootAndWait()
        {
            yield return new WaitForSeconds(0.5f);

            //MyAudioManager.Instance.PlaySfx("fireVoice");

            Instantiate(spellPrefab, spellSpawn.position, spellSpawn.rotation);
            
            isShooting = false;
            
            yield return new WaitForSeconds(0.5f);

            shootCoroutine = null;
        }
        
        private IEnumerator LaunchAttack(GameObject attack)
        {
            Rigidbody _rbAttack = attack.GetComponent<Rigidbody>();
            _rbAttack.velocity =  _rbAttack.transform.forward * 5f;
            

            yield return new WaitForSeconds(10f);

            Destroy(attack.gameObject);
        }
        
        #endregion

        #region MOVEMENT
        public void SetMovementPerformed(Vector3 movementInput)
        {
            movement = movementInput;
        }

        public void SetMovementCanceled(Vector3 movementInput)
        {
            movement = movementInput;
        }

        #endregion

        #region JUMP
        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            if (!CanMove) return;
            
            if (!isJumping && isGrounded)
            {
                isJumping = true;
                isGrounded = false;

                float jumpForce = Mathf.Sqrt(2f * jumpHeight * gravity);
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }
            
            // Verificar si se debe disparar en el aire
            if (isShooting)
            {
                isShootingInAir = true;
                isShooting = false;

                // Detener la rutina de disparo si está en progreso
                if (shootCoroutine != null)
                    StopCoroutine(shootCoroutine);
            }
        }
    
        private void CheckGrounded()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayerMask))
            {
                isGrounded = true;
            }     

            if (rb.velocity.y != 0f && !isGrounded)
            {
                isJumping = true;
            } else if (rb.velocity.y == 0f && isGrounded)
            {
                isJumping = false;
            }
        }

        // Controlado por evento de animator
        private IEnumerator WaitAndWalk()
        {
            CanMove = false;

            yield return new WaitForSeconds(0.5f);

            CanMove = true;
        }
        #endregion
    
        #region DEFENSE
        
        public void SetDefensePerformed()
        {
            if (!CanMove || isDefenseCooldown) return;

            CanMove = false;

            isDefending = true;
          //  MyAudioManager.Instance.PlaySfx("defenseVoice");
            defensePrefab.SetActive(true);
            
            StartCoroutine(DefenseActive());   
        }

        
        private IEnumerator DefenseActive()
        {
            yield return new WaitForSeconds(0.5f);
            CanMove = true;
            
            yield return new WaitForSeconds(1f);
            defensePrefab.SetActive(false);
            isDefending = false;
            
            isDefenseCooldown = true;
            StartCoroutine(DefenseCooldownTimer());
        }
        
        private IEnumerator DefenseCooldownTimer()
        {
            yield return new WaitForSeconds(defenseCooldown);
            isDefenseCooldown = false;
        }
    
        #endregion
    
        #region SPECIAL ATTACK

        private void OnR2Hold(InputAction.CallbackContext context)
        {
            //if (!CanMove) return;
            while (true) return;

            if (context.performed)
            {
                if (currentSpecialAttack != null || isSpecialAttackCooldown) return;
                r2Triggered = true;
                animator.SetTrigger(Specialattack);
                StartCoroutine(LoadSpecialAttack());
            }
            else if (context.canceled)
            {
                if (currentSpecialAttack == null) return;
                r2Triggered = false;
                animator.SetTrigger(Releasespecial);
                StartCoroutine(nameof(LaunchSpecialAttack));

                isSpecialAttackCooldown = true;
                StartCoroutine(SpecialAttackCooldownTimer());
            }
        }

        private IEnumerator SpecialAttackCooldownTimer()
        {
            yield return new WaitForSeconds(specialAttackCooldown);
            isSpecialAttackCooldown = false;
        }


        private IEnumerator LaunchSpecialAttack()
        {
            rbCurrentSpecialAttack.velocity = rbCurrentSpecialAttack.transform.forward * 10f;

            yield return new WaitForSeconds(2f);

            // Eliminar el ataque especial después de cierto tiempo
            Destroy(currentSpecialAttack.gameObject);
            currentSpecialAttack = null;
        }


        private IEnumerator LoadSpecialAttack()
        {
            var elapsedTime = 0f;
            var initialScale = new Vector3(0.1f, 0.1f, 0.1f);
            var targetScale = new Vector3(1, 1, 1);

            currentSpecialAttack = Instantiate(specialAttackPrefab, specialSpellSpawn.position, Quaternion.identity) as GameObject;
            rbCurrentSpecialAttack = currentSpecialAttack.GetComponent<Rigidbody>();

            currentSpecialAttack.transform.LookAt(transform);

            while (elapsedTime < loadTime)
            {
                if (currentSpecialAttack != null)
                {
                    float t = elapsedTime / loadTime;
                    currentSpecialAttack.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
        }

        #endregion
        
        #region TRIGGERS
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Dragon")) //TODO controlar, en story_1 peta
            {
                if(MyLevelManager.Instance.backToScene) return;
                //sceneManager.LoadScene();
            }
            else if (other.CompareTag("Limit"))
            {
                MyGameManager.Instance.GameOver();
            }

            if (isDefending) return;
            
            if (other.CompareTag("Enemy") && !_damaged)
            {
                _damaged = true;
                PlayerHealth.Instance.AddDamage(10);
                StartCoroutine(Damaged(other.transform.position));
                animator.SetTrigger("damage");
            } 
            else if (other.CompareTag("BossAttack1") && !_damaged)
            {
                _damaged = true;
                PlayerHealth.Instance.AddDamage(25);
                StartCoroutine(Damaged(other.transform.position));
                animator.SetTrigger("damage");
            }
            else if (other.CompareTag("Bat") && !_damaged)
            {
                _damaged = true;
                PlayerHealth.Instance.AddDamage(30);
                StartCoroutine(Damaged(other.transform.position));
                animator.SetTrigger("damage");
            }

            if (_damaged) StartCoroutine(nameof(DamagedCoolDown));
        }

        private IEnumerator DamagedCoolDown()
        {
            yield return new WaitForSeconds(1f);
            _damaged = false;

        }

        // Recoil
        private IEnumerator Damaged(Vector3 enemyPos)
        {
            Vector3 recoilDirection = (transform.position - enemyPos).normalized;
            rb.AddForce(recoilDirection * 20f, ForceMode.Impulse);

            yield return new WaitForSeconds(1f);
        }

        public void ShowDragon(Vector3 position)
        {
            //Dragon.GetComponent<Transform>().position = position;
            //Dragon.SetActive(true);
           // Dragon.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            //Dragon.transform.DOMoveY(transform.position.y, 2).SetEase(Ease.Linear).Play();
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        #endregion
    }
}
