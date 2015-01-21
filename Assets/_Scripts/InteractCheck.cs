using UnityEngine;
using System.Collections;

public class InteractCheck : MonoBehaviour 
{
    public GameObject interactiveObject;            //interactive object colliding with trigger
    public LayerMask interactIdentity;              //which layers are considered interactive
    public bool canInteract = false;                //whether an interactive object is in reach

    void OnTriggerEnter2D(Collider2D other)
    {
        if(((1 << other.gameObject.layer) & interactIdentity) != 0)
        {
            canInteract = true;
            interactiveObject = other.gameObject;
        }
            
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(((1 << other.gameObject.layer) & interactIdentity) != 0)
        {
            canInteract = false;
            interactiveObject = null;
        }
            
    }
}
