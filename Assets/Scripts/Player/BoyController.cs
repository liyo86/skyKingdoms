using System.Collections;
using DG.Tweening;
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
    
        [Header("Jump Landing")]
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

    
        public LoadScreenManager sceneManager;
        #endregion

        #region REFERENCES
        private Rigidbody rb;
        private Animator animator;
        private Vector3 movement;

        // New Input System
        private InputAction movementAction;
        private InputAction shootAction;
        private InputAction jumpAction;
        private InputAction defenseAction;    
        private InputAction specialAttackAction;
        public InputActionAsset inputActions;
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
        public float loadTime = 1f;
        private GameObject currentSpecialAttack;
        private Rigidbody rbCurrentSpecialAttack;
        private bool r2Triggered;
        private static readonly int Specialattack = Animator.StringToHash("specialattack");
        private static readonly int Releasespecial = Animator.StringToHash("releasespecial");
        #endregion

        #region PUBLIC VARIABLES
        public static BoyController Instance;

        public GameObject Dragon;
        public bool CanMove { get; set; } = false;

        public bool IsDefending => isDefending;
        
        #endregion
    
        #region UNITY METHODS
        private void Awake()
        {
            Instance = this;
        
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            movementAction = inputActions.FindActionMap("Player").FindAction("Movement");
            shootAction = inputActions.FindActionMap("Player").FindAction("Shoot");
            jumpAction = inputActions.FindActionMap("Player").FindAction("Jump");
            defenseAction = inputActions.FindActionMap("Player").FindAction("Defense");
            specialAttackAction = inputActions.FindActionMap("Player").FindAction("Special Attack");
        }

        private void OnEnable()
        {
            movementAction.performed += OnMovementPerformed;
            movementAction.canceled += OnMovementCanceled;
            movementAction.Enable();
        
            shootAction.performed += OnShootPerformed;
            shootAction.Enable();

            jumpAction.performed += OnJumpPerformed;
            jumpAction.Enable();

            defenseAction.performed += OnDefensePerformed;
            defenseAction.Enable();
        
            specialAttackAction.performed += OnR2Hold;
            specialAttackAction.canceled += OnR2Hold;
            specialAttackAction.Enable();
        }

        private void OnDisable()
        {
            movementAction.performed -= OnMovementPerformed;
            movementAction.canceled -= OnMovementCanceled;
            movementAction.Disable();
        
            shootAction.performed -= OnShootPerformed;
            shootAction.Disable();
        
            jumpAction.performed -= OnJumpPerformed;
            jumpAction.Disable();
        
            defenseAction.performed -= OnDefensePerformed;
            defenseAction.Disable();
        
            specialAttackAction.performed -= OnR2Hold;
            specialAttackAction.canceled -= OnR2Hold;
            specialAttackAction.Disable();
        }

        private void Update()
        {
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
        private void OnShootPerformed(InputAction.CallbackContext context)
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
            //}
        }
        
        private IEnumerator ShootCooldownTimer()
        {
            yield return new WaitForSeconds(shootCooldown);
            isShootCooldown = false;
        }

        IEnumerator Shoot()
        {
            yield return new WaitForSeconds(0.5f);

            MyAudioManager.Instance.PlaySfx("fireVoice");

            Instantiate(spellPrefab, spellSpawn.position, spellSpawn.rotation);
            
            isShooting = false;
        }
        
        private IEnumerator ShootAndWait()
        {
            yield return new WaitForSeconds(0.5f);

            MyAudioManager.Instance.PlaySfx("fireVoice");

            Instantiate(spellPrefab, spellSpawn.position, spellSpawn.rotation);
            
            isShooting = false;

            // Esperar a que termine la animación de disparo antes de reactivar los controles
            yield return new WaitForSeconds(0.5f);

            shootCoroutine = null;
        }
        
        #endregion

        #region MOVEMENT
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            movement = context.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            movement = Vector2.zero;
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

        private void OnDefensePerformed(InputAction.CallbackContext context)
        {
            if (!CanMove || isDefenseCooldown) return;

            CanMove = false;

            isDefending = true;
            MyAudioManager.Instance.PlaySfx("defenseVoice");
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
            // Lanzar el ataque especial en la dirección hacia la que mira el jugador
            Vector3 attackDirection = transform.position;
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
        
        // SACAR FUERA DE ESTE SCRIPT
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Dragon"))
            {
                sceneManager.LoadScene();
            }
            else if (other.CompareTag("Enemy") && !_damaged && !isDefending)
            {
                _damaged = true;
                Debug.Log("Entro y añado daño");
                PlayerHealth.Instance.AddDamage(10);
                StartCoroutine(Damaged(other.transform.position));
                animator.SetTrigger("damage");
            } 
            else if (other.CompareTag("BossAttack1") && !_damaged && !isDefending)
            {
                _damaged = true;
                PlayerHealth.Instance.AddDamage(25);
                StartCoroutine(Damaged(other.transform.position));
                animator.SetTrigger("damage");
            }
            else if (other.CompareTag("Bat") && !_damaged && !isDefending)
            {
                _damaged = true;
                PlayerHealth.Instance.AddDamage(30);
                StartCoroutine(Damaged(other.transform.position));
                animator.SetTrigger("damage");
            } 
            else if (other.CompareTag("Limit"))
            {
                MyGameManager.Instance.GameOver();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _damaged = false;
        }

        IEnumerator Damaged(Vector3 enemyPos)
        {
            // Recoil
            Vector3 recoilDirection = (transform.position - enemyPos).normalized;
            rb.AddForce(recoilDirection * 20f, ForceMode.Impulse);

            yield return new WaitForSeconds(1f);
        }

        public void ShowDragon(Vector3 position)
        {
            Dragon.GetComponent<Transform>().position = position;
            Dragon.SetActive(true);
            Dragon.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            Dragon.transform.DOMoveY(transform.position.y, 2).SetEase(Ease.Linear).Play();
        }
    }
}
