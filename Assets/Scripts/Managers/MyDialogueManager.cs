using System;
using System.Reflection;
using Doublsb.Dialog;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class MyDialogueManager : MonoBehaviour
    {
        public static MyDialogueManager Instance;

        [Tooltip("Activar si el player tiene el control de la escena.")]
        public bool PlayerControl;
        public int Step => step;

        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private GameObject selectorManager;
        [SerializeField] private GameObject nextBtn;
        [SerializeField] private Text characterText;

        private DOTDialogAnimator dialogAnimator;

        private MethodInfo storyMethod;
        private MethodInfo storyMaxStepMethod;
        private string currentText;
        private int step = 0;
        private int maxStep = 0;
        private string currentScene;
        private bool isSubmitBtn;
        private bool canStart;
        private bool canCheckVisibility;
        private const string TEXT_STORY = "Text_Story";
        private const string TEXT = "Text";
        private const string MAX_STEPS = "GetMaxStep";

        void LateUpdate()
        {
            if (canStart && PlayerControl)
            {
                CheckShowButton();   
            } else if (canStart && !PlayerControl)
            {
                //CheckDialogVisibility();
            }
        }

        public void Init()
        {
            currentScene = GetActiveStoryScene();
            Type storyType = Type.GetType(currentScene);
            if (storyType != null)
            {
                storyMethod = storyType.GetMethod(TEXT);
                storyMaxStepMethod = storyType.GetMethod(MAX_STEPS);

                if (storyMaxStepMethod != null)
                {
                    maxStep = (int)storyMaxStepMethod.Invoke(null, null);
                }
                
                
                if (storyMethod != null)
                {
                    dialogAnimator.ShowDialogBox();
                    canStart = true;
                    NewDialogText();
                }
            }
        }
        
        // Recojo la escena actual
        string GetActiveStoryScene()
        {
            string scene = SceneManager.GetActiveScene().name;
            return TEXT_STORY + scene.Substring(scene.IndexOf("_"));
        }

        // Muestro nuevo texto
        private void NewDialogText()
        {
            if (step < maxStep)
            {
                step++;

                currentText = (string)storyMethod.Invoke(null, new object[] { step });

                int asteriskIndex = currentText.IndexOf("*");
                
                string characterName = currentText.Substring(0, asteriskIndex);

                DialogData dialogData = new DialogData(currentText.Substring(asteriskIndex + 1));

                characterText.text = characterName;

                canCheckVisibility = true;

                dialogAnimator.ShowDialogBox();
                
                dialogManager.Show(dialogData);
            }
            else
            {
                StoryEnds();
            }
        }
        
        // Dialogos con opciones
        public void NewOptionText(string text, string character)
        {
            currentText = text;

            string characterName = character;

            DialogData dialogData = new DialogData(currentText);

            characterText.text = characterName;

            canCheckVisibility = true;
            
            dialogAnimator.ShowDialogBox();

            dialogManager.Show(dialogData);
        }
        
        // Para cuando el Player controla el botón
        void CheckShowButton()
        {
            var isActivated = CanContinue();
            nextBtn.SetActive(isActivated);   
            isSubmitBtn = !isActivated;
        }

        // Para cuando la cinemática controla los cambios de texto

        public bool CanContinue()
        {
            int asteriskIndex = currentText.IndexOf("*");
            string text = currentText.Substring(asteriskIndex + 1);

            return dialogManager.Printer_Text.text.Length >= text.Length;
        }
        
        // No hay más texto que mostrar
        void StoryEnds()
        {
            dialogAnimator.HideDialogBox();
            MyGameManager.ResumePlayerMovement();
        }
        
        // Player Input Action Submit
        public void OnBtnSubmit()
        {
            if (PlayerControl)
            {
                if (nextBtn.activeSelf && !isSubmitBtn)
                {
                    isSubmitBtn = true;
                    NewDialogText();   
                }   
            }
        }
        
        // Siguiente texto controlado por la cinemática
        public void NextText()
        {
            if (!PlayerControl)
            {
                dialogAnimator.ShowDialogBox();
                NewDialogText();   
            }
        }
        
        // Carga de textos en Niveles
        public void TextLevel(string level)
        {
            step = 0;
            
            Type storyType = Type.GetType(TEXT + "_" + level);
 
            if (storyType != null)
            {
                storyMethod = storyType.GetMethod(TEXT);
                storyMaxStepMethod = storyType.GetMethod(MAX_STEPS);

                if (storyMaxStepMethod != null)
                {
                    maxStep = (int)storyMaxStepMethod.Invoke(null, null);
                }
                
                
                if (storyMethod != null)
                {
                    dialogAnimator.ShowDialogBox();
                    canStart = true;
                    NewDialogText();
                }
            }
        }
        
        public void HideDialogBox()
        {
            dialogAnimator.HideDialogBox();
        }

        public void StopStory()
        {
            step = maxStep;
            StoryEnds();
        }
    }
}
