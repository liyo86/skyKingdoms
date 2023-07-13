using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using Managers;
using Player;
using Service;
using UnityEngine;
using UnityEngine.Serialization;

public class DollyTrack_Story_1 : MonoBehaviour
{
    #region VARIABLES
    public CinemachineDollyCart dollyCartSeq1;
    public CinemachineDollyCart dollyCartSeq2;
    public CinemachineVirtualCamera finalCam;
    [FormerlySerializedAs("leoCam")] 
    public CinemachineFreeLook playerCam;

    [FormerlySerializedAs("Player")]
    [FormerlySerializedAs("Leo")] 
    [Header("Characters")]
    public GameObject player;
    private Animator _anim;

    private bool btnPresed = false;
    #endregion

    private void Awake()
    {
        _anim = player.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (MyLevelManager.Instance.backToScene)
        {
            GameControl();
            SetDragonPosition();
        }
        else
        {
            if (dollyCartSeq1.m_Path != null)
                dollyCartSeq1.m_Speed = 12;
        
            if (dollyCartSeq2.m_Path != null)
                dollyCartSeq2.m_Speed = 0;
        
    
            _anim.SetBool("walk", false);
        
            StartCoroutine(DollyCart());   
        }
    }

    private void Update()
    {
        if (ServiceLocator.GetService<MyInputManager>().AnyBtnPressed)
        {
            GameControl();
            SetDragonPosition();
        }
    }

    private IEnumerator DollyCart()
    {
        MyDialogueManager.Instance.Init(); 
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);

        //**************************************Cambio de camara
        finalCam.enabled = true;
        //******* Leo Camina acercandose al Dragon
        dollyCartSeq2.m_Speed = 2;
        _anim.SetBool("walk", true);
        yield return new WaitForSeconds(2.5f);
        dollyCartSeq2.m_Speed = 0;
        _anim.SetBool("walk", false);
        //********* Leo se para
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);

        GameControl();
    }

    public void GameControl()
    {
        StopAllCoroutines();
        //Dragon.transform.DOMove(DragonIdlePosition.position, 1f).SetEase(Ease.Linear).Play();
        MyDialogueManager.Instance.StopStory();
        playerCam.LookAt = player.GetComponentInChildren<Transform>();
        playerCam.Follow = player.GetComponentInChildren<Transform>();
        playerCam.enabled = true;
        finalCam.enabled = false;
        MyLevelManager.Instance.Level("Story_1", true);
    }

    private void SetDragonPosition()
    {
        dollyCartSeq1.m_Position = dollyCartSeq1.m_Path.PathLength;
    }

}
