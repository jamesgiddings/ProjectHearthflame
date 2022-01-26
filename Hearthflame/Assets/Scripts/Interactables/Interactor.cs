using UnityEngine;

namespace GramophoneUtils.Interactables
{
    public class Interactor : MonoBehaviour
    {
        private IInteractable currentInteractable = null;

        private void Update()
        {
            CheckForInteraction();
        }

        private void CheckForInteraction()
        {

            if (currentInteractable == null) { return; }
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentInteractable.Interact(transform.root.gameObject);
                currentInteractable = null; //test
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var interactable = other.GetComponent<IInteractable>();

            if (interactable == null) { return; }

            currentInteractable = interactable;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var interactable = other.GetComponent<IInteractable>();

            if (interactable == null) { return; }

            if (interactable != currentInteractable) { return; }

            currentInteractable = null;
        }
    }
}
