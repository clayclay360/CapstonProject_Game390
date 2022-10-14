using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    private GameManager gm;

    [SerializeField] private LayerMask groundMask;
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
    private Vector3 rotateVec;

    //Inventory
    //Holds item ids for held items
    private int main_hand_id = 0; //0 for empty
    private int off_hand_id = 0;
    private float interactRange = 5f;
    [Header("Inventory")]
    public Text Inv1;
    public Text Inv2;

    //Interactions
    [Header("Interactions")]
    public Text interactionText;
    public Dictionary<int, Item> hand = new Dictionary<int, Item>();
    public enum ItemInMainHand { empty, egg, spatula, pan };
    public ItemInMainHand itemInMainHand;
    RecipeBook cookBook;

    //quickref for whether hands are full
    public bool inventoryFull = false;

    [HideInInspector]
    public bool isInteracting;
    private bool readyToInteract;

    void Awake()
    {
        playerCamera.transform.position = gameObject.transform.position + camOffset;
        playerCamera.transform.eulerAngles = camRotation;
    }

    private void Start()
    {
        attackPoint = transform.Find("Attackpoint");
        animator.GetComponent<Animator>();
        PlayerAssignment();
        gm = GameManager.Instance;
    }

    public void Update()
    {
        //Interact();
        cookBook = GameObject.Find("DetectCollision").GetComponent<RecipeBook>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMovement();
        CheckInventory();
        GetNameInMain();
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

    public void PlayerMovement() 
    {
        animator.SetFloat("Blend", moveVec.magnitude);

        //Movement
        if (Mathf.Abs(moveVec.x) > 0 || Mathf.Abs(moveVec.z) > 0)
        {
            if (moveVec.magnitude > .1f)
            {
                transform.position += movingSpeed * Time.deltaTime * moveVec;
            }
        }
        //Rotation
        if (Mathf.Abs(rotateVec.x) > 0 || Mathf.Abs(rotateVec.z) > 0)
        {
            Vector3 rotateDirection = (Vector3.right * rotateVec.x) + (Vector3.forward * rotateVec.z);
            if (rotateDirection.sqrMagnitude > 0)
            {
                Quaternion newRotation = Quaternion.LookRotation(rotateDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotatingSpeed);
            }
        }

        //Camera
        playerCamera.transform.position = gameObject.transform.position + camOffset;
    }

    public void PlayerAssignment()
    {
        if(GameManager.numberOfPlayers == 1)
        {
            player = Player.PlayerOne;
            transform.position = new Vector3(-5, 0, 0);
        }
        else
        {
            player = Player.PlayerTwo;
            transform.position = new Vector3(1, 0, 0);
        }

        hand[0] = null;
        hand[1] = null;
        hand[2] = null;
        itemInMainHand = ItemInMainHand.empty;

        interactionText.GetComponent<Text>();
    }

    private void GetNameInMain()
    {
        if (hand[0] != null)
        {
            switch (hand[0].Name)
            {
                case "Egg":
                    itemInMainHand = ItemInMainHand.egg;
                    break;
                case "Spatula":
                    itemInMainHand = ItemInMainHand.spatula;
                    break;
                case "Pan":
                    itemInMainHand = ItemInMainHand.pan;
                    break;
                default:
                    itemInMainHand = ItemInMainHand.empty;
                    break;
            }
        }
        else
        {
            itemInMainHand = ItemInMainHand.empty;
        }
    }

    private void OnSwapInventorySlots()
    {
        hand[2] = hand[0];
        hand[0] = hand[1];
        hand[1] = hand[2];

        if (hand[0] != null)
        {
            Inv1.text = hand[0].Name;
        }
        else
        {
            Inv1.text = "";
        }


        if (hand[1] != null)
        {
            Inv2.text = hand[1].Name;
        }
        else
        {
            Inv2.text = "";
        }

        Debug.LogWarning("Switching Hands: \nHand 1: " + hand[0] + "\nHand 2: " + hand[1]);
    }

    private void CheckInventory()
    {
        if (hand[0] != null && hand[1] != null)
        {
            inventoryFull = true;
        }
        else
        {
            inventoryFull = false;
        }
    }

    public void OnInteract()
    {
        if (readyToInteract)
        {
            isInteracting= true;
        }
        //Debug.LogWarning("OnInteract()\nisInteracting: " + isInteracting.ToString() + "\nreadyToInteract: " + readyToInteract.ToString());
        cookBook.ClickOnBook();
    }

    public IEnumerable InteractCD()
    {
        readyToInteract = false;
        yield return new WaitForSeconds(.5f);
        readyToInteract = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        { 
            readyToInteract = true; //assign ready to interact so that isinteracting can be set from OnInteract()

            if (other.gameObject.GetComponent<Item>() != null)
            {
                other.gameObject.GetComponent<Item>().CheckHand(itemInMainHand, this);
                interactionText.text = other.gameObject.GetComponent<Item>().Interaction;

                if (isInteracting && !other.gameObject.GetComponent<Item>().Prone && /* This is temporary just for prototype*/ other.gameObject.GetComponent<Item>().Name != "Plate") //check isinteracting on the item
                {
                    isInteracting = false; //turn off isinteracting HERE to prevent problems
                    if (hand[0] == null)
                    {
                        hand[0] = other.gameObject.GetComponent<Item>();
                        Inv1.text = hand[0].Name;
                        Debug.Log(hand[0].Name);
                    }
                    else if (hand[1] == null)
                    {
                        hand[1] = hand[0];
                        hand[0] = other.gameObject.GetComponent<Item>();
                        Inv1.text = hand[0].Name;
                        Inv2.text = hand[1].Name;
                        Debug.Log(hand[0].Name);
                    }
                }
                else
                {
                    other.gameObject.GetComponent<Item>().Prone = false;
                    isInteracting = false;
                }
            }
            else if (other.gameObject.GetComponent<Utility>() != null)
            {
                other.gameObject.GetComponent<Utility>().CheckHand(itemInMainHand, this);
                interactionText.text = other.gameObject.GetComponent<Utility>().Interaction;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactionText.text = "";
        readyToInteract = false;
        isInteracting = false;
        if (other.GetComponent<Item>() != null || other.GetComponent<Utility>() != null || other.gameObject.tag == "Interactable")
        {
            //Depreciated
        }
        //Debug.LogWarning("OnTriggerExit()\nisInteracting: " + isInteracting.ToString() + "\nreadyToInteract: " + readyToInteract.ToString());

        if (other.gameObject.tag == "CookBook")
        {
            GameManager.isTouchingBook = false;
            cookBook = null;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CookBook")
        {
            GameManager.isTouchingBook = true;
            cookBook = GameObject.Find("DetectCollision").GetComponent<RecipeBook>();
        }
    }

    //public void OnInteract()
    //{
    //    GameObject[] interactableObjs = GameObject.FindGameObjectsWithTag("Interactable");
    //    GameObject closest = null;
    //    Interactable interactObj = null;

    //    //Find which of the gameobjects with interactable tag is the closest
    //    foreach (var obj in interactableObjs)
    //    {
    //        if ((transform.position - obj.transform.position).magnitude < interactRange)
    //        {
    //            if (closest == null)
    //            {
    //                closest = obj;
    //            }
    //            else if ((transform.position - obj.transform.position).magnitude < (transform.position - closest.transform.position).magnitude)
    //            {
    //                closest = obj;
    //            }
    //        }
    //    }
    //    if (closest == null) { return; }

    //    //Get the Interactable component from the gameobject
    //    if (closest.GetComponent<Interactable>() != null)
    //    {
    //        interactObj = closest.GetComponent<Interactable>();
    //    }

    //    //Now find out if it is an inventory item or a utility
    //    if (interactObj.TryGetComponent<InventoryItem>(out InventoryItem invItem))
    //    {
    //        addToInventory(invItem.itemID);
    //    }
    //    if (interactObj.TryGetComponent<Utility>(out Utility util))
    //    {
    //        int x = util.interactionType;
    //        if (x == 1 && util.itemNeed == main_hand_id)
    //        {
    //            Debug.Log("making progress");
    //            if (util.makeProgress(2))
    //            {
    //                main_hand_id = util.itemGive;
    //                Debug.Log("progress complete");
    //                Debug.Log("mainhand ID: " + main_hand_id + "\noffand ID:" + off_hand_id);
    //            }
    //        }
    //        else if (x == 2 && util.itemNeed == main_hand_id)
    //        {
    //            Debug.Log("making progress");
    //            if (util.makeProgress(2))
    //            {
    //                main_hand_id = util.itemGive;
    //                Debug.Log("progress complete");
    //                Debug.Log("mainhand ID: " + main_hand_id + "\noffand ID:" + off_hand_id);
    //            }
    //        }
    //        else if (x == 3)
    //        {
    //            main_hand_id = off_hand_id;
    //            off_hand_id = 0;
    //            Debug.Log("trashed mainhand item");
    //            Debug.Log("mainhand ID: " + main_hand_id + "\noffand ID:" + off_hand_id);
    //        }
    //    }
    //}

    //void OnSwapInventorySlots()
    //{
    //    if (main_hand_id == 0 || off_hand_id == 0) { return; }
    //    int temp_id = main_hand_id;
    //    main_hand_id = off_hand_id;
    //    off_hand_id = temp_id;

    //    Debug.Log("mainhand ID: " + main_hand_id + "\noffand ID:" + off_hand_id);
    //}

    //void addToInventory(int id)
    //{
    //    if (main_hand_id == 0)
    //    {
    //        main_hand_id = id;
    //    }
    //    else if (off_hand_id == 0)
    //    {
    //        off_hand_id = id;
    //    }
    //    else { Debug.Log("Both Inventory Slots filled"); }
    //    Debug.Log("mainhand ID: " + main_hand_id + "\noffand ID:" + off_hand_id);
    //}

    void OnThrowKnife()
    {
        if (GameManager.recipeIsOpenP1)
        {
            cookBook.ClickThroughBook();
        } else
        {
            readyToThrow = false;

            // instantiate object to throw
            GameObject projectile = Instantiate(objectToThrow, attackPoint.position, transform.rotation);

            // get rigidbody component
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            // calculate direction
            KnifeAddon kscript = projectile.GetComponent<KnifeAddon>();

            kscript.forward = transform.forward;

            //OLD
            //RaycastHit hit;

            //if (Physics.Raycast(transform.position, transform.forward, out hit, 500f))
            //{
            //    forceDirection = (hit.point - attackPoint.position).normalized;
            //}

            //// add force
            //Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

            //projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

            //totalThrows++;
            //END OLD

            // implement throwCooldown
            Invoke(nameof(ResetThrow), throwCooldown);
        }
    }

    void ResetThrow()
    {
        readyToThrow = true;
    }
}
