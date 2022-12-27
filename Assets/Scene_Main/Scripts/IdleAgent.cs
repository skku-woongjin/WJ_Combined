using UnityEngine;
using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;
using UnityEngine.AI;
public class IdleAgent : Agent
{
    Rigidbody m_AgentRb;
    public Material blueMat;
    public Material purpleMat;
    public Material redMat;
    public Material greenMat;
    public Material yellowMat;
    public bool showColor;
    public enum States
    {
        rand,
        inte,
        stop,
        bound,
        avoid,
        outbound,
        say,
        enterGroup,
        lead
    }

    public enum Loc
    {
        safe,
        bound,
        outbound

    }

    public States state;
    public Loc loc;
    public bool colliding = false;
    public bool interested;
    public float turnSpeed = 300;
    public float moveSpeed = 2;
    public float rew;

    EnvironmentParameters m_ResetParams;

    public override void Initialize()
    {
        nav = GetComponent<NavMeshAgent>();
        m_AgentRb = GetComponent<Rigidbody>();
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        inteStop = stop(true);
        ChangeDir = changedir();
        miauCouroutine = playInteSound();
        rend = gameObject.GetComponentInChildren<Renderer>();
        Physics.IgnoreCollision(GetComponent<Collider>(), owner.GetComponent<Collider>());
    }
    public OwnerController ownerController;
    IEnumerator ChangeDir;
    IEnumerator miauCouroutine;
    public override void OnEpisodeBegin()
    {
        Physics.IgnoreLayerCollision(3, 8);
        StopAllCoroutines();
        transform.localPosition = new Vector3(-3.5f, 1.02f, 3.8f);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        Random.InitState(50);
        StartCoroutine(ChangeDir);
        ownerController.enabled = false;
        owner.position = new Vector3(0, 1.55f, 1.84f);
        ownerController.enabled = true;
        m_AgentRb.velocity = Vector3.zero;
        gameObject.GetComponentInChildren<Renderer>().material = blueMat;
        StartCoroutine("StopStop");
        state = States.rand;
        loc = Loc.safe;
        //lead(leadobj);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        var localVelocity = transform.InverseTransformDirection(m_AgentRb.velocity);
        sensor.AddObservation((int)state - 4);
        sensor.AddObservation(localVelocity.x);
        sensor.AddObservation(localVelocity.z);
        sensor.AddObservation((owner.position.x - transform.position.x) / 50f);
        sensor.AddObservation((owner.position.z - transform.position.z) / 50f);
        // sensor.AddObservation(owner.position.x);
        // sensor.AddObservation(owner.position.z);
    }
    public Transform owner;
    Vector3 dirVec;
    float autoTurnSpeed = 150;
    float autoMoveSpeed = 0.15f;
    IEnumerator inteStop;
    public Vector3 removY(Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }
    NavMeshAgent nav;
    public void MoveAgent(ActionBuffers actionBuffers) // 매 프레임 호출 
    {
        if (state == States.say || state == States.stop || decel)
            return;

        //ANCHOR ENTERGROUP
        if (state == States.enterGroup)
        {
            if (nav.enabled == true)
            {
                if (checkNavEnd(nav))
                {
                    nav.enabled = false;
                }
                return;
            }
            else
            {
                // Debug.Log(Vector3.Distance(GameManager.Instance.curGroup.transform.position, transform.position));
                if (Vector3.Distance(GameManager.Instance.curGroup.transform.position, transform.position) > 1.5f)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(removY(GameManager.Instance.curGroup.transform.position - transform.position)), Time.deltaTime * autoTurnSpeed * 0.1f);
                    if (m_AgentRb.velocity.sqrMagnitude > 3f) //최대속도 설정
                    {
                        m_AgentRb.velocity *= 0.95f;
                    }
                }
                else
                {
                    state = States.rand;
                }
            }

        }
        switch (state)
        {
            //ANCHOR RAND
            case States.rand:
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirVec), Time.deltaTime * autoTurnSpeed * 0.005f);
                if (m_AgentRb.velocity.sqrMagnitude > 1.3f) //최대속도 설정
                {
                    m_AgentRb.velocity *= 0.95f;
                }
                break;
            //ANCHOR INTE
            case States.inte:
                if (nav.enabled == true)
                {
                    if (checkNavEnd(nav))
                    {
                        nav.enabled = false;
                        StartCoroutine(inteStop);
                        return;
                    }
                }
                else
                    nav.enabled = true;
                if (interestingObj != null)
                    nav.SetDestination(removY(interestingObj.GetComponent<Collider>().ClosestPoint(transform.position)));
                return;
            //ANCHOR AVOID(agent)
            case States.avoid:
                float rot = Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
                Vector3 rotateDir = -transform.up * rot;
                if (rotateDir.sqrMagnitude > 0.1)
                {
                    transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);
                }
                m_AgentRb.AddForce(transform.forward * moveSpeed * 0.5f, ForceMode.VelocityChange);
                if (m_AgentRb.velocity.sqrMagnitude > 2f) //최대속도 설정
                {
                    m_AgentRb.velocity *= 0.95f;
                }
                if (m_AgentRb.velocity.sqrMagnitude < 1f) //최소속도 설정
                {
                    m_AgentRb.velocity *= 1.05f;
                }
                // }
                break;
            //ANCHOR BOUND
            case States.bound:
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(removY(owner.position - transform.position)), Time.deltaTime * turnSpeed * 0.005f);
                m_AgentRb.AddForce(transform.forward * autoMoveSpeed * 0.1f, ForceMode.VelocityChange);
                if (m_AgentRb.velocity.sqrMagnitude > 1f) //최대속도 설정
                {
                    m_AgentRb.velocity *= 0.95f;
                }
                // if (m_AgentRb.velocity.sqrMagnitude < 0.5f) //최소속도 설정
                // {
                //     m_AgentRb.velocity *= 1.05f;
                // }
                break;
        }
        //ANCHOR OUTBOUND
        if (state == States.outbound)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(Navpos + owner.position, out hit, 5, 1);
            Vector3 finalPosition = hit.position;
            if (nav.enabled == true)
            {
                if (checkNavEnd(nav))
                {
                    nav.enabled = false;
                    dirVec = transform.forward;
                    state = States.rand;
                    if (interested)
                    {
                        interest();
                    }
                    setMat();
                    return;
                }
            }
            else
                nav.enabled = true;
            nav.SetDestination(finalPosition);
            //capsule.position = finalPosition;
        }
        else if (state == States.lead)  //ANCHOR LEAD
        {
            if (Vector3.SqrMagnitude(transform.position - owner.position) > 300)
            {
                state = States.outbound;
                endlead();
                return;
            }
            else if (Vector3.SqrMagnitude(transform.position - owner.position) > 60)
            {
                nav.speed = 0;
            }
            else
            {
                nav.speed = 5;
            }
            navIcon.transform.rotation = Quaternion.LookRotation(navIcon.transform.position - GameManager.Instance.cam.position);
            if (checkNavEnd(nav) || GameManager.Instance.curQuest == -1)
            {
                endlead();
            }
        }
        else
        {
            if (state != States.stop && state != States.say)
                m_AgentRb.AddForce(transform.forward * autoMoveSpeed * 2.4f, ForceMode.VelocityChange);
        }

    }
    bool checkNavEnd(NavMeshAgent mNavMeshAgent)
    {
        if (!mNavMeshAgent.pathPending)
        {
            if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance)
            {

                if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude <= 0.1f)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public override void OnActionReceived(ActionBuffers actionBuffers) { MoveAgent(actionBuffers); }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //ANCHOR AVOID
        var continuousActionsOut = actionsOut.ContinuousActions;
        if (state == States.avoid)
        {
            if (obstacle == null)
                endObst();
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(removY(-obstacle.position + transform.position)), Time.deltaTime * turnSpeed * 0.01f);
            }
        }
    }

    //NOTE collision
    #region collision
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("ground"))
        {
            if (interestingObj != null && collision.collider.gameObject == interestingObj.gameObject)
            {
                return;
            }
            colliding = true;
            obstacle = collision.transform;
            ObstAgent(obstacle);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        colliding = false;
        if (obstacle != null && other.gameObject == obstacle.gameObject)
        {
            endObst();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("bound"))
        {
            loc = Loc.outbound;
            OutBoundAgent();
        }
        else if (other.gameObject.CompareTag("aibound2"))
        {
            loc = Loc.bound;
            BoundAgent();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bound"))
        {
            loc = Loc.bound;
        }
        else if (other.gameObject.CompareTag("aibound2"))
        {
            loc = Loc.safe;
            if (state == States.bound)
            {
                endBoundAgent();
            }
        }
    }
    #endregion
    Renderer rend;
    void setMat()
    {
        if (!showColor)
            return;
        if (state != States.outbound)
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }
        switch (state)
        {
            case States.rand:
                rend.material = blueMat;
                break;
            case States.inte:
                rend.material = greenMat;
                break;
            case States.avoid:
                rend.material = yellowMat;
                break;
            case States.bound:
                rend.material = purpleMat;
                break;
            case States.outbound:
                rend.material = redMat;
                break;
        }
    }

    //NOTE entergroup
    // bool entered;
    public void enterGroup()
    {
        if (interested)
            endInterest();
        nav.enabled = true;
        NavMeshHit hit;
        NavMesh.SamplePosition(GameManager.Instance.curGroup.GetComponent<Collider>().ClosestPoint(transform.position), out hit, 100, 1);
        Vector3 finalPosition = hit.position;
        nav.stoppingDistance = 1f;
        state = States.enterGroup;
        if (GameManager.Instance.curGroup != null)
            nav.SetDestination(removY(finalPosition));

    }
    //NOTE interest
    #region interest
    public Transform interestingObj;
    public void interest()
    {
        // if (state == States.enterGroup) return;
        // interested = true;
        // if (!miauing)
        // {
        //     miauCouroutine = playInteSound();
        //     StartCoroutine(miauCouroutine);
        // }
        // if (state <= States.inte)
        // {
        //     state = States.inte;
        //     setMat();
        // }
        // if (nav.enabled && interestingObj != null)
        // {
        //     nav.SetDestination(interestingObj.position);
        //     nav.stoppingDistance = 0.5f;
        //     state = States.inte;
        //     setMat();
        // }
    }
    public void endInterest()
    {
        interested = false;
        if (loc == Loc.outbound)
            OutBoundAgent();
        else if (obstacle != null && obstacle.gameObject == interestingObj.gameObject)
        {
            ObstAgent(obstacle);
        }
        if (interestingObj != null)
            interestingObj.tag = "Obstacle";
        interestingObj = null;
        nav.enabled = false;
        setMat();
    }

    #endregion
    //NOTE say
    #region say
    public GameObject QuoteCanv;
    public void say()
    {
        nav.enabled = false;
        interested = false;
        stopStart();
        transform.rotation = Quaternion.LookRotation(-removY(transform.position - GameManager.Instance.owner.position));
        // QuoteCanv.transform.rotation = Quaternion.LookRotation(QuoteCanv.transform.position - GameManager.Instance.cam.position);
        GetComponent<SaySomething>().say("나쁜 말을 하고 있는 것 같아..\n 다른 데 가자!");
        Invoke("showOkNo", 1f);
        state = States.say;
        rend.material = redMat;
    }
    void showOkNo()
    {
        GameManager.Instance.okno.SetActive(true);
    }

    void showOkNoQuest()
    {
        GameManager.Instance.oknoQuest.SetActive(true);
    }

    public void endSay()
    {
        state = States.rand;
        if (loc == Loc.outbound)
        {
            OutBoundAgent();
        }
        if (interested)
        {
            interest();
        }
        setMat();
        rend.material = blueMat;
    }

    public void makeBlue()
    {
        rend.material = blueMat;
    }
    #endregion

    //NOTE bound
    #region boundAgent
    void BoundAgent()
    {
        if (state == States.enterGroup || state == States.say || state == States.lead)
            return;
        if (interested) return;
        if (state <= States.bound || state == States.outbound)
        {
            if (state == States.stop || decel)
            {
                stopEnd();
                return;
            }
            state = States.bound;
            setMat();
        }
        dirVec = transform.forward;
    }
    void endBoundAgent()
    {
        state = States.rand;
        if (interested)
        {
            interest();
        }
        setMat();
    }
    #endregion

    //NOTE outbound
    #region outboundAgent
    Vector3 Navpos;
    void OutBoundAgent()
    {
        if (state == States.say || state == States.lead) return;
        if (GameManager.Instance.ingroup && !GameManager.Instance.curGroup.isbad)
        {
            enterGroup();
            return;
        }
        if (interested) return;
        Vector2 Nav2D = Random.insideUnitCircle;
        Navpos = Random.Range(0.5f, 1.8f) * new Vector3(Nav2D.x, 0, Nav2D.y) + owner.forward * 2;
        nav.stoppingDistance = 2;
        nav.speed = 8;
        if (state == States.stop || decel)
        {
            stopEnd();
        }
        state = States.outbound;

        setMat();
    }
    #endregion

    //NOTE avoid
    #region avoid 
    public Transform obstacle;
    public void ObstAgent(Transform obs)
    {
        if (state == States.lead || state == States.outbound || state == States.say || state == States.enterGroup)
        {
            return;
        }
        if (state == States.avoid || decel)
        {
            stopEnd();
        }
        if (nav.enabled)
            nav.enabled = false;
        obstacle = obs;
        state = States.avoid;
        setMat();
    }
    public void endObst()
    {
        if (state == States.lead || state == States.say || state == States.enterGroup)
        {
            return;
        }
        state = States.rand;
        obstacle = null;
        if (loc == Loc.outbound)
        {
            OutBoundAgent();
        }
        if (interested)
        {
            interest();
        }
        setMat();
    }
    #endregion
    //NOTE update
    #region update


    void FixedUpdate()
    {
        // if (start)
        // {
        //     Debug.Log(state);
        // }
        rew = GetCumulativeReward();
        if (loc == Loc.outbound && state != States.outbound)
        {
            OutBoundAgent();
        }
        if (colliding)
        {
            AddReward(-0.0002f);
        }
        if (state == States.outbound || state == States.avoid)
        {
            RequestDecision();
        }
        else
        {
            AddReward(0.0002f);
        }
        RequestAction();
        // if (!nav.enabled && state != States.say && Vector3.SqrMagnitude(owner.position - transform.position) > 500)
        // {
        //     OutBoundAgent();
        // }
    }
    void setRandDir()
    {
        dirVec = new Vector3(Random.insideUnitSphere.normalized.x, 0, Random.insideUnitSphere.normalized.z);
    }
    IEnumerator changedir()
    {
        while (true)
        {
            if (state == States.rand)
            {
                setRandDir();
            }
            autoMoveSpeed = Random.Range(0.1f, moveSpeed);
            autoTurnSpeed = Random.Range(50, turnSpeed);
            yield return new WaitForSecondsRealtime(Random.Range(1, 8));
        }
    }
    #endregion
    //NOTE stop
    #region stop
    public bool decel;
    IEnumerator stop(bool inte)
    {
        if (nav.enabled)
            nav.enabled = false;
        StopCoroutine(ChangeDir);
        ChangeDir = changedir();
        decel = true;
        while (m_AgentRb.velocity.sqrMagnitude > 0.001f)
        {
            autoMoveSpeed *= 0.99f;
            yield return new WaitForFixedUpdate();
        }
        decel = false;
        stopStart();
        if (inte)
        {
            if (interestingObj != null)
                transform.rotation = Quaternion.LookRotation(removY(interestingObj.position - transform.position));
            yield return new WaitForSecondsRealtime(Random.Range(3f, 5f));
        }
        else
            yield return new WaitForSecondsRealtime(Random.Range(5f, 30));
        stopEnd();
        if (inte)
        {
            endInterest();
            StopCoroutine(miauCouroutine);

        }
    }
    IEnumerator JustStop;
    IEnumerator StopStop()
    {
        while (true)
        {
            if (state != States.avoid && !decel)
            {
                yield return new WaitForSecondsRealtime(Random.Range(4.5f, 7));
                JustStop = stop(false);
                if (state == States.rand)
                    StartCoroutine(JustStop);
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public void stopStart()
    {
        if (Random.Range(0, 3) < 1)
        {
            transform.rotation = Quaternion.LookRotation(removY(owner.position - transform.position));
        }
        state = States.stop;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    void stopEnd()
    {
        if (state == States.stop)
            state = States.rand;
        decel = false;
        if (inteStop != null)
            StopCoroutine(inteStop);
        if (JustStop != null)
            StopCoroutine(JustStop);
        inteStop = stop(true);
        StartCoroutine(ChangeDir);
    }
    #endregion

    //NOTE sound
    #region playsound
    public AudioSource angrySound;
    public AudioSource inteSound;

    void playAngrySound()
    {
        angrySound.Play();
    }

    IEnumerator playInteSound()
    {

        while (interested)
        {
            inteSound.Play();
            yield return new WaitForSecondsRealtime(Random.Range(0.6f, 3f));
        }

    }

    #endregion


    //NOTE lead 
    #region lead
    public Transform leadobj;
    public GameObject navIcon;

    public void lead()
    {
        GetComponent<SaySomething>().say("내가 안내해줄게!");
        Invoke("lead2", 2f);
    }
    public void lead2()
    {
        GetComponent<SaySomething>().bubble.SetActive(false);
        navIcon.SetActive(true);
        nav.stoppingDistance = 5;
        NavMeshHit hit;

        NavMesh.SamplePosition(removY(GameManager.Instance.destinations[GameManager.Instance.curQuest + 1].position), out hit, 5, 1);

        Vector3 finalPosition = hit.position;

        nav.enabled = true;
        nav.SetDestination(hit.position);
        state = States.lead;
    }

    public void endlead()
    {
        state = States.rand;
        navIcon.SetActive(false);
        interested = false;
        if (loc == Loc.outbound)
            OutBoundAgent();
        leadobj = null;

    }
    #endregion
}