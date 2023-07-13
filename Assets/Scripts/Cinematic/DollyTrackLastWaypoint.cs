    using System;
    using System.Collections;
    using Managers;
    using Service;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class DollyTrackLastWaypoint : MonoBehaviour
    {
        #region VARIABLES
        public GameObject RainBow;
        private bool _btnPressed;
        #endregion
        
        #region UNITY METHODS

        private void OnEnable()
        {
            ServiceLocator.GetService<MyInputManager>().anyAction.performed += EndsCinematic;
        }

        private void OnDisable()
        {
            ServiceLocator.GetService<MyInputManager>().anyAction.performed -= EndsCinematic;
        }

        void Start()
        {
            StartCoroutine(DrawRainbow());
        }

        private void EndsCinematic(InputAction.CallbackContext obj)
        {
            ServiceLocator.GetService<LoadScreenManager>().LoadScene("Menu_game");
        }

        #endregion
        

        private IEnumerator DrawRainbow()
        {
            yield return new WaitForSeconds(2f);
            RainBow.SetActive(true);
        }
    }