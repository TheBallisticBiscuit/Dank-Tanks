using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneEnemyController : MonoBehaviour, IDamageable {
    private int hP = 10;

    public int HP
    {
        get { return hP; }
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<NavMeshAgent>().destination = FindObjectOfType<TankController>().transform.position;
    }

    public void Death()
    {
        throw new NotImplementedException();
    }

    public void TakeDamage(int amt)
    {
        hP -= amt;
        if (HP <= 0) Death();
    }
}
