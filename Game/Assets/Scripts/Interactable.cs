using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable<I, T>
{
    public bool Interact(T chef);
    public void CheckHand(I index, T chef);
}

public class Interactable : MonoBehaviour
{
    public enum testenum {inv, util};
    //public GameObject myHands; //reference to your hands/the position where you want your object to go
    bool interactionEnabled; //a bool to see if you can or cant pick up the item
    public float Interactionrange = 5f;
    // Start is called before the first frame update


    public virtual int Interact()
    {
        return 0;
    }
}

