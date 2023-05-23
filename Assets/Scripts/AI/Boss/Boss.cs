using System.Collections;
using System.Collections.Generic;
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
    public float SecondsListening
    {
        get => _secondsListening;
        set => _secondsListening = value;
    }

    #endregion
    
    #region WAYPOINTS
    [Header("WayPoints")]
    private List<Transform> _wayPointsList = new List<Transform>();
    
    private int _actualWayPoint = 0;
    #endregion
    
    #region IA
    
    public enum BossPhase
    {
        Phase1,
        Phase2,
        Phase3
    }
    
    private FSMBoss _currentPhase;
    
    private bool _canListen;

    public bool CanListen
    {
        get => _canListen;

        set => _canListen = value;
    }
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
    
    public float _timeBetweenJumps = 2f; // Tiempo entre saltos (segundos)
    public float _jumpTimer; // Temporizador para el salto
    public float jumpForce = 15f;
    private ParticleSystem shockWaveParticle;
    private Vector3 originalPosition;
    public GameObject AttackPhase1;

    #region UNITY METHODS

    private void Awake()
    { 
        bossRigidbody = GetComponent<Rigidbody>();
        shockWaveParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        ChangePhase(BossPhase.Phase1);
    }
    
    void Update()
    {
        _currentPhase.Execute(this);
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
        MyAudioManager.Instance.PlaySfx("bossAttack1SFX");
        Vector3 attackOnePosition = new Vector3(transform.position.x, UnityEngine.Random.Range(1, 3), transform.position.z - 6);
        Instantiate(AttackPhase1, attackOnePosition, Quaternion.identity);
        shockWaveParticle.Play();
    }

}
