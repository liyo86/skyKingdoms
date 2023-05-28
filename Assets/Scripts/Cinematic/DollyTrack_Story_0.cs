using System.Collections;
using Cinemachine;
using Managers;
using UnityEngine;

public class DollyTrack_Story_0 : MonoBehaviour
{
    #region VARIABLES
    public CinemachineDollyCart dollyCartSeq1;
    public CinemachineDollyCart dollyCartSeq2;
    public CinemachineVirtualCamera finalCam;
    public GameObject BlackCanvas;
    
    public LoadScreenManager _LoadScreenManager;

    [Header("Characters")]
    public GameObject Princess;

    public DOTGoblinWalk Goblin;
    #endregion

    #region References

    private float pathLength1;
    private float pathLength2;

    #endregion

    #region Sequence Steps

    private bool step_1;
    private bool step_2;
    private bool step_3;
    private bool step_4;
    private bool btnPresed;

    #endregion

    void Start()
    {
        BlackCanvas.SetActive(false);
        
        if (dollyCartSeq1.m_Path != null) // Dolly Track del Goblin
        {
            pathLength1 = dollyCartSeq1.m_Path.PathLength;
            dollyCartSeq1.m_Speed = 1;
            Goblin.DoWalk();
        }
        if (dollyCartSeq2.m_Path != null) // Dolly Track de la princesa
        {
            finalCam.enabled = false;
            pathLength2 = dollyCartSeq2.m_Path.PathLength;
        }
        
        StartCoroutine(DollyCart());
    }
    
    private IEnumerator DollyCart()
    {
        dollyCartSeq1.m_Speed = 1;
        yield return new WaitForSeconds(5); //Camino al centro de la sala
        dollyCartSeq1.m_Speed = 0;
        Goblin.DoStop();
        MyDialogueManager.Instance.Init(); // LLego a la mitad de la sala

        yield return new WaitForSeconds(3);
        dollyCartSeq1.m_Speed = 1;
        Goblin.DoWalk();
        
        yield return new WaitForSeconds(2);
        dollyCartSeq1.m_Speed = 0;
        Goblin.DoStop();
        
        MyDialogueManager.Instance.NextText(); // ALTO
        Princess.SetActive(true);
        dollyCartSeq2.m_Speed = 2;
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        Goblin.DoRotate();
        yield return new WaitForSeconds(1);
        
        finalCam.enabled = true;
        MyDialogueManager.Instance.NextText(); // Sera mejor
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        BlackCanvas.SetActive(true);
        MyDialogueManager.Instance.NextText();
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.NextText(); // Ultimo dialogo
        yield return new WaitUntil(() => MyDialogueManager.Instance.CanContinue());
        yield return new WaitForSeconds(1);
        
        MyDialogueManager.Instance.HideDialogBox();
        yield return new WaitForSeconds(1);
        _LoadScreenManager.LoadScene();
    }

    public void LoadScene()
    {
        if (!btnPresed)
        {
            btnPresed = true;
            MyDialogueManager.Instance.HideDialogBox();
            _LoadScreenManager.LoadScene();
        }
    }
    
}
