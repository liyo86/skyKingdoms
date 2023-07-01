    using System.Collections;
    using UnityEngine;
    using Cinemachine;

    public class DollyTrackLastWaypoint : MonoBehaviour
    {
        #region VARIABLES
        public GameObject RainBow;
        public LoadScreenManager _LoadScreenManager;
        private bool _btnPressed;
        #endregion
        
        #region UNITY METHODS
  
        void Start()
        {
            StartCoroutine(DrawRainbow());
        }
        #endregion

        #region CHARACTER SEQUENCES

        private IEnumerator DrawRainbow()
        {
            yield return new WaitForSeconds(2f);
            RainBow.SetActive(true);
        }
        #endregion
        
        #region UI
        public void EndsCinematic()
        {
            if (!_btnPressed)
            {
                _btnPressed = true;
                _LoadScreenManager.LoadScene();
            }
        }
        #endregion
    }