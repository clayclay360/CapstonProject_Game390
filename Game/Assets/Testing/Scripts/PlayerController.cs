using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public enum Player {PlayerOne,PlayerTwo};
    public Player player;

    //Movement
    private float movingSpeed = 10f;
    private Vector2 moveVal;
    private Vector3 moveVec;
    private Vector2 mousePos;

    //Inventory
    //Holds item ids for held items
    private int main_hand_id = 0; //0 for empty
    private int off_hand_id = 0;
    private float interactRange = 5f;

    //quickref for whether hands are full
    private bool hands_full = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Movement
        transform.position += movingSpeed * Time.deltaTime * moveVec;

        //Rotation

    }

    public void OnMove(InputValue value)
    {
        //Debug.Log("OnMove Called!");
        moveVal = value.Get<Vector2>();
        moveVec = new Vector3(moveVal.x, 0, moveVal.y);

    }

    public void OnLook(InputValue value)
    {
        
        //OnLook is connected to the mouse's delta value
        mousePos = value.Get<Vector2>();
        //God help me
        //Uses a raycast to make the player look towards the mouse
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Vector3 lookDir = raycastHit.point;
            //transform.position = lookDir;
            lookDir.y = 0f;
            transform.LookAt(lookDir);
            //lookDir -= transform.position;
            

            //transform.rotation.eulerAngles.y = 0f;
        }
    }

    public void OnInteract()
    {
        GameObject[] interactableObjs = GameObject.FindGameObjectsWithTag("Interactable");
        GameObject closest = null;
        Interactable interactObj = null;

        //Find which of the gameobjects with interactable tag is the closest
        foreach (var obj in interactableObjs)
        {
            if ((transform.position - obj.transform.position).magnitude < interactRange)
            {
                if (closest == null)
                {
                    closest = obj;
                }
                else if ((transform.position - obj.transform.position).magnitude < (transform.position - closest.transform.position).magnitude)
                {
                    closest = obj;
                }
            }
        }
        if (closest == null) { return; }

        //Get the Interactable component from the gameobject
        if (closest.GetComponent<Interactable>() != null)
        {
            interactObj = closest.GetComponent<Interactable>();
        }

        //Now find out if it is an inventory item or a utility
        if (interactObj.TryGetComponent<InventoryItem>(out InventoryItem invItem))
        {
            addToInventory(invItem.itemID);
        }
        if (interactObj.TryGetComponent<Utility>(out Utility util))
        {
            //Do something else
            //util.Interact()
        }
    }
    
    public void OnSwapInventorySlots()
    {
        if (main_hand_id == 0 || off_hand_id == 0) { return; }
        int temp_id = main_hand_id;
        main_hand_id = off_hand_id;
        off_hand_id = temp_id;

        Debug.Log("mainhand ID: " + main_hand_id + "\noffand ID:" + off_hand_id);
    }

    private void addToInventory(int id)
    {
        if (main_hand_id == 0)
        {
            main_hand_id = id;
        }
        else if (off_hand_id == 0)
        {
            off_hand_id = id;
        }
        else { Debug.Log("Both Inventory Slots filled"); }
        Debug.Log("mainhand ID: " + main_hand_id + "\noffand ID:" + off_hand_id);
    }
}
