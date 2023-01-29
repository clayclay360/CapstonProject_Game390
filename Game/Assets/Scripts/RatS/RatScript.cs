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

    [Header("Stats")]
    public string item;
    public bool isCarryingItem;
    public Canvas hbCanv;

    [Header("Target")]
    public float attackRadius;
    public List<GameObject> TargetsList;
    public GameObject target;
    public List<GameObject> DestinationsList;
    public bool objectiveComplete;
    public GameObject[] ventsTransform;

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
    private bool attackReady;
    Bacon baconRespawn;
    Egg eggRespawn;

    private NavMeshAgent agent;
    private MeshRenderer climbableTargetMesh;
    private Transform escapeVent;
    private RatSpawnSystem ratSpawnSystem;
    private GameObject itemObject;
    private List<GameObject> destinationsList;
    private List<GameObject> itemsList;
    private Item itemScript;
    private DestinationPoint destinationScript;
    private Rigidbody ratBody;

    // Start is called before the first frame update
    private void Awake()
    {
        ratBody = gameObject.GetComponent<Rigidbody>();
        ol.enabled = false;
        startHeight = transform.position.y;
        attackReady = true;
        hiding = false;
        //Create healthbar and instance it
        GameObject hbar = Instantiate(healthBar);
        hbarScript = hbar.GetComponent<RatHealthBar>();
        hbarScript.rat = gameObject;
        hbarScript.gameObject.SetActive(false);

        //GetTarget();
        ventsTransform = GameObject.FindGameObjectsWithTag("RatVent");
        ratSpawnSystem = FindObjectOfType<RatSpawnSystem>();
        agent = GetComponent<NavMeshAgent>();
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

        var CanvRot = hbCanv.transform.rotation.eulerAngles;
        CanvRot.z = -transform.rotation.eulerAngles.y;
        hbCanv.transform.rotation = Quaternion.Euler(CanvRot);
        baconRespawn = GameManager.bacon;
        eggRespawn = GameManager.egg;

        if(ratBody.velocity != agent.desiredVelocity)
        {
            ratBody.drag = 100;
        }
        else
        {
            ratBody.drag = 10;
        }
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
                itemScript.isBeingCarried = false;
                ol.enabled = false;
                item = "";
                hbarScript.SetItemText(item);
            }
            ratSpawnSystem.ratsList.Remove(gameObject);
            Destroy(hbarScript.gameObject);
            Destroy(gameObject);
        }
    }

    private void GetAction()
    {

        if (target != null && !hiding)
        {
            if(target.GetComponent<Collider>() != null)
            {
                Vector3 closestTargetPoint = target.GetComponent<Collider>().ClosestPoint(transform.position);
                distanceBetweenTarget = Vector3.Distance(transform.position, closestTargetPoint);
            }
            else
            {
                distanceBetweenTarget = Vector3.Distance(transform.position, target.transform.position);
            }
            agent.stoppingDistance = attackRadius;

            //Debug.Log(gameObject.name + " " + distanceBetweenTarget.ToString());
            if (distanceBetweenTarget <= attackRadius)
            {
                LookAt();
                Attack();
            }
            else
            {
                CheckTarget(target);
            }
        }
        else
        {
            ReturnToVent();
        }
    }

    private void SetAgentDestination(GameObject target)
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
            //Debug.Log(path.status);
        }
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
            Debug.Log(gameObject.name + " hit" + other.gameObject.name);
            collider.enabled = false;
            switch (target.tag)
            {
                case "CookBook":
                    CookBook cookbook = other.GetComponentInParent<CookBook>();
                    cookbook.lives--;
                    target = null;
                    objectiveComplete = true;
                    break;

                case "Destination":
                    if(isCarryingItem)
                    {
                        itemObject = GameObject.Find(item);
                        itemScript = itemObject.GetComponent<Item>();
                        destinationScript = target.GetComponent<DestinationPoint>();
                        itemObject.transform.position = other.gameObject.transform.position;
                        itemScript.RespawnItem(itemObject);
                        isCarryingItem = false;
                        itemScript.isBeingCarried = false;
                        destinationScript.hasItem = true;
                        ol.enabled = false;
                        if (item == "Egg")
                        {
                            eggRespawn.Respawn();
                        } else if (item == "Bacon")
                        {
                            baconRespawn.Respawn();
                        }
                        item = "";
                        hbarScript.SetItemText(item);
                    }
                    target = null;
                    objectiveComplete = true;
                    break;

                case "Interactable":
                    Debug.Log("Hit " + other.gameObject.name);
                        switch (other.gameObject.name)
                        {
                            case ("Spatula"):
                                Spatula spatula = other.gameObject.GetComponent<Spatula>();
                                spatula.isTarget = false;
                                spatula.status = Item.Status.dirty;
                                spatula.DespawnItem(other.gameObject);
                                item = other.gameObject.name;
                                hbarScript.SetItemText(item);
                                SelectDestination(DestinationsList);
                                isCarryingItem = true;
                                spatula.isBeingCarried = true;
                                ol.enabled = true;
                                spatula.CheckCounter();
                                if (counter != null)
                                {
                                    CounterTop counterScript = counter.GetComponentInChildren<CounterTop>();
                                    counterScript.inUse = false;
                                }
                            break;

                            case ("Pan"):
                                Pan pan = other.gameObject.GetComponent<Pan>();
                                pan.isTarget = false;
                                pan.status = Item.Status.dirty;
                                pan.DespawnItem(other.gameObject);
                                item = other.gameObject.name;
                                hbarScript.SetItemText(item);
                                SelectDestination(DestinationsList);
                                isCarryingItem = true;
                                pan.isBeingCarried = true;
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

                            case ("Egg"):
                                Egg egg = other.gameObject.GetComponent<Egg>();
                                egg.isTarget = false;
                                egg.DespawnItem(other.gameObject);
                                item = other.gameObject.name;
                                hbarScript.SetItemText(item);
                                SelectDestination(DestinationsList);
                                isCarryingItem = true;
                                egg.isBeingCarried = true;
                                ol.enabled = true;
                                egg.CheckCounter();
                                egg.status = Item.Status.spoiled;
                                if (counter != null)
                                {
                                    CounterTop counterScript = counter.GetComponentInChildren<CounterTop>();
                                    counterScript.inUse = false;
                                }
                            break;

                            case ("Bacon"):
                                Bacon bacon = other.gameObject.GetComponent<Bacon>();
                                bacon.isTarget = false;
                                bacon.DespawnItem(other.gameObject);
                                item = other.gameObject.name;
                                hbarScript.SetItemText(item);
                                SelectDestination(DestinationsList);
                                isCarryingItem = true;
                                bacon.isBeingCarried = true;
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
        Debug.Log(gameObject.name + " looking at target");
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
                    //Don't target if spatula is dirty, despawned, or being targeted or carried by another rat
                    Spatula spatula = item.GetComponent<Spatula>();
                    if (spatula.status == Item.Status.dirty || !spatula.isActiveAndEnabled || spatula.isTarget || spatula.isBeingCarried)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("Plate"):
                case ("Plate(Clone)"):
                    removeList.Add(item);
                    break;

                case ("Pan"):
                    //Don't target if pan is dirty, despawned, or being targeted or carried by another rat
                    Pan pan = item.GetComponent<Pan>();
                    if (pan.status == Item.Status.dirty || !pan.isActiveAndEnabled || pan.isTarget || pan.isBeingCarried)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("Sink"):
                    //Don't target sink if it's off or being targeted by another rat
                    Sink sink = item.GetComponent<Sink>();
                    if (!sink.On || sink.isTarget)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("Stove"):
                    //Don't target stove if it's off or being targeted by another rat
                    Stove stove = item.GetComponent<Stove>();
                    if (!stove.On || stove.isTarget)
                    {
                        removeList.Add(item);
                    }
                    break;
                
                case ("Egg"):
                    //Don't target egg if it's despawned or being targeted or carried by another rat
                    Egg egg = item.GetComponent<Egg>();
                    if (!egg.isActiveAndEnabled || egg.isTarget || egg.isBeingCarried)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("TrashCan"):
                    removeList.Add(item);
                    break;

                case ("Bacon"):
                    //Don't target bacon if it's despawned or being targeted or carried by another rat
                    Bacon bacon = item.GetComponent<Bacon>();
                    if (!bacon.isActiveAndEnabled || bacon.isTarget || bacon.isBeingCarried)
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

        if(targetList.Count > 0)
        {
            SetTarget(targetList);
        }
        else
        {
            target = null;
            ReturnToVent();
        }
    }

    public void SelectDestination(List<GameObject> destinationList)
    {
        GameObject[] destinationsArray = GameObject.FindGameObjectsWithTag("Destination");
        destinationList.AddRange(destinationsArray);
        List<GameObject> removeList = new List<GameObject> { };
        //Remove potential destinations that already have items in them
        foreach (GameObject destination in destinationList)
        {
            if (destination.GetComponent<DestinationPoint>().hasItem)
            {
                removeList.Add(destination);
            }
        }
        foreach(GameObject destination in removeList)
        {
            if (destinationList.Contains(destination))
            {
                destinationList.Remove(destination);
            }
        }
        target = destinationList[Random.Range(0, destinationList.Count)];
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
            if (target.GetComponent<Utility>() != null)
            {
                target.GetComponent<Utility>().isTarget = true;
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
        agent.CalculatePath(agent.destination, path);
        //Debug.Log(path.status);
    }

    public void CheckTarget(GameObject target)
    {
        bool switchTarget = false;
        bool switchDestination = false;
        if(target.tag == "Destination")
        {
            DestinationPoint destination = target.GetComponent<DestinationPoint>();
            if (destination.hasItem)
            {
                switchDestination = true;
            }
        }
        switch (target.name)
        {
            case ("CookBook"):
                //Switch target from cookbook if it's destroyed
                if (!GameManager.cookBookActive)
                {
                    switchTarget = true;
                }
                break;

            case ("Spatula"):
                //Switch target if spatula is dirty, despawned, or being carried by another rat
                Spatula spatula = target.GetComponent<Spatula>();
                if (spatula.status == Item.Status.dirty || !spatula.isActiveAndEnabled || spatula.isBeingCarried)
                {
                    switchTarget = true;
                }
                break;

            case ("Pan"):
                //Switch target if pan is dirty, despawned, or being carried by another rat
                Pan pan = target.GetComponent<Pan>();
                if (pan.status == Item.Status.dirty || !pan.isActiveAndEnabled || pan.isBeingCarried)
                {
                    switchTarget = true;
                }
                break;

            case ("Sink"):
                //Switch target from sink if it's off
                if (!target.GetComponent<Sink>().On)
                {
                    switchTarget = true;
                }
                break;

            case ("Stove"):
                //Switch target from stove if it's off
                if (!target.GetComponent<Stove>().On)
                {
                    switchTarget = true;
                }
                break;

            case ("Egg"):
                //Don't target egg if it's despawned or being carried by another rat
                Egg egg = target.GetComponent<Egg>();
                if (!egg.isActiveAndEnabled || egg.isBeingCarried)
                {
                    switchTarget = true;
                }
                break;

            case ("Bacon"):
                //Don't target bacon if it's despawned or being carried by another rat
                Bacon bacon = target.GetComponent<Bacon>();
                if (!bacon.isActiveAndEnabled || bacon.isBeingCarried)
                {
                    switchTarget = true;
                }
                break;
        }
        if (switchTarget)
        {
            AdjustTargetList(TargetsList);
        }
        else if (switchDestination)
        {
            SelectDestination(DestinationsList);
        }
        else
        {
            agent.destination = target.transform.position;
        }
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
                ratSpawnSystem.ratsList.Remove(gameObject);
                Destroy(hbarScript.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
