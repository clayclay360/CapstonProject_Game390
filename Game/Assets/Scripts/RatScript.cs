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

    [Header("Target")]
    public float attackRadius;
    public List<GameObject> TargetsList;
    public bool objectiveComplete;
    public GameObject[] ventsTransform;

    [Header("RayCast")]
    public float rayDistance;
    public Transform starRay;
    public Transform endRay;

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

        //GetTarget();
        ventsTransform = GameObject.FindGameObjectsWithTag("RatVent");
        ratSpawnSystem = FindObjectOfType<RatSpawnSystem>();
        agent = GetComponent<NavMeshAgent>();
        offMeshLink.GetComponent<OffMeshLink>();
        starRay.GetComponent<Transform>();
        endRay.GetComponent<Transform>();
        startLink.GetComponent<Transform>();
        endLink.GetComponent<Transform>();
        AdjustTargetList(TargetsList);
        hidingPointsList = GameObject.FindGameObjectsWithTag("HidingPoint");
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

    private void RayCast()
    {
        RaycastHit hit;

        if (Physics.Linecast(starRay.position, endRay.position, out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Climbable" && !climbing)
                {
                    climbableTargetMesh = hit.transform.gameObject.GetComponent<MeshRenderer>(); ;
                }
            }
        }
        Debug.DrawLine(starRay.position, endRay.position);
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
                    Debug.Log("climb");
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
        startLink.position = new Vector3(body.transform.position.x, body.transform.position.y - startHeight, body.transform.position.z + startLinkOffset);
        RayCast();
        if (climbableTargetMesh != null)
        {
            endLink.position = new Vector3((dir.x + transform.localPosition.x), climbableTargetMesh.bounds.size.y + transform.position.y, (dir.z + transform.localPosition.z));
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
            Debug.Log(gameObject.name + "hit");
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
                case "Interactable":
                    //Debug.Log("Hit Interactable Object");
                    if(other.TryGetComponent<InventoryItem>(out InventoryItem invitem))
                    {
                        //Debug.Log("Hit Inventory Item.");
                        other.gameObject.SetActive(false);
                        objectiveComplete = true;
                    }
                    else if(other.TryGetComponent<Utility>(out Utility util))
                    {
                        //Debug.Log("Hit Utility.");
                        switch (target.name)
                        {
                            case "Stove":
                                target.GetComponent<Stove>().On = false;
                                target.GetComponent<Stove>().State(false);
                                objectiveComplete = true;
                                break;
                        }
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
        transform.up = dir;
    }

    public void AdjustTargetList(List<GameObject> targetList)
    {
        //Get all interactable items and the cookbook
        GameObject[] targetarray =  GameObject.FindGameObjectsWithTag("Interactable");
        targetList.AddRange(targetarray);
        targetList.Add(GameObject.FindGameObjectWithTag("CookBook"));

        //Remove items that we don't want the rats targeting in a given circumstance.
        List<GameObject> removeList = new List<GameObject> { };
        foreach (GameObject item in targetList)
        {
            switch (item.name)
            {
                case ("CookBook"):
                    break;
                
                case ("Spatula"):
                    break;

                case ("Plate"):
                    break;

                case ("Pan"):
                    break;

                case ("Stove"):
                    //Don't target stove if it's off
                    if (!item.GetComponent<Stove>().On)
                    {
                        removeList.Add(item);
                    }
                    break;

                case ("Egg"): case ("Egg(Clone)"):
                    break;

                case ("TrashCan"):
                    removeList.Add(item);
                    break;

                case ("Bacon"):
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

    public void SetTarget(List<GameObject> targetList)
    {
        target = targetList[Random.Range(0, targetList.Count)];

        Debug.Log(gameObject.name + " is targeting: " + target.name);
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
            Debug.Log(gameObject.name + " Fled");

            target = null;
            ReturnToVent();
        }
        else if (chance >= 70)
        {
            Debug.Log(gameObject.name + " Changed Target");

            //Pick new target from list
            AdjustTargetList(TargetsList);
        }
        else
        {
            Debug.Log(gameObject.name + " Kept going");
        }

        yield return new WaitForSeconds(1f);
    }

    public void Hide()
    {
        hideTime = Random.Range(minHideTimer, maxHideTimer);
        int hideindex = Random.Range(0, hidingPointsList.Length);

        hiding = true;
        agent.destination = hidingPointsList[hideindex].transform.position;
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
