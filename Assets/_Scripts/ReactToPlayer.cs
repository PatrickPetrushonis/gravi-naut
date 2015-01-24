using UnityEngine;
using System.Collections;

public class ReactToPlayer : MonoBehaviour 
{
    public bool playerTrigger = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
            playerTrigger = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
            playerTrigger = false;
    }
}
