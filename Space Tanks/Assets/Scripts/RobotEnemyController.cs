using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class RobotEnemyController : MonoBehaviour, IDamageable, IShotInformation {
    public int startingHP = 10;
    public float sightRange = 20;
    public int damage = 1;
    public Transform gunTip;
    public Transform head;
    public GameObject projectile;
    public Healthbar healthbar;

    public int HP { get { return hp; } }

    private Animator animator;
    private int hp;
    private NavMeshAgent ai;
    private Transform playerTarget;
    private bool dying = false;
    private bool missed = false;
    private TankController tank;
    enum AIState {MoveToPlayer, Attack, None };
    private AIState state = AIState.None;
    private float AIWaitTime;
    private bool AIWaiting = false;
    
	// Use this for initialization
	void Start () {
        hp = startingHP;
        ai = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerTarget = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
        healthbar.UpdateBar(hp, startingHP);
        tank = FindObjectOfType<TankController>();
        //StartCoroutine(Determine());

    }
	
	// Update is called once per frame
	void Update () {
        animator.SetFloat("Speed", ai.desiredVelocity.magnitude);
        AIWaitTime += Time.deltaTime;
        if (dying)
            return;
        if(state == AIState.None)
        {
            if (!PlayerInSight() || missed)
            {
                missed = false;
                SetMoveToPlayer();
            }
            else
            {
                Fire();
            }
            AIWaitTime = 0;
        }
        else if(state == AIState.Attack && AIWaiting)
        {
            if (AIWaitTime > 1)
                state = AIState.None;
        }
        else if (state == AIState.MoveToPlayer && AIWaiting)
        {
            if (AIWaitTime > 1.5)
                state = AIState.None;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Death();
        }
    }

    private void Fire()
    {
        ai.ResetPath();
        animator.SetBool("Moving", false);
        animator.SetBool("Firing", true);
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Firing") || animator.IsInTransition(0))
        {
            state = AIState.None;
            return;
        }
        GameObject proj = Instantiate(projectile, gunTip.position, transform.rotation);
        proj.GetComponent<EnemyShotController>().damage = damage;

        // check if will miss
        RaycastHit hit;
        //Debug.DrawLine(gunTip.position, transform.forward * sightRange);
        if (Physics.Raycast(gunTip.position, transform.forward, out hit, sightRange))
        {
            //Debug.Log(hit.transform.gameObject);
            if (hit.transform.tag == "Player")
            {
                missed = false;
            }
            else
            {
                missed = true;
            }
        }
        else
        {
            missed = true;
        }
        state = AIState.Attack;
        AIWaiting = true;
        return;
    }

    //[Task]
    //private void 
    private void SetMoveToPlayer()
    {
        animator.SetBool("Moving", true);
        animator.SetBool("Firing", false);
        ai.SetDestination(tank.transform.position);
        state = AIState.MoveToPlayer;
        AIWaiting = true;
    }
    
    //[Task]
    //private void 
    private bool PlayerInSight()
    {
        RaycastHit hit;
        if(Physics.Raycast(head.position, playerTarget.position - head.position, out hit, sightRange))
        {
            if(hit.transform.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    public void TakeDamage(int amt)
    {
        if (dying) return;
        hp -= amt;
        if (hp <= 0)
        {
            Death();
            return;
        }
        healthbar.UpdateBar(hp, startingHP);
    }

    public void Death()
    {
        hp = 0;
        healthbar.UpdateBar(hp, startingHP);
        dying = true;
        animator.enabled = false;
        ai.enabled = false;
        //GetComponent<PandaBehaviour>().enabled = false;
        healthbar.gameObject.SetActive(false);
        Rigidbody[] allRBs = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in allRBs)
        {
            rb.isKinematic = false;
        }
        Destroy(gameObject, 5f);
    }

    public void MissedShot()
    {
        if (dying) return;
        missed = true;
        //Debug.Log("Missed");
    }
}
