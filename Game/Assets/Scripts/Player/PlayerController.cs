using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using UnityEngine.AI;

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
    private bool isAiming = false;

    //Animator
    [Header("Animator")]
    public Animator animator;

    [Header("Task Manager")]
    public TaskManager taskManager;

    //Movement
    private Vector3 moveVec;
    private Vector3 rotateVec;
    private float rightAnalogMagnitude;

    //Inventory
    //Holds item ids for held items
    private int main_hand_id = 0; //0 for empty
    private int off_hand_id = 0;
    private float interactRange = 5f;
    [Header("Inventory")]
    public Text Inv1;
    public Text Inv2;

    [Header("Prefabs")]
    public GameObject pan;
    public GameObject spatula;
    public GameObject bacon;
    public GameObject egg;
    public GameObject pages;
    private GameObject passItems;

    [Header("Pass Items Scripts")]
    Pan passPan;
    Bacon passBacon;
    Egg passEgg;
    Spatula passSpatula;
    CookBookPages passPages;

    [Header("Counter Top")]
    public GameObject counterTop;
    CounterTop counterTopScript;

    //Interactions
    [Header("Interactions")]
    public Text interactionText;
    public Dictionary<int, Item> hand = new Dictionary<int, Item>();
    public enum ItemInMainHand { empty, egg, spatula, pan, bacon, pages, plate };
    public ItemInMainHand itemInMainHand;
    private Color outlineColor;
    RecipeBook cookBook;

    [Header("Orders")]
    public GameObject orderPrefab;
    public GameObject orderLayoutGroup;

    //quickref for whether hands are full
    public bool inventoryFull = false;

    [HideInInspector]
    public bool isInteracting;
    private bool readyToInteract;

    //comment

    //player count to stop error with trying to spawn in more players
    

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
        ColorAssignment();
        throwCooldown = 0.4f;

        GameManager.isTouchingTrashCan = false;
        GameManager.passItemsReady = false;
        GameManager.putOnCounter = false;

        passPan = GameObject.Find("Pan").GetComponentInChildren<Pan>();
        passBacon = GameObject.Find("Bacon").GetComponentInChildren<Bacon>();
        passEgg = GameObject.Find("Egg").GetComponentInChildren<Egg>();
        passSpatula = GameObject.Find("Spatula").GetComponentInChildren<Spatula>();
        passPages = GameObject.Find("Pages").GetComponentInChildren<CookBookPages>();
    }

    public void Update()
    {
        //Interact();
        cookBook = GameObject.Find("CookBook").GetComponentInChildren<RecipeBook>();
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
        
        if (rightAnalogMagnitude < 0.1f)
        {
            Vector2 rotateVal = value.Get<Vector2>();
            rotateVec = new Vector3(rotateVal.x, 0, rotateVal.y);
        }
    }

    public void OnLook(InputValue value)
    {
        Vector2 rotateVal = value.Get<Vector2>();
        rotateVec = new Vector3(rotateVal.x,0,rotateVal.y);
        rightAnalogMagnitude = rotateVec.magnitude;
        
    }

    public void PlayerMovement() 
    {
        animator.SetFloat("BlendX", moveVec.magnitude);

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
            if (rotateDirection.sqrMagnitude > .1f)
            {
                Quaternion newRotation = Quaternion.LookRotation(rotateDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotatingSpeed);
                float angle = Mathf.Atan2(rotateVec.x, rotateVec.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
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
            GameManager.playerOne = this;
            transform.position = new Vector3(-5f, 0.125f, 0f);
        }
        else
        {
            player = Player.PlayerTwo;
            GameManager.playerTwo = this;
            transform.position = new Vector3(5f, 0.125f, 0f);
        }

        hand[0] = null;
        hand[1] = null;
        hand[2] = null;
        itemInMainHand = ItemInMainHand.empty;

        Invoke("NavEnable", 0.25f);
        interactionText.GetComponent<Text>();
    }

    public void NavEnable()
    {
        //enable nav mesh
        GetComponent<NavMeshAgent>().enabled = true;
    }

    private void ColorAssignment()
    {
        switch (player)
        {
            case Player.PlayerOne:
                outlineColor = Color.blue;
                break;
            case Player.PlayerTwo:
                outlineColor = Color.green;
                break;
        }
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
                case "Bacon":
                    itemInMainHand = ItemInMainHand.bacon;
                    break;
                case "Cookbook Pages":
                    itemInMainHand = ItemInMainHand.pages;
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
    }

    public void OnInteract()
    {
        if (readyToInteract)
        {
            isInteracting= true;
        }

        if (hand[0] != null && !GameManager.passItemsReady && !GameManager.putOnCounter && !readyToInteract)
        {
            switch (hand[0].Name)
            {
                case "Egg":
                    passEgg.DropEggOnGround(gameObject);
                    hand[0] = null;
                    break;
                case "Spatula":
                    passSpatula.DropSpatulaOnGround(gameObject);
                    hand[0] = null;
                    break;
                case "Pan":
                    passPan.DropPanOnGround(gameObject);
                    hand[0] = null;
                    break;
                case "Bacon":
                    passBacon.DropBaconOnGround(gameObject);
                    hand[0] = null;
                    break;
                case "Cookbook Pages":
                    passPages.DropPagesOnGround(gameObject);
                    hand[0] = null;
                    break;
                default:
                    itemInMainHand = ItemInMainHand.empty;
                    break;
            }
        }
        if (hand[0] != null && GameManager.passItemsReady && !readyToInteract)
        {
            passItems = GameObject.Find("PassItems");

            switch (hand[0].Name)
            {
                case "Egg":
                    if (GameManager.counterItems.Contains(egg.name))
                    {
                        Debug.Log("Contains Egg");
                    }
                    else
                    {
                        passEgg.PassEgg(AddItemsToCounter(egg.name));
                    }
                    hand[0] = null;
                    break;
                case "Spatula":
                    if (GameManager.counterItems.Contains(spatula.name))
                    {
                        Debug.Log("Contains Spatula");
                    } else
                    {
                        passSpatula.PassSpatula(AddItemsToCounter(spatula.name));
                    }
                    hand[0] = null;
                    break;
                case "Pan":
                    if (GameManager.counterItems.Contains(pan.name))
                    {
                        Debug.Log("Contains Pan");
                    }
                    else
                    {
                        passPan.PassPan(AddItemsToCounter(pan.name));
                    }
                    hand[0] = null;
                    break;
                case "Bacon":
                    if (GameManager.counterItems.Contains(bacon.name))
                    {
                        Debug.Log("Contains Bacon");
                    }
                    else
                    {
                        passBacon.PassBacon(AddItemsToCounter(bacon.name));
                    }
                    hand[0] = null;
                    break;
                case "Cookbook Pages":
                    if (GameManager.counterItems.Contains(pages.name))
                    {
                        Debug.Log("Contains Pages");
                    }
                    else
                    {
                        passPages.PassPages(AddItemsToCounter(pages.name));
                    }
                    hand[0] = null;
                    break;
                default:
                    itemInMainHand = ItemInMainHand.empty;
                    break;
            }
        }

        if (hand[0] != null && GameManager.putOnCounter && !counterTopScript.inUse && readyToInteract)
        {
            counterTopScript.AddToCounterTop(hand[0].ToString());
            hand[0] = null;
        }

        if (GameManager.isTouchingBook && GameManager.recipeIsOpenP1)
        {
            GameManager.recipeIsOpenP1 = false;
            cookBook.setActiveFalseFunc();
        } else if (GameManager.isTouchingBook && !GameManager.recipeIsOpenP1)
        {
            GameManager.recipeIsOpenP1 = true;
            cookBook.setActiveTrueFunc();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Interactable" || other.gameObject.tag == "CookBook" || other.gameObject.tag == "CounterTop")
        { 
            readyToInteract = true; //assign ready to interact so that isinteracting can be set from OnInteract()

            if (other.gameObject.GetComponent<Item>() != null)
            {
                other.gameObject.GetComponent<Item>().CheckHand(itemInMainHand, this);
                interactionText.text = other.gameObject.GetComponent<Item>().Interaction;

                if (isInteracting && !other.gameObject.GetComponent<Item>().prone && /* This is temporary just for prototype*/ other.gameObject.GetComponent<Item>().Name != "Plate") //check isinteracting on the item
                {
                    isInteracting = false; //turn off isinteracting HERE to prevent problems
                    if (hand[0] == null)
                    {
                        hand[0] = other.gameObject.GetComponent<Item>();
                        Inv1.text = hand[0].Name;
                        DestroyOutline(other.gameObject);
                    }
                    else if (hand[1] == null)
                    {
                        hand[1] = hand[0];
                        hand[0] = other.gameObject.GetComponent<Item>();
                        Inv1.text = hand[0].Name;
                        Inv2.text = hand[1].Name;
                        DestroyOutline(other.gameObject);
                    }
                }
                else
                {
                    other.gameObject.GetComponent<Item>().prone = false;
                    isInteracting = false;
                }
            }
            else if (other.gameObject.GetComponent<Utility>() != null)
            {
                other.gameObject.GetComponent<Utility>().CheckHand(itemInMainHand, this);
                interactionText.text = other.gameObject.GetComponent<Utility>().Interaction;
            }
        }
        else if (other.gameObject.tag == "PassItems" && !readyToInteract)
        {
            if(other.TryGetComponent(out Window wind))
            {
                wind.CheckHand(itemInMainHand, this);
            }
            interactionText.text = other.gameObject.GetComponent<Utility>().Interaction;
        }
        if (other.gameObject.tag == "PassItems")
        {
            GameManager.passItemsReady = true;
        }

        if (other.gameObject.tag == "CounterTop")
        {
            GameManager.putOnCounter = true;
            counterTopScript = other.gameObject.GetComponent<CounterTop>();
            counterTopScript.CheckIfInUse();
        }
    }

    /// <summary>
    /// Destroy's the object's item outline if it has one
    /// </summary>
    /*// <param name="go"></param>*/
    public void DestroyOutline(GameObject go)
    {
        if (go.TryGetComponent(out Outline ol))
        {
            Destroy(ol);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactionText.text = "";
        readyToInteract = false;
        isInteracting = false;

        if (other.gameObject.tag == "CookBook")
        {
            GameManager.isTouchingBook = false;
            cookBook = null;
        }

        //if not looking at the plate, deactivate slider
        if(other.GetComponent<Plate>() != null)
        {
            other.GetComponent<Plate>().sliderTimer.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "PassItems")
        {
            GameManager.passItemsReady = false;
        }

        if (other.gameObject.tag == "CounterTop")
        {
            GameManager.putOnCounter = false;
            counterTopScript.DeleteGameObject();
        }

        DestroyOutline(other.gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CookBook")
        {
            GameManager.isTouchingBook = true;
            cookBook = GameObject.Find("CookBook_Closed").GetComponent<RecipeBook>();
        }
        if (other.gameObject.tag == "Interactable" || other.gameObject.tag == "CookBook")
        {
            //Add highlight to item
            if (!other.TryGetComponent<Outline>(out _)) //Using discard here because we don't need the outline
            {
                var outline = other.gameObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineVisible;
                outline.OutlineColor = outlineColor;
                outline.OutlineWidth = 3f;
            }
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

    public void OnThrowKnife()
    {
        if ((hand[0] == null || hand[1] == null) && readyToThrow)
        {
            readyToThrow = false;

            // instantiate object to throw
            GameObject projectile = Instantiate(objectToThrow, attackPoint.position, transform.rotation);

            // get rigidbody component
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            // calculate direction
            KnifeAddon kscript = projectile.GetComponent<KnifeAddon>();

            kscript.forward = transform.forward;

            // implement throwCooldown
            Invoke(nameof(ResetThrow), throwCooldown);
        }

        if (GameManager.isTouchingBook && GameManager.recipeIsOpenP1)
        {
            cookBook.ClickOnBook();
        }
    }

    public void OnToggleAimLine()
    {
        isAiming = !isAiming;
        LineRenderer aimLine = gameObject.GetComponent<LineRenderer>();

        if (isAiming)
        {
            aimLine.enabled = true;
        }
        else
        {
            aimLine.enabled = false;
        }
    }

    public void ResetThrow()
    {
        readyToThrow = true;
    }

    public void AddOrder(string name, int timer)
    {
        GameObject orderRef = Instantiate(orderPrefab, orderLayoutGroup.transform);
        Order order = orderRef.GetComponent<Order>();
        order.AssignOrder(name, timer);
    }

    public int AddItemsToCounter(string checkItem)
    {
        int itemLocation = -1;

        for (int i = 0; i <= GameManager.counterItems.Length; i++)
        {
            if (i >= GameManager.counterItems.Length)
            {
                return (itemLocation);
            }

            if (GameManager.counterItems[i] == "")
            {
                itemLocation = i;
                GameManager.counterItems[i] = checkItem;
                return (itemLocation);
            }
        }
        return (itemLocation);
    }
}
