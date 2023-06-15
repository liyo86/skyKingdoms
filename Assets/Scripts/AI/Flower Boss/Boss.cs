using System.Collections;
using Managers;
using Player;
using UnityEngine;

public class Boss : MonoBehaviour
{
    #region CONFIGURATION
    
    [Header("Tags Necesarios:\n" +
            "Player: Transform del Player.\n" +
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

    public Rigidbody bossRigidbody;

    private Vector2 _direction;

    private Transform _playerInitialPosition;

    public Transform PlayerInitialPosition
    {
        get => _playerInitialPosition;
    }

    #endregion
    
    public ParticleSystem shockWaveParticle;
    private Vector3 originalPosition;
    public GameObject AttackPhase1;
    private bool gameOver;
    private bool fight;

    #region UNITY METHODS

    private void Awake()
    {
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
    
    
    void GameOver()
    {
        if (gameOver) return;
        StopAllCoroutines();
        gameOver = true;
    }
}
