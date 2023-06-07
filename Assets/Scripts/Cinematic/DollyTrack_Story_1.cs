using System.Collections;
using Cinemachine;
using Managers;
using Player;
using UnityEngine;

public class DollyTrack_Story_1 : MonoBehaviour
{
    #region VARIABLES
    public CinemachineDollyCart dollyCartSeq1;
    public CinemachineDollyCart dollyCartSeq2;
    public CinemachineVirtualCamera finalCam;
    public CinemachineFreeLook leoCam;
    public BoyController boyControllerScript;
    public LoadScreenManager _LoadScreenManager;

    [Header("Characters")]
    public GameObject Leo;
    public Animator _anim;
    public GameObject Dragon;

    private bool btnPresed = false;
    #endregion

    void Start()
    {
        if (dollyCartSeq1.m_Path != null)
            dollyCartSeq1.m_Speed = 12;
        
        if (dollyCartSeq2.m_Path != null)
            dollyCartSeq2.m_Speed = 0;
        
    
        _anim.SetBool("walk", false);
        StartCoroutine(DollyCart());
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
        MyDialogueManager.Instance.StopStory();
        leoCam.enabled = true;
        finalCam.enabled = false;
        boyControllerScript.enabled = true;
    }

    #region SALTAR ESCENA
    public void LoadScene()
    {
        if (!btnPresed)
        {
            btnPresed = true;
            MyDialogueManager.Instance.HideDialogBox();
            _LoadScreenManager.LoadScene();
        }
    }
    #endregion
}
