using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public enum Player {PlayerOne,PlayerTwo};
    public Player player;

    [Header("Camera")]
    public Camera playerCamera;
    private Vector3 camOffset = new Vector3(0, 10, -3);
    private Vector3 camRotation = new Vector3(70, 0, 0);

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Knife Throw")]
    private Transform attackPoint;
    public GameObject objectToThrow;
    private float throwForce = 11f;
    private float throwUpwardForce = 2f;
    private bool readyToThrow = true;


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

    void Awake()
    {
        playerCamera.transform.position = gameObject.transform.position + camOffset;
        playerCamera.transform.eulerAngles = camRotation;
    }

    private void Start()
    {
        attackPoint = transform.Find("Attackpoint");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Movement
        transform.position += movingSpeed * Time.deltaTime * moveVec;

        //Camera
        playerCamera.transform.position = gameObject.transform.position + camOffset;

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
        Ray ray = playerCamera.ScreenPointToRay(mousePos);

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
            int x = util.interactionType;
            if (x == 1)
            {
                //hold button to progress interaction
            }
            else if (x == 2)
            {
                //press button to progress interaction
            }
            else
            {
                //trash main hand
                if (main_hand_id != 0)
                {
                    main_hand_id = off_hand_id;
                    off_hand_id = 0;
                    Debug.Log("mainhand ID: " + main_hand_id + "\noffhand ID:" + off_hand_id);
                }
                else
                {
                    Debug.Log("Nothing in Main Hand");
                }
            }
        }
    }
    
    void OnSwapInventorySlots()
    {
        if (main_hand_id == 0 || off_hand_id == 0) { return; }
        int temp_id = main_hand_id;
        main_hand_id = off_hand_id;
        off_hand_id = temp_id;

        Debug.Log("mainhand ID: " + main_hand_id + "\noffhand ID:" + off_hand_id);
    }

    void addToInventory(int id)
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

    void OnThrowKnife()
    {
        readyToThrow = false;

        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, transform.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows++;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }


    void ResetThrow()
    {
        readyToThrow = true;
    }
}
