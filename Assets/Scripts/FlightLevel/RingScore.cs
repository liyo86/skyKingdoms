using System.Collections;
using UnityEngine;
using DG.Tweening;

public class RingScore : MonoBehaviour
{
    private bool hasScoreUp;
    
    private bool hasScoreDown;

    private ParticleSystem ringDoneParticles;

    private GameObject fireRing;
    
    private RectTransform ringsUI;

    private Vector3 ringDonePosition;

    private bool checkUpCollision;
    
    private bool checkDownCollision;

    public DOTElementToUI PlusOne;

    private void Awake()
    {
        fireRing = GameObject.FindGameObjectWithTag("Particles");
        ringsUI = GameObject.FindGameObjectWithTag("RingsUI").GetComponent<RectTransform>();
    }

    private void Start()
    {
        GameObject fireRingInstantiate = Instantiate(fireRing, transform.position, Quaternion.identity);
        ringDoneParticles = fireRingInstantiate.GetComponentInChildren<ParticleSystem>();

        Vector3 posicionUI = ringsUI.transform.position;
        ringDonePosition = RectTransformUtility.WorldToScreenPoint(Camera.main, posicionUI);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("RingUp") && !hasScoreUp)
        {
            hasScoreUp = true;
            checkUpCollision = true;
            CheckCollisions();

        } else if (col.CompareTag("RingDown") && !hasScoreDown)
        {
            hasScoreDown = true;
            checkDownCollision = true;
            CheckCollisions();
        }
    }

    void CheckCollisions()
    {
        if (checkUpCollision && checkDownCollision)
        {
            PlusOne.StartShrinkAndMove();
            FlightLevel.Instance.ringDone++;
            MyAudioManager.Instance.PlaySfx("ringSFX");
            ringDoneParticles.Play();
            PlayerUI.Instance.ScoreRingPoints();
            FlightLevel.Instance.AddSecond();
            StartCoroutine(WaitAndScaleRing());
        }
    }

    private IEnumerator WaitAndScaleRing()
    {
        yield return new WaitForSeconds(1f);
        
        transform.DOScale(0f, 1f).SetEase(Ease.Flash);
    }
}
