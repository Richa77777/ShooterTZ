using UnityEngine;

public class InteractableGun : InteractableObject
{
    [Header("Gun Settings")]
    [SerializeField] private int _gunID;
    
    public
        override void Interact(GameObject obj)
    {
        EventsHandler.Instance.OnGunPickedUp?.Invoke(_gunID);
        Debug.Log($"Gun with ID {_gunID} picked up!");
        base.Interact(obj);
    }
}