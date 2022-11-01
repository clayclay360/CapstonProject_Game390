using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatScript : MonoBehaviour
{
    [Header("Variables")]
    public GameObject body;

    [Header("Stats")]
    public int health;
    public RatHealthBar ratHealthBar;
    public string item;
    public bool isCarryingItem;

    [Header("Target")]
    public float attackRadius;
    public List<GameObject> TargetsList;
    public bool objectiveComplete;
    public GameObject[] ventsTransform;

    [Header("Off Mesh Link")]
    public OffMeshLink offMeshLink;
    public Transform startLink;
    public Transform endLink;
    public float startLinkOffset;
    public float endLinkOffset;

    [Header("Climb")]
    public float climbRaduis;
    public float platformYOffset;
    public float climbCoolDown;

    [Header("Attack")]
    public float attackRate;
    public float attackCoolDown;
    public new Collider collider;

    [Header("Scared")]
    public GameObject[] hidingPointsList;
    public float minHideTimer;
    public float maxHideTimer;
    public bool hiding;

    private float distanceBetweenTarget;
    private float startHeight;
    private float hideTime;
    private bool linkActivated;
    private bool climbing;
    private bool attackReady;

    private NavMeshAgent agent;
    private MeshRenderer climbableTargetMesh;
    private Transform escapeVent;
    private RatSpawnSystem ratSpawnSystem;
    private GameObject target;

    // Start is called before the first frame update
    private void Awake()
    {
        startHeight = transform.position.y;
        attackReady = true;
        hiding = false;
        climbing = false;

        //GetTarget();
        ventsTransform = GameObject.FindGameObjectsWithTag("RatVent");
        ratSpawnSystem = FindObjectOfType<RatSpawnSystem>();
        agent = GetComponent<NavMeshAgent>();
        offMeshLink.GetComponent<OffMeshLink>();
        startLink.GetComponent<Transform>();
        endLink.GetComponent<Transform>();
        AdjustTargetList(TargetsList);
        hidingPointsList = GameObject.FindGameObjectsWithTag("HidingPoint");

        ratHealthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        GetAction();
        //RayCast();
        //Climbing
        DistanceBetweenTarget();
        ReturnToVent();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        ratHealthBar.SetHealth(health);

        if (health <= 0)
        {
            ratSpawnSystem.numberOfRats--;
            Destroy(gameObject);
        }
    }

    private void GetAction()
    {

        if (target != null && !hiding)
        {
            distanceBetweenTarget = Vector3.Distance(transform.position, target.transform.position);
            agent.stoppingDistance = attackRadius;

            if (distanceBetweenTarget > attackRadius)
            {
                //Debug.Log("Distance to "+target.name+": " + distanceBetweenTarget.ToString());
                MoveToTarget();
            }
            else
            {
                //LookAt();
                Attack();
            }
        }
        else
        {
            ReturnToVent();
        }
    }

    private void MoveToTarget()
    {
        agent.destination = target.transform.position;
    }

    private void LookForClosestClimbableObject()
    {
        Collider closestCollider = null;
        float radius = climbRaduis;
        Collider[] colliders = Physics.OverlapSphere(transform.position, climbRaduis);

        foreach(Collider collider in colliders)
        {
            if(Vector3.Distance(transform.position, collider.ClosestPoint(transform.position)) < radius)
            {
                if (collider.gameObject.CompareTag("Climbable"))
                {
                    radius = Vector3.Distance(transform.position, collider.ClosestPoint(transform.position));
                    closestCollider = collider;
                }
            }
        }

        if (!climbing)
        {
            climbableTargetMesh = closestCollider.gameObject.GetComponent<MeshRenderer>();
        }
    }

    private void DistanceBetweenTarget()
    {
        if (target != null)
        {
            float distanceBetweenTarget = Vector3.Distance(transform.position, target.transform.position);
            //Debug.Log(distanceBetweenTarget.ToString());
            if (distanceBetweenTarget < climbRaduis && !climbing)
            {
                if (transform.position.y + platformYOffset < target.transform.position.y)
                {
                    Debug.Log(gameObject.name + " climb");
                    Climb();
                    StartCoroutine(ClimbCoolDOwn());
                }
            }
        }
    }

    private void Climb()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        startLink.position = transform.position;
        LookForClosestClimbableObject();
        if (climbableTargetMesh != null)
        {
            Transform[] jumpPoints = climbableTargetMesh.GetComponentsInChildren<Transform>();
            float radius = climbRaduis * 3;
            Transform closestJumpPoint = null;
            foreach (Transform jumpPoint in jumpPoints)
            {
                if (Vector3.Distance(transform.position, jumpPoint.position) < radius && jumpPoint.transform.position.y > 0.5)
                {
                    radius = Vector3.Distance(transform.position, jumpPoint.position);
                    closestJumpPoint = jumpPoint;
                }
            }

            endLink.position = closestJumpPoint.position;
        }
        else
        {
            endLink.position = new Vector3((dir.x + body.transform.position.x), target.transform.position.y + transform.position.y, (dir.z + body.transform.position.z));
        }
        offMeshLink.activated = true;
        climbing = true;
    }

    private IEnumerator ClimbCoolDOwn()
    {
        yield return new WaitForSeconds(climbCoolDown);
        climbing = false;
        offMeshLink.activated = false;
    }

    private void Attack()
    {
        if (attackReady)
        {
            attackReady = false;
            collider.enabled = true;
            StartCoroutine(AttackRate());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Untagged" && other.CompareTag(target.tag))
        {
            Debug.Log(gameObject.name + " hit" + other.gameObject.name);
            collider.enabled = false;
            switch (target.tag)
            {
                case "CookBook":
                    CookBook cookbook = other.GetComponentInParent<CookBook>();
                    cookbook.lives--;
                    if (cookbook.lives == 0)
                    {
                        objectiveComplete = true;
                    }
                    break;

                case "Destination":
                    if(item != "")
                    {
                        GameObject itemObject = GameObject.Find(item);
                        Item itemScript = itemObject.GetComponent<Item>();
                        itemObject.transform.position = other.gameObject.transform.position;
                        itemScript.RespawnItem(itemObject);
                        isCarryingItem = false;
                        item = "";
                        ratHealthBar.SetItemText(item);
                    }
                    objectiveComplete = true;
                    break;

                case "Interactable":
                    //Debug.Log("Hit Interactable Object");
                    switch (other.gameObject.name)
                    {
                        case ("Spatula"):
                            Spatula spatula = other.gameObject.GetComponent<Spatula>();
                            spatula.status = Item.Status.dirty;
                            spatula.DespawnItem(other.gameObject);
                            item = other.gameObject.name;
                            ratHealthBar.SetItemText(item);
                            SelectDestination();
                            isCarryingItem = true;
                            break;

                        case ("Plate"):
                            Plate plate = other.gameObject.GetComponent<Plate>();
                            plate.status = Item.Status.dirty;
                            plate.DespawnItem(other.gameObject);
                            item = other.gameObject.name;
                            ratHealthBar.SetItemText(item);
                            SelectDestination();
                            isCarryingItem = true;
                            break;

                        case ("Pan"):
                            Pan pan = other.gameObject.GetComponent<Pan>();
                            pan.status = Item.Status.dirty;
                            pan.DespawnItem(other.gameObject);
                            item = other.gameObject.name;
                            ratHealthBar.SetItemText(item);
                            SelectDestination();
                            isCarryingItem = true;
                            break;

                        case ("Sink"):
                            objectiveComplete = true;
                            break;

                        case ("Stove"):
                            Stove stove = other.gameObject.GetComponent<Stove>();
                            stove.On = false;
                            stove.State(stove.On);
                            objectiveComplete = true;
                            break;

                        case ("Egg"):  case ("Egg(Clone)"):
                            Egg egg = other.gameObject.GetComponent<Egg>();
                            egg.DespawnItem(other.gameObject);
                            item = other.gameObject.name;
                            ratHealthBar.SetItemText(item);
                            SelectDestination();
                            isCarryingItem = true;
                            break;

                        case ("Bacon"):
                            Bacon bacon = other.gameObject.GetComponent<Bacon>();
                            bacon.DespawnItem(other.gameObject);
                            item = other.gameObject.name;
                            ratHealthBar.SetItemText(item);
                            SelectDestination();
                            isCarryingItem = true;
                            break;
                    }
                    break;
            }
        }
    }

    IEnumerator AttackRate()
    {
        yield return new WaitForSeconds(attackRate);
        collider.enabled = false;
        yield return new WaitForSeconds(attackCoolDown);
        attackReady = true;
    }

    private void LookAt()
    {
        Vector2 dir = target.transform.position - transform.position;
        transform.forward = dir;
    }

    public void AdjustTargetList(List<GameObject> targetList)
    {
        //Get all interactable items and the cookbook
        GameObject[] targetarray =  GameObject.FindGameObjectsWithTag("Interactable");
        targetList.AddRange(targetarray);
        targetList.Add(GameObject.FindGameObjectWithTag("CookBook"));

        //Remove items that we don't want the rats targeting in a given surcunstance.
        List<GameObject> removeList = new List<GameObject> { };
        foreach (GameObject item in targetList)
        {
            switch (item.name)
            {
                case ("CookBook"):
                    //Don't target cookbook if it's destroyed
                    if (!GameManager.cookBookActive)
                    {
                        removeList.Add(item);
                    }
                    break;
                
                case ("Spatula"):
                    //Don't target if spatula is dirty
                    if(item.GetComponent<Spatula>().status == Item.Status.dirty)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("Plate"):
                    //Don't target if plate is dirty
                    if (item.GetComponent<Plate>().status == Item.Status.dirty)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("Pan"):
                    //Don't target if pan is dirty
                    if (item.GetComponent<Pan>().status == Item.Status.dirty)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("Sink"):
                    //Don't target sink if it's off
                    if (!item.GetComponent<Sink>().On)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("Stove"):
                    //Don't target stove if it's off
                    if (!item.GetComponent<Stove>().On)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("Egg"): case ("Egg(Clone)"):
                    //Don't target egg if it's despawned
                    if (!item.GetComponent<Egg>().isActive)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("TrashCan"):
                    removeList.Add(item);
                    break;

                case ("Bacon"):
                    //Don't target bacon if it's despawned
                    if (!item.GetComponent<Bacon>().isActive)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("Fridge"):
                    removeList.Add(item);
                    break;

                case ("Computer"):
                    removeList.Add(item);
                    break;
            }

            if (item == target)
            {
                removeList.Add(item);
            }
        }
        foreach (GameObject item in removeList)
        {
            if (targetList.Contains(item))
            {
                targetList.Remove(item);
            }
        }

        SetTarget(targetList);
    }

    public void SelectDestination()
    {
        GameObject[] destinationsList = GameObject.FindGameObjectsWithTag("Destination");
        int destinationIndex = Random.Range(0, destinationsList.Length);

        target = destinationsList[destinationIndex];
    }

    public void SetTarget(List<GameObject> targetList)
    {
        target = targetList[Random.Range(0, targetList.Count)];

        Debug.Log(gameObject.name + " is targeting: " + target.name);
        Debug.Log(target.transform.position);
    }

    public void CrossEntryway()
    {
        if (!hiding)
        {
            StartCoroutine(RethinkTarget());
        }
    }

    public IEnumerator RethinkTarget()
    {
        agent.destination = transform.position;
        int chance = Random.Range(1, 100);

        if (chance >= 90)
        {
            Debug.Log(gameObject.name + " fled");

            target = null;
            ReturnToVent();
        }
        else if (chance >= 70)
        {
            Debug.Log(gameObject.name + " changed Target");

            //Pick new target from list
            AdjustTargetList(TargetsList);
        }
        else
        {
            Debug.Log(gameObject.name + " kept going");
        }

        yield return new WaitForSeconds(1f);
    }

    public void Hide()
    {
        hideTime = Random.Range(minHideTimer, maxHideTimer);
        int hideIndex = Random.Range(0, hidingPointsList.Length);

        hiding = true;
        agent.destination = hidingPointsList[hideIndex].transform.position;
        agent.speed = agent.speed * 2;
        agent.angularSpeed = agent.angularSpeed * 2;

        StartCoroutine(HideTimer());
    }

    public IEnumerator HideTimer()
    {
        float distanceToHidingPoint = Vector3.Distance(transform.position, agent.destination);
        bool reachedHidingPoint = false;
        while (distanceToHidingPoint > 0.5 && !reachedHidingPoint)
        {
            distanceToHidingPoint = Vector3.Distance(transform.position, agent.destination);
            yield return null;
        }
        reachedHidingPoint = true;
        yield return new WaitForSeconds(hideTime);
        agent.destination = target.transform.position;
        hiding = false;
        agent.speed = agent.speed / 2;
        agent.angularSpeed = agent.angularSpeed / 2;
    }

    public void ReturnToVent()
    {
        if (objectiveComplete || target == null)
        {
            float closestVent = 100;

            for (int i = 0; i < ventsTransform.Length; i++)
            {
                if (Vector3.Distance(transform.position, ventsTransform[i].transform.position) < closestVent)
                {
                    closestVent = Vector3.Distance(transform.position, ventsTransform[i].transform.position);
                    escapeVent = ventsTransform[i].transform;
                }
            }

            agent.destination = escapeVent.position;

            if (Vector3.Distance(transform.position, escapeVent.transform.position) < attackRadius)
            {
                ratSpawnSystem.numberOfRats--;
                Destroy(gameObject);
            }
        }
    }
}
