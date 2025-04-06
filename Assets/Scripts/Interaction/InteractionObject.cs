using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public virtual void Interact(GameObject obj)
    {
        Debug.Log($"Interacting with {obj.name}");
        obj.SetActive(false);
    }
}