using Player;
using UnityEngine;

public class GoblinBoss : MonoBehaviour
{
    private DOTGoblinWalk DOTGoblin;
    private Animator anim;

    public Transform target; // Referencia al transform del jugador
    public GameObject Bat;
    public Transform batOriginalPosition; // Posición original del bat
    public float speed; // Velocidad de movimiento del enemigo
    public float batSpeed; // Tiempo que tarda el arma en llegar al player
    public float batReturnSpeed; // Tiempo que tarda el arma en regresar a la mano
    public float chaseDuration; // Duración de la persecución
    public float restDuration; // Duración del descanso
    public float visionRange;

    private float chaseTimer; // Temporizador de persecución
    private float restTimer; // Temporizador de descanso
    private bool isResting; // Indica si el enemigo está descansando
    private bool isAttacking;
    private bool isBatReturning;

    public bool fight;

    private Vector3 batTargetPosition; // Posición objetivo del bate cuando se lanza

    private void Awake()
    {
        DOTGoblin = GetComponent<DOTGoblinWalk>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //MyAudioManager.Instance.PlayMusic("finalBoss");
        Bat.transform.position = batOriginalPosition.position;
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        fight = true;
        StartChase();
    }

    
    private void Update()
{
    if (!fight) return;

    if (isAttacking)
    {
        if (!isBatReturning)
        {
            Bat.transform.position = Vector3.MoveTowards(Bat.transform.position, batTargetPosition, batSpeed * Time.deltaTime);

            if (Vector3.Distance(Bat.transform.position, batTargetPosition) <= 0.5f)
            {
                if (BoyController.Instance.IsDefending)
                {
                    Bat.transform.position = batOriginalPosition.position;
                    Bat.transform.rotation = Quaternion.Euler(0f, 0, 50f);
                    isAttacking = false;
                    isBatReturning = false;
                    StartRest();

                    //GoblinBossHealth.Instance.AddDamage(30);
                }
                else
                {
                    isBatReturning = true;
                }
            }
        }
        else
        {
            Bat.transform.position = Vector3.MoveTowards(Bat.transform.position, batOriginalPosition.position, batReturnSpeed * Time.deltaTime);

            if (Vector3.Distance(Bat.transform.position, batOriginalPosition.position) <= 0.5f)
            {
                isAttacking = false;
                isBatReturning = false;
                StartRest();
            }
        }
    }
    else if (!isResting)
    {
        Bat.transform.position = batOriginalPosition.position;
        Bat.transform.rotation = Quaternion.Euler(0f, 0, 50f);

        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.LookAt(target);

            if (Vector3.Distance(transform.position, target.position) <= visionRange)
            {
                StartAttack();
            }
        }

        chaseTimer -= Time.deltaTime;
        if (chaseTimer <= 0f)
        {
            StartRest();
        }
    }
    else if (isResting)
    {
        Bat.transform.position = batOriginalPosition.position;
        Bat.transform.rotation = Quaternion.Euler(0f, 0, 50f);

        restTimer -= Time.deltaTime;
        if (restTimer <= 0f)
        {
            StartChase();
        }
    }
}

    private void StartChase()
    {
        chaseTimer = chaseDuration;
        isResting = false;
        DOTGoblin.DoWalk();
    }

    private void StartRest()
    {
        restTimer = restDuration;
        isResting = true;
        DOTGoblin.DoStop();
    }

    private void StartAttack()
    {
        isAttacking = true;
        DOTGoblin.DoStop();
        anim.SetTrigger("action");
        
        Vector3 targetDirection = target.position - batOriginalPosition.position;
        targetDirection.Normalize();
        batTargetPosition = batOriginalPosition.position + targetDirection * 10f;
    }
}
