using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class RatScript : MonoBehaviour
{
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

    private float distanceBetweenTarget;
    private float startHeight;
    private bool linkActivated;
    private bool climbing;

    private NavMeshAgent agent;
    private MeshRenderer climbableTargetMesh;


    // Start is called before the first frame update
    private void Awake()
    {
        startHeight = transform.position.y;

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

            if (distanceBetweenTarget > attackRaduis)
            {
                MoveToTarget();
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
        float distanceBetweenTarget = Vector3.Distance(transform.position, Target.transform.position);
        
        if(distanceBetweenTarget < climbRaduis)
        {
            if(transform.position.y + platformYOffset < Target.transform.position.y)
            {
                Climb();
                StartCoroutine(ClimbCoolDOwn());
            }
        }
    }

    private void Climb()
    {
        Vector3 dir = Target.transform.position - transform.position;
        dir.Normalize();
        startLink.position = new Vector3(transform.localPosition.x, transform.position.y - startHeight, transform.localPosition.z + startLinkOffset);
        if (climbableTargetMesh != null)
        {
            endLink.position = new Vector3((dir.x + transform.localPosition.x), climbableTargetMesh.bounds.size.y + transform.position.y, (dir.z + transform.localPosition.z));
        }
        else
        {
            endLink.position = new Vector3((dir.x + transform.localPosition.x), Target.transform.position.y + transform.position.y, (dir.z + transform.localPosition.z));
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

    public void SetTarget(GameObject target)
    {
        Target = target;
    }
}
