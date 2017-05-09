using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSceneController : MonoBehaviour {

    public GameObject spawnpointsParent;
    public GameObject robotEnemy;

    private Spawnpoint[] spawnpoints;

    // Use this for initialization
    void Start () {
        spawnpoints = spawnpointsParent.GetComponentsInChildren<Spawnpoint>();

        // for testing, spawn some at start
        foreach(Spawnpoint s in spawnpoints)
        {
            s.Spawn(robotEnemy);
            s.Spawn(robotEnemy);
        }
        StartCoroutine(TestSpawn());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator TestSpawn()
    {
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(10f);
            foreach (Spawnpoint s in spawnpoints)
            {
                s.Spawn(robotEnemy);
            }
        }
    }
}
