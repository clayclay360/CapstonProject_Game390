using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class RatScript : MonoBehaviour
{
    [Header("Variables")]
    public GameObject body;

    [Header("Target")]
    public float attackRaduis;
    public GameObject Target;
    public bool objectiveComplete;

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

    private float distanceBetweenTarget;
    private float startHeight;
    private bool linkActivated;
    private bool climbing;
    private bool attackReady;

    private NavMeshAgent agent;
    private MeshRenderer climbableTargetMesh;


    // Start is called before the first frame update
    private void Awake()
    {
        startHeight = transform.position.y;
        attackReady = true;

        //GetTarget();
        Target = GameObject.FindGameObjectWithTag("CookBook");
        agent = GetComponent<NavMeshAgent>();
        offMeshLink.GetComponent<OffMeshLink>();
        starRay.GetComponent<Transform>();
        endRay.GetComponent<Transform>();
        startLink.GetComponent<Transform>();
        endLink.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        GetAction();
        //RayCast();
        //Climbing
        DistanceBetweenTarget();
    }

    private void GetAction()
    {

        if(Target != null)
        {
            distanceBetweenTarget = Vector3.Distance(transform.position, Target.transform.position);
            agent.stoppingDistance = attackRaduis;

            if (distanceBetweenTarget > attackRaduis)
            {
                MoveToTarget();
            }
            else
            {
                //LookAt();
                Attack();
            }
        }
    }

    private void MoveToTarget()
    {
        agent.destination = Target.transform.position;
    }

    private void RayCast()
    {
        RaycastHit hit;

        if (Physics.Linecast(starRay.position, endRay.position,out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Climbable" && !climbing)
                {
                    climbableTargetMesh = hit.transform.gameObject.GetComponent<MeshRenderer>(); ;
                    Climb();
                    StartCoroutine(ClimbCoolDOwn());
                }
            }
        }
        Debug.DrawLine(starRay.position, endRay.position);
    }

    private void DistanceBetweenTarget()
    {
        if (Target != null)
        {
            float distanceBetweenTarget = Vector3.Distance(transform.position, Target.transform.position);

            if (distanceBetweenTarget < climbRaduis && !climbing)
            {
                if (transform.position.y + platformYOffset < Target.transform.position.y)
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
        Vector3 dir = Target.transform.position - transform.position;
        dir.Normalize();
        startLink.position = new Vector3(body.transform.position.x, body.transform.position.y - startHeight, body.transform.position.z + startLinkOffset);
        if (climbableTargetMesh != null)
        {
            endLink.position = new Vector3((dir.x + transform.localPosition.x), climbableTargetMesh.bounds.size.y + transform.position.y, (dir.z + transform.localPosition.z));
        }
        else
        {
            endLink.position = new Vector3((dir.x + body.transform.position.x), Target.transform.position.y + transform.position.y, (dir.z + body.transform.position.z));
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
        if (other.CompareTag(Target.tag))
        {
            Debug.Log("hit");
            collider.enabled = false;
            switch (Target.tag)
            {
                case "CookBook":
                    CookBook cookbook = other.GetComponent<CookBook>();
                    cookbook.lives--;
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
        Vector2 dir = Target.transform.position - transform.position;
        transform.up = dir;
    public void SetTarget(GameObject target)
    {
        Target = target;
    }
}
