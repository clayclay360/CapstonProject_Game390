using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public enum Player {PlayerOne,PlayerTwo};
    public Player player;

    [Header("Movement")]
    public float movingSpeed;
    public float rotatingSpeed;

    [Header("Camera")]
    public Camera playerCamera;
    private Vector3 camOffset = new Vector3(0, 10, -3);
    private Vector3 camRotation = new Vector3(70, 0, 0);

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Knife Throw")]
    public GameObject objectToThrow;
    private Transform attackPoint;
    private float throwForce = 11f;
    private float throwUpwardForce = 2f;
    private bool readyToThrow = true;

    //Animator
    [Header("Animator")]
    public Animator animator;

    //Movement
    private Vector3 moveVec;
    //Rotation
    private Vector3 rotateVec;

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
        animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Movement
        if(Mathf.Abs(moveVec.x) > 0 || Mathf.Abs(moveVec.z) > 0)
        {
            if(moveVec.magnitude > .1f)
            {
                transform.position += movingSpeed * Time.deltaTime * moveVec;
                animator.SetBool("Run",true);
            }
        }
        else
        {
            animator.SetBool("Run",false);
        }
        //Rotation
        if(Mathf.Abs(rotateVec.x ) > 0 || Mathf.Abs(rotateVec.z) > 0)
        {
            Vector3 rotateDirection = (Vector3.right * rotateVec.x) + (Vector3.forward * rotateVec.z);
            if(rotateDirection.sqrMagnitude > 0)
            {
                Quaternion newRotation = Quaternion.LookRotation(rotateDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotatingSpeed);
            }
        }

        //Camera
        playerCamera.transform.position = gameObject.transform.position + camOffset;

    }

    public void OnMove(InputValue value)
    {
        Vector2 moveVal = value.Get<Vector2>();
        moveVec = new Vector3(moveVal.x, 0, moveVal.y);
    }

    public void OnLook(InputValue value)
    {
        Vector2 rotateVal = value.Get<Vector2>();
        rotateVec = new Vector3(rotateVal.x,0,rotateVal.y);
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
            if (x == 1 && util.itemNeed == main_hand_id)
            {
                Debug.Log("making progress");
                if (util.makeProgress(2))
                {
                    main_hand_id = util.itemGive;
                    Debug.Log("progress complete");
                    Debug.Log("mainhand ID: " + main_hand_id + "\noffand ID:" + off_hand_id);
                }
            }
            else if (x == 2 && util.itemNeed == main_hand_id)
            {
                Debug.Log("making progress");
                if (util.makeProgress(2))
                {
                    main_hand_id = util.itemGive;
                    Debug.Log("progress complete");
                    Debug.Log("mainhand ID: " + main_hand_id + "\noffand ID:" + off_hand_id);
                }
            }
            else if (x == 3)
            {
                main_hand_id = off_hand_id;
                off_hand_id = 0;
                Debug.Log("trashed mainhand item");
                Debug.Log("mainhand ID: " + main_hand_id + "\noffand ID:" + off_hand_id);
            }
        }
    }
    
    void OnSwapInventorySlots()
    {
        if (main_hand_id == 0 || off_hand_id == 0) { return; }
        int temp_id = main_hand_id;
        main_hand_id = off_hand_id;
        off_hand_id = temp_id;

        Debug.Log("mainhand ID: " + main_hand_id + "\noffand ID:" + off_hand_id);
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
