using System.Collections;
using Player;
using UnityEngine;

public class Boss : MonoBehaviour
{
    #region CONFIGURATION
    
    [Header("Tags Necesarios:\n" +
            "Player: Transform del Player.\n" +
            "WayPoint: Transform de cada punto de platrulla.\n" +
            "PlayerInitialPosition: Transform de destino del Player\n si hay game over.\n\n")]
    
    [SerializeField]
    [Tooltip("Radio del sentido escucha.")]
    private float _listenRadio;
    
    [SerializeField]
    [Tooltip("Distancia a la que vemos al player.")]
    private float _sightAware;
    
    [SerializeField]
    [Tooltip("Distancia mínima para ser alcanzado.")]
    private float _playerFollowDistance;
    
    [SerializeField] 
    [Tooltip("Tiempo que tarda en volver a patrulla desde cualquier sentido.")]
    private float _resetSeconds;
    
    [SerializeField] 
    [Tooltip("Tiempo que tardo en resetar el sentido del oído.")]
    private float _resetEarSenseSeconds;
    
    [SerializeField] 
    [Tooltip("Tiempo que tardo en patrullar desde que he escuchado al player.")]
    private float _secondsListening;

    private float _secondsListeningSaved;

    #endregion
    
    #region IA
    public enum BossPhase
    {
        Phase1,
        Phase2,
        Phase3
    }
    
    private FSMBoss _currentPhase;
    #endregion
    
    #region REFERENCIAS
   
    private GameObject _player;
    
    public GameObject Player
    {
        get => _player;
    }
    
    private Rigidbody _playerRB;

    private Rigidbody bossRigidbody;

    private Vector2 _direction;

    private Transform _playerInitialPosition;

    public Transform PlayerInitialPosition
    {
        get => _playerInitialPosition;
    }

    #endregion

    public static Boss Instance;
    public float _timeBetweenJumps; // Tiempo entre saltos (segundos)
    private float _jumpTimer; // Temporizador para el salto
    public float jumpForce;
    private ParticleSystem shockWaveParticle;
    private Vector3 originalPosition;
    public GameObject AttackPhase1;
    private bool gameOver;
    private bool fight;

    #region UNITY METHODS

    private void Awake()
    {
        Instance = this;
        bossRigidbody = GetComponent<Rigidbody>();
        shockWaveParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        StartCoroutine(StartIA());
    }
    
    void Update()
    {
        if (!fight) return;
        _currentPhase.Execute(this);
        if(MyGameManager.Instance.gameOver)
            GameOver();
    }

    private bool CheckPlayerStart() => BoyController.Instance.CanMove;

    private IEnumerator StartIA()
    {
        yield return new WaitUntil(CheckPlayerStart);
        ChangePhase(BossPhase.Phase1);
        fight = true;
    }
    #endregion
    
    public void ChangePhase(BossPhase newPhase)
    {
        switch (newPhase)
        {
            case BossPhase.Phase1:
                _currentPhase = new Phase1();
                break;
            case BossPhase.Phase2:
                _currentPhase = new Phase2();
                break;
            case BossPhase.Phase3:
                _currentPhase = new Phase3();
                break;
        }
    }
    
    public void JumpAndCreateShockwave()
    {
        _jumpTimer += Time.deltaTime;

        if (_jumpTimer >= _timeBetweenJumps)
        { 
            bossRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(WaitAndShoot());
            _jumpTimer = 0;
        }
       
    }

    private IEnumerator WaitAndShoot()
    {
        yield return new WaitForSeconds(0.5f);
        
        if(shockWaveParticle != null)
            shockWaveParticle.Play();
        
        MyAudioManager.Instance.PlaySfx("bossAttack1SFX");
        
        Vector3 attackOnePosition = new Vector3(transform.position.x, UnityEngine.Random.Range(0f, 2f), transform.position.z - 6);
        
        Instantiate(AttackPhase1, attackOnePosition, Quaternion.identity);
    }

    void GameOver()
    {
        if (gameOver) return;
        StopAllCoroutines();
        gameOver = true;
    }
}
