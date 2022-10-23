using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OwnerController : MonoBehaviour
{
    public Animator animator;
    public float speed;
    public float turnSpd;
    public Transform bound;
    public Transform dest;
    public request req;

    public Rigidbody rb;
    Vector3 dirVec;
    NavMeshAgent nav;
    NavMeshPath navPath;
    Vector3 direction;
    public bool randomMovement = false;


    void OnEnable()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.updatePosition = true;
        nav.updateRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        bound.rotation = Quaternion.Euler(Vector3.zero);
        StopAllCoroutines();
        StartCoroutine("changedir");
        navPath = new NavMeshPath();
    }

    public float traceSpacing;

    void FixedUpdate()
    {
        // if (randomMovement)
        // {
        //     transform.position += dirVec * speed * 0.1f;
        //     if (transform.position.x * transform.position.x > 2500 || transform.position.z * transform.position.z > 2500)
        //     {
        //         transform.position = new Vector3(0, transform.position.y, 0);
        //     }
        //     if (dirVec != Vector3.zero)
        //     {
        //         bound.rotation = Quaternion.Lerp(bound.rotation, Quaternion.LookRotation(dirVec), Time.deltaTime * turnSpd);
        //     }
        // }
        // if (nav.destination != null)
        //     nav.CalculatePath(nav.destination, navPath);
        //animator.SetFloat(Const.Speed, rb.velocity.sqrMagnitude);
        if (rb.velocity.sqrMagnitude > 1)
        {
            animator.SetFloat("MoveSpeed", 0.5f);

        }
        else
        {
            animator.SetFloat("MoveSpeed", 0f);
            //animator.SetBool(Const.Moving, false);
        }
        if (GameManager.Instance.ingroup || GameManager.Instance.idleAgent.state != IdleAgent.States.say)
        {
            // rb.AddForce((nav.nextPosition - transform.position).normalized * speed, ForceMode.VelocityChange);
            //dash
            if (Input.GetKey(KeyCode.LeftControl))
            {
                rb.AddForce(-bound.forward * speed * 10, ForceMode.VelocityChange);

                // transform.position += transform.forward * speed * 0.5f;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.AddForce(bound.forward * speed * 10, ForceMode.VelocityChange);

                // transform.position += transform.forward * speed * 0.5f;
            }
            //move
            if (Input.GetKey(KeyCode.W))
            {
                animator.transform.localRotation = Quaternion.identity;
                rb.AddForce(bound.forward * speed, ForceMode.VelocityChange);
                // transform.position += transform.forward * speed * 0.5f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                animator.transform.localRotation = Quaternion.LookRotation(Vector3.back);
                rb.AddForce(-bound.forward * speed, ForceMode.VelocityChange);
            }

        }
        if (Input.GetKey(KeyCode.D))
        {
            bound.Rotate(Vector3.up * turnSpd * 0.1f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            bound.Rotate(Vector3.up * turnSpd * -0.1f);
        }
        if (Vector3.Magnitude(rb.velocity) > 5)
        {
            rb.velocity *= 0.9f;
        }

        if (lastpos == null || Vector3.SqrMagnitude(lastpos - transform.position) > traceSpacing)
        {
            //if (agent.energy > 0)
            makeTrace(1);

            if (queueFilled == queueSize)
            {
                waypoints.Dequeue();
                queueFilled -= 1;
            }
            waypoints.Enqueue(transform.position);
            queueFilled++;
            lastpos = transform.position;
            // if (queueFilled > 0)
            //     agent.AddReward(Vector3.SqrMagnitude(waypoints.Peek() - transform.position) / 10000);
            // agent.AddReward(agent.energy / 10);

        }

    }

    void Update()
    {
        if (GameManager.Instance.ingroup || GameManager.Instance.idleAgent.state != IdleAgent.States.say)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(req.getLocationEqual() && req.getQuestGen() && req.questType == 1){
                    GameManager.Instance.checkJump = true;
                }
                rb.AddForce(bound.up * speed * 1000, ForceMode.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                rb.AddForce(bound.up * speed * 10000, ForceMode.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                rb.AddForce(-bound.up * speed * 10000, ForceMode.Impulse);
            }
        }
    }
    IEnumerator changedir()
    {
        while (true)
        {
            if (randomMovement)
            {
                float walkRadius = Random.Range(20f, 50f);

                Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
                Vector3 finalPosition = hit.position;
                nav.SetDestination(finalPosition);
                dest.position = finalPosition + transform.up;
                // dirVec = new Vector3(Random.insideUnitSphere.normalized.x, 0, Random.insideUnitSphere.normalized.z);
                yield return new WaitForSecondsRealtime(Random.Range(10, 15));
            }
            yield return null;
        }
    }

    public void goTo(Vector3 pos)
    {
        nav.SetDestination(pos);
    }

    //NOTE Trace 관련 
    public GameObject tracePrefab;
    public Transform traces;
    public Queue<Vector3> waypoints;
    public int queueSize;
    int queueFilled = 0;
    Vector3 lastpos;

    private void Start()
    {
        waypoints = new Queue<Vector3>();
    }

    public void makeTrace(int count)
    {
        //Debug.Log("maketrace");
        for (int i = 0; i < count; i++)
            Instantiate(tracePrefab, new Vector3(transform.position.x, 1f, transform.position.z), Quaternion.identity, traces);
    }

    public void resetQ()
    {
        waypoints.Clear();
        queueFilled = 0;
    }
}
