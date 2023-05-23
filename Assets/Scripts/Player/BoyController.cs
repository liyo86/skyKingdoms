using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoyController : MonoBehaviour
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
    
    [Tooltip("Transform para instanciar los ataques.")]
    [SerializeField]
    private Transform spellSpawn;
    
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
    public InputActionAsset inputActions;
    #endregion
    
    #region PRIVATE VARIABLES
    private bool dragonCollision;
    private bool _damaged;
    private bool isJumping;
    private bool isGrounded = true;
    private bool isDefending;
    private bool boyControl = true;
    #endregion

    #region PUBLIC VARIABLES
    public static BoyController Instance;
    public bool BoyControl
    {
        set => boyControl = value;
    }

    public bool HasGemBlue = false;

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
    }

    private void Update()
    {
        CheckGrounded();
    }

    private void FixedUpdate()
    {
        FixedControls();   
    }

    void FixedControls()
    {
        // Movimiento del personaje
        Vector3 direction = new Vector3(movement.x, 0, movement.y);

        // Calcula la dirección del movimiento relativa a la cámara
        Vector3 relativeDirection = Camera.main.transform.TransformDirection(direction);
        relativeDirection.y = 0;
        relativeDirection.Normalize();

        // Aplica la velocidad al movimiento
        Vector3 move = relativeDirection * speed * Time.deltaTime;
        rb.MovePosition(rb.position + move);

        // Orienta el personaje en la dirección del movimiento
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(relativeDirection, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        animator.SetFloat("speed", movement.magnitude);
        animator.SetBool("walk", movement.magnitude > 0f);
        animator.SetBool("jump", isJumping);
        animator.SetBool("land", isGrounded);
    }
    
    #endregion

    #region SHOOT
    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            movement = Vector2.zero;
            animator.SetTrigger("shoot");
            StartCoroutine(nameof(Shoot));
        }
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.5f);

        MyAudioManager.Instance.PlaySfx("fireVoice");

        Instantiate(spellPrefab, spellSpawn.position, spellSpawn.rotation);
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
    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (!isJumping && isGrounded)
        {
            isJumping = true;
            isGrounded = false;
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), rb.velocity.z);
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
    #endregion
    
    #region DEFENSE

    void OnDefensePerformed(InputAction.CallbackContext context)
    {
        isDefending = true;
        MyAudioManager.Instance.PlaySfx("defenseVoice");
        defensePrefab.SetActive(true);
        StartCoroutine(DefenseActive());
    }

    IEnumerator DefenseActive()
    {
        yield return new WaitForSeconds(3f);
        defensePrefab.SetActive(false);
        isDefending = false;
    }
    
    #endregion
    
    // SACAR FUERA DE ESTE SCRIPT
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dragon") && !dragonCollision)
        {
            dragonCollision = true;
            
            if (HasGemBlue)
            {
                sceneManager.LoadScene();
            }
            else
            { 
                MyDialogueManager.Instance.TextLevel1();
            }
        }
        else if (other.CompareTag("Enemy") && !_damaged && !isDefending)
        {
            _damaged = true;
            PlayerHealth.Instance.AddDamage(10);
            StartCoroutine(Damaged(other.transform.position));
            animator.SetTrigger("damage");
        } else if (other.CompareTag("BossAttack1") && !_damaged && !isDefending)
        {
            _damaged = true;
            PlayerHealth.Instance.AddDamage(25);
            StartCoroutine(Damaged(other.transform.position));
            animator.SetTrigger("damage");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _damaged = false;
        dragonCollision = false;
    }

    IEnumerator Damaged(Vector3 enemyPos)
    {
        // Recoil
        Vector3 recoilDirection = (transform.position - enemyPos).normalized;
        rb.AddForce(recoilDirection * 20f, ForceMode.Impulse);

        yield return new WaitForSeconds(1f);
    }

}
