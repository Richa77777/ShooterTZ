using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float _interactionRange = 3f;
    [SerializeField] private TextMeshProUGUI _interactionText;
    [SerializeField] private Transform _rayOrigin;

    private void Update()
    {
        CheckForInteractableObject();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void CheckForInteractableObject()
    {
        RaycastHit hit;
        Ray ray = new Ray(_rayOrigin.position, _rayOrigin.forward);
        
        if (Physics.Raycast(ray, out hit, _interactionRange))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                _interactionText.text = "Press E to interact with " + interactable.gameObject.name;
                _interactionText.enabled = true;
            }
            else
            {
                _interactionText.enabled = false;
            }
        }
        else
        {
            _interactionText.enabled = false;
        }
    }

    private void TryInteract()
    {
        RaycastHit hit;
        Ray ray = new Ray(_rayOrigin.position, _rayOrigin.forward);

        if (Physics.Raycast(ray, out hit, _interactionRange))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                interactable.Interact(hit.collider.gameObject);
            }
        }
    }
}