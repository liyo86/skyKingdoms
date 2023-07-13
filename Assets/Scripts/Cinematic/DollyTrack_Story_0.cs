using System;
using System.Collections;
using Cinemachine;
using Managers;
using Service;
using UnityEngine;
using UnityEngine.InputSystem;

public class DollyTrack_Story_0 : MonoBehaviour
{
    #region VARIABLES
    public CinemachineDollyCart dollyCartSeq1;
    public CinemachineDollyCart dollyCartSeq2;
    public CinemachineVirtualCamera finalCam;
    public GameObject BlackCanvas;

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

    #endregion

    private void OnEnable()
    {
        ServiceLocator.GetService<MyInputManager>().submitAction.performed += LoadScene;
    }

    private void OnDisable()
    {
        ServiceLocator.GetService<MyInputManager>().submitAction.performed -= LoadScene;
    }

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

        ServiceLocator.GetService<MyDialogueManager>().Init(); // LLego a la mitad de la sala

        yield return new WaitForSeconds(3);
        dollyCartSeq1.m_Speed = 1;
        Goblin.DoWalk();
        
        yield return new WaitForSeconds(2);
        dollyCartSeq1.m_Speed = 0;
        Goblin.DoStop();
        
        ServiceLocator.GetService<MyDialogueManager>().NextText(); // ALTO
        Princess.SetActive(true);
        dollyCartSeq2.m_Speed = 2;
        yield return new WaitUntil(() =>  ServiceLocator.GetService<MyDialogueManager>().CanContinue());
        yield return new WaitForSeconds(1);
        
        Goblin.DoRotate();
        yield return new WaitForSeconds(1);
        
        finalCam.enabled = true;
        ServiceLocator.GetService<MyDialogueManager>().NextText(); // Sera mejor
        yield return new WaitUntil(() =>  ServiceLocator.GetService<MyDialogueManager>().CanContinue());
        yield return new WaitForSeconds(1);
        
        ServiceLocator.GetService<MyDialogueManager>().NextText();
        yield return new WaitUntil(() =>         ServiceLocator.GetService<MyDialogueManager>().CanContinue());
        yield return new WaitForSeconds(1);
        
        BlackCanvas.SetActive(true);
        ServiceLocator.GetService<MyDialogueManager>().NextText();
        yield return new WaitUntil(() => ServiceLocator.GetService<MyDialogueManager>().CanContinue());
        yield return new WaitForSeconds(1);
        
        ServiceLocator.GetService<MyDialogueManager>().NextText(); // Ultimo dialogo
        yield return new WaitUntil(() =>  ServiceLocator.GetService<MyDialogueManager>().CanContinue());
        yield return new WaitForSeconds(1);
        
        ServiceLocator.GetService<MyDialogueManager>().HideDialogBox();
        yield return new WaitForSeconds(1);
        ServiceLocator.GetService<LoadScreenManager>().LoadScene("Story_1");
    }

    private void LoadScene(InputAction.CallbackContext context)
    {
        ServiceLocator.GetService<MyDialogueManager>().HideDialogBox();
        ServiceLocator.GetService<LoadScreenManager>().LoadScene("Story_1");
    }
    
}
