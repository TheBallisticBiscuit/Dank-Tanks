using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour {

    public float detectionRadius = 3f;
    public float waitBetweenTries = 0.5f;
    public int maxTries = 100;

    private int spawnQueue = 0;
    public int SpawnQueue { get { return spawnQueue; } }

	// Use this for initialization
	void Start () {
		
	}
	
	public void Spawn(GameObject toSpawn)
    {
        StartCoroutine(AttemptSpawn(toSpawn));
    }

    private IEnumerator AttemptSpawn(GameObject toSpawn)
    {
        spawnQueue++;
        for(int i = 0; i < maxTries; i++)
        {
            if(!Physics.CheckSphere(transform.position, detectionRadius))
            {
                Instantiate(toSpawn, transform.position, transform.rotation);
                spawnQueue--;
                yield break;
            }
            yield return new WaitForSeconds(waitBetweenTries);
        }
        spawnQueue--;
    }
}
