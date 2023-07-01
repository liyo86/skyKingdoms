using UnityEngine;

namespace Interactable
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        public GameObject IconInteract;
        
        public void Interact()
        {
       
        }

        public void ShowCanInteract(bool show)
        {
            IconInteract.SetActive(show);
        }
    }
}
