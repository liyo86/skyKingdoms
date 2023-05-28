    using System.Collections;
    using UnityEngine;
    using Cinemachine;

    public class DollyTrackLastWaypoint : MonoBehaviour
    {
        #region VARIABLES
        public CinemachineDollyCart dollyCartSeq1;
        public CinemachineDollyCart dollyCartSeq2;
        public CinemachineDollyCart dollyCartSeq3;
        public CinemachineDollyCart dollyCartSeq4;
        public GameObject canvasToActivate;
        public GameObject RainBow;
        public GameObject FireAttack;
        public LoadScreenManager _LoadScreenManager;

        private bool dragonActivated = false;
        private bool canvasActivated = false;
        private bool btnPressed;


        [Header("Characters")] 
        public GameObject boy_begins;
        private Renderer boy_begins_render;
        private bool boy_begins_anim;
        private bool boy_begins_animIdle;

        public GameObject boy_ends;
        public Animator boy_ends_animator;


        public GameObject boy_magic;
        private Animator boy_magic_animator;
        #endregion

        #region References
        private float pathLength1;
        private float pathLength3;
        private float pathLength4;

        #endregion
        
        #region UNITY METHODS
        private void Awake()
        {
            boy_begins_render = boy_begins.GetComponentInChildren<Renderer>();
            boy_magic_animator = boy_magic.GetComponent<Animator>();
            boy_ends_animator = boy_ends.GetComponent<Animator>();
        }

        void Start()
        {
            if (dollyCartSeq1.m_Path != null)
            {
                pathLength1 = dollyCartSeq1.m_Path.PathLength;
            }

            if (dollyCartSeq3.m_Path != null)
            {
                pathLength3 = dollyCartSeq3.m_Path.PathLength;
            }
            
            if (dollyCartSeq4.m_Path != null)
            {
                pathLength4 = dollyCartSeq4.m_Path.PathLength;
            }
     
        }

        void Update()
        {
            DollyCart();
        }
        #endregion

        #region CHARACTER SEQUENCES
        private IEnumerator BoyMagicActions()
        {
            dollyCartSeq4.m_Speed = 1.5f;
            boy_magic_animator.SetFloat("speed", 2f);

            yield return new WaitForSeconds(2.5f);
            boy_magic_animator.SetFloat("speed", 0f);
            boy_magic_animator.SetTrigger("shoot");
            
            
            yield return new WaitForSeconds(0.5f);
            FireAttack.SetActive(true);
            FireAttack.transform.Translate(Vector3.forward * 5f * Time.deltaTime);
        }

        private IEnumerator DrawRainbow()
        {
            yield return new WaitForSeconds(6.5f);
            RainBow.SetActive(true);
        }
        #endregion
        
        #region CAMERA MOVEMENT
        private void DollyCart()
        {
            if (!dragonActivated && dollyCartSeq1.m_Position >= pathLength1)
            {
                dragonActivated = true;
                boy_ends.SetActive(true);
                boy_ends_animator.SetBool("jump", false);
                dollyCartSeq2.m_Speed = 15;
            }
            
            if (!canvasActivated && dollyCartSeq1.m_Position >= pathLength1) // He llegado al final del track
            {
                canvasActivated = true;
                canvasToActivate.SetActive(true);
            }

            if (!boy_begins_animIdle && dollyCartSeq3.m_Position >= pathLength3 - 5) // Animación idle del primer boy
            {
                boy_begins_animIdle = true;

                //Cuando termina la animación del primer boy
                StartCoroutine(BoyMagicActions());
            }
            
            if (dollyCartSeq4.m_Position >= pathLength4) // Canvas final
            {
                StartCoroutine(DrawRainbow());
            }
        }
        #endregion
        
        #region UI
        public void EndsCinematic()
        {
            if (!btnPressed)
            {
                btnPressed = true;
                canvasToActivate.SetActive(false);
                _LoadScreenManager.LoadScene();
            }
        }
        #endregion
    }