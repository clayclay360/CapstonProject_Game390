using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatScript : MonoBehaviour
{
    [Header("Variables")]
    public GameObject body;
    public GameObject counter;
    public Outline ol;

    [Header("Healthbar")]
    public int health;
    public GameObject healthBar;
    private RatHealthBar hbarScript;
    //private float hbarTimer = 0f;
    //private const float HBARTIME = 3f;
    //private bool hbarVisible = false;

    [Header("Stats")]
    public string item;
    public bool isCarryingItem;
    public Canvas hbCanv;

    [Header("Target")]
    public float attackRadius;
    public List<GameObject> TargetsList;
    public GameObject target;
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
    Bacon baconRespawn;
    Egg eggRespawn;

    private NavMeshAgent agent;
    private MeshRenderer climbableTargetMesh;
    private Transform escapeVent;
    private RatSpawnSystem ratSpawnSystem;
    private GameObject itemObject;
    private Item itemScript;

    // Start is called before the first frame update
    private void Awake()
    {
        ol.enabled = false;
        startHeight = transform.position.y;
        attackReady = true;
        hiding = false;
        climbing = false;
        //Create healthbar and instance it
        GameObject hbar = Instantiate(healthBar);
        hbarScript = hbar.GetComponent<RatHealthBar>();
        hbarScript.rat = gameObject;
        hbarScript.gameObject.SetActive(false);

        //GetTarget();
        ventsTransform = GameObject.FindGameObjectsWithTag("RatVent");
        ratSpawnSystem = FindObjectOfType<RatSpawnSystem>();
        agent = GetComponent<NavMeshAgent>();
        offMeshLink.GetComponent<OffMeshLink>();
        startLink.GetComponent<Transform>();
        endLink.GetComponent<Transform>();
        //AdjustTargetList(TargetsList);
        hidingPointsList = GameObject.FindGameObjectsWithTag("HidingPoint");

        hbarScript.SetMaxHealth(health);
    }

    private void Start()
    {
        AdjustTargetList(TargetsList);
    }

    // Update is called once per frame
    void Update()
    {
        GetAction();
        //RayCast();
        //Climbing
        DistanceBetweenTarget();
        ReturnToVent();

        var CanvRot = hbCanv.transform.rotation.eulerAngles;
        CanvRot.z = -transform.rotation.eulerAngles.y;
        hbCanv.transform.rotation = Quaternion.Euler(CanvRot);
        baconRespawn = GameManager.bacon;
        eggRespawn = GameManager.egg;

        //Healthbar timer—disappears if it has been visible for more than 3 seconds
        //if (hbarVisible)
        //{
        //    hbarTimer += Time.deltaTime;
        //    if (hbarTimer >= HBARTIME)
        //    {
        //        hbarScript.gameObject.SetActive(false);
        //        hbarTimer = 0f;
        //        hbarVisible = false;
        //    }
        //}
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        hbarScript.SetHealth(health);
        hbarScript.gameObject.SetActive(true);
        hbarScript.hbarVisible = true;

        if (health <= 0)
        {
            if(isCarryingItem)
            {
                itemObject = GameObject.Find(item);
                itemScript = itemObject.GetComponent<Item>();
                itemObject.transform.position = transform.position;
                itemScript.RespawnItem(itemObject);
                isCarryingItem = false;
                ol.enabled = false;
                item = "";
                hbarScript.SetItemText(item);
            }
            ratSpawnSystem.numberOfRats--;
            Destroy(hbarScript.gameObject);
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
                //SetAgentDestination();
            }
            else
            {
                LookAt();
                Attack();
            }
        }
        else
        {
            ReturnToVent();
        }
    }

    private void SetAgentDestination()
    {
        if(agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            NavMeshPath path = new NavMeshPath();
            if (!agent.isOnNavMesh)
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas);
                agent.Warp(hit.position);
                agent.enabled = false;
                agent.enabled = true;
            }
            agent.SetDestination(target.transform.position);
            agent.CalculatePath(agent.destination, path);
            Debug.Log(path.status);
        }
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
                Debug.Log(collider.gameObject.name + "Is Cloest Enough To Climb");
                if (collider.gameObject.CompareTag("Climbable"))
                {
                    Debug.Log(collider.gameObject.name + "Is Climbable");
                    radius = Vector3.Distance(transform.position, collider.ClosestPoint(transform.position));
                    closestCollider = collider;
                }
            }
        }

        if (!climbing)
        {
            if(closestCollider.gameObject.GetComponent<MeshRenderer>() != null)
            {
                climbableTargetMesh = closestCollider.gameObject.GetComponent<MeshRenderer>();
            }
            else
            {
                climbableTargetMesh = closestCollider.gameObject.GetComponentInChildren<MeshRenderer>();
            }
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
        Transform[] jumpPoints;
        if (climbableTargetMesh != null)
        {
            if(climbableTargetMesh.transform.childCount > 0)
            {
                jumpPoints = climbableTargetMesh.GetComponentsInChildren<Transform>();
            }
            else
            {
                jumpPoints = climbableTargetMesh.transform.parent.gameObject.GetComponentsInChildren<Transform>();
            }
            float radius = climbRaduis * 3;
            Transform closestJumpPoint = null;
            foreach (Transform jumpPoint in jumpPoints)
            {
                if (Vector3.Distance(transform.position, jumpPoint.position) < radius && jumpPoint.transform.position.y > 0.5 && jumpPoint.gameObject.CompareTag("JumpPoints"))
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
        if (other.gameObject == target)
        {
            //Debug.Log(gameObject.name + " hit" + other.gameObject.name);
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
                    if(isCarryingItem)
                    {
                        itemObject = GameObject.Find(item);
                        itemScript = itemObject.GetComponent<Item>();
                        itemObject.transform.position = other.gameObject.transform.position;
                        itemScript.RespawnItem(itemObject);
                        isCarryingItem = false;
                        ol.enabled = false;
                        if (item == "Egg(Clone)")
                        {
                            eggRespawn.Respawn();
                        } else if (item == "Bacon(Clone)")
                        {
                            baconRespawn.Respawn();
                        }
                        item = "";
                        hbarScript.SetItemText(item);
                    }
                    objectiveComplete = true;
                    break;

                case "Interactable":
                    //Debug.Log("Hit Interactable Object");
                        switch (other.gameObject.name)
                        {
                            case ("Spatula"):
                                Spatula spatula = other.gameObject.GetComponent<Spatula>();
                                spatula.isTarget = false;
                                spatula.status = Item.Status.dirty;
                                spatula.DespawnItem(other.gameObject);
                                item = other.gameObject.name;
                                hbarScript.SetItemText(item);
                                SelectDestination();
                                isCarryingItem = true;
                                ol.enabled = true;
                                spatula.CheckCounter();
                                if (counter != null)
                                {
                                    CounterTop counterScript = counter.GetComponentInChildren<CounterTop>();
                                    counterScript.inUse = false;
                                }
                            break;

                            //case ("Plate"):
                            //    Plate plate = other.gameObject.GetComponent<Plate>();
                            //    plate.isTarget = false;
                            //    plate.status = Item.Status.dirty;
                            //    plate.DespawnItem(other.gameObject);
                            //    item = other.gameObject.name;
                            //    hbarScript.SetItemText(item);
                            //    SelectDestination();
                            //    isCarryingItem = true;
                            //    break;

                            case ("Pan"):
                                Pan pan = other.gameObject.GetComponent<Pan>();
                                pan.isTarget = false;
                                pan.status = Item.Status.dirty;
                                pan.DespawnItem(other.gameObject);
                                item = other.gameObject.name;
                                hbarScript.SetItemText(item);
                                SelectDestination();
                                isCarryingItem = true;
                                ol.enabled = true;
                                pan.CheckCounter();
                                if (counter != null)
                                {
                                    CounterTop counterScript = counter.GetComponentInChildren<CounterTop>();
                                    counterScript.inUse = false;
                                }
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

                            case ("Egg(Clone)"):
                                Egg egg = other.gameObject.GetComponent<Egg>();
                                egg.isTarget = false;
                                egg.DespawnItem(other.gameObject);
                                item = other.gameObject.name;
                                hbarScript.SetItemText(item);
                                SelectDestination();
                                isCarryingItem = true;
                                ol.enabled = true;
                                egg.CheckCounter();
                                egg.status = Item.Status.spoiled;
                                if (counter != null)
                                {
                                    CounterTop counterScript = counter.GetComponentInChildren<CounterTop>();
                                    counterScript.inUse = false;
                                }
                            break;

                            case ("Bacon(Clone)"):
                                Bacon bacon = other.gameObject.GetComponent<Bacon>();
                                bacon.isTarget = false;
                                bacon.DespawnItem(other.gameObject);
                                item = other.gameObject.name;
                                hbarScript.SetItemText(item);
                                SelectDestination();
                                isCarryingItem = true;
                                ol.enabled = true;
                                bacon.CheckCounter();
                                bacon.status = Item.Status.spoiled;
                                if (counter != null)
                                {
                                    CounterTop counterScript = counter.GetComponentInChildren<CounterTop>();
                                    counterScript.inUse = false;
                                }
                            break;
                        }
                        break;
            }
        }

        if (other.gameObject.tag == "CounterTop")
        {
            counter = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CounterTop")
        {
            counter = null;
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
                    //Don't target if spatula is dirty, despawned, or being targeted by another rat
                    Spatula spatula = item.GetComponent<Spatula>();
                    if (spatula.status == Item.Status.dirty || !spatula.isActive || spatula.isTarget)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("Plate"):
                case ("Plate(Clone)"):
                    removeList.Add(item);
                    break;

                case ("Pan"):
                    //Don't target if pan is dirty, despawned, or being targeted by another rat
                    Pan pan = item.GetComponent<Pan>();
                    if (pan.status == Item.Status.dirty || !pan.isActive || pan.isTarget)
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

                case ("Egg"):
                    removeList.Add(item);
                    break;
                
                case ("Egg(Clone)"):
                    //Don't target egg if it's despawned or being targeted by another rat
                    Egg egg = item.GetComponent<Egg>();
                    if (!egg.isActive || egg.isTarget)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("TrashCan"):
                    removeList.Add(item);
                    break;

                case ("Bacon"):
                    removeList.Add(item);
                    break;

                case ("Bacon(Clone)"):
                    //Don't target bacon if it's despawned or being targeted by another rat
                    Bacon bacon = item.GetComponent<Bacon>();
                    if (!bacon.isActive || bacon.isTarget)
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

                case ("Pages"):
                case ("Pages2"):
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
        agent.SetDestination(target.transform.position);
    }

    public void SetTarget(List<GameObject> targetList)
    {
        target = targetList[Random.Range(0, targetList.Count)];

        //Debug.Log(gameObject.name + " is targeting: " + target.name);
        NavMeshPath path = new NavMeshPath();
        if(target != null)
        {
            if (target.GetComponent<Item>() != null)
            {
                target.GetComponent<Item>().isTarget = true;
            }
            if (!agent.isOnNavMesh)
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(transform.position, out hit, 1f, 1);
                agent.Warp(hit.position);
                agent.enabled = false;
                agent.enabled = true;
            }
            agent.SetDestination(target.transform.position);
        }
        //else
        //{
        //    float distance = Random.Range(1, 20);
        //    Vector3 direction = Random.insideUnitSphere * distance;
        //    direction += transform.position;

        //}
        agent.CalculatePath(agent.destination, path);
        //Debug.Log(path.status);
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

        yield return new WaitForSeconds(0.1f);
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
