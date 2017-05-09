using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazaEscapeSceneController : MonoBehaviour {

    public GameObject firstArea;
    public GameObject plaza;
    public GameObject robotEnemy;
    public Spawnpoint initialSpawn;

    private Spawnpoint[] firstAreaSpawnpoints;
    private Spawnpoint[] plazaSpawnpoints;
    private bool plazaEntered = false;
    // Use this for initialization
    void Start () {
        Messenger.AddListener(GameEvent.AREA_STARTED, SpawnInStartArea);
        Messenger.AddListener(GameEvent.PLAZA_REACHED, SpawnInPlazaArea);
        firstAreaSpawnpoints = firstArea.GetComponentsInChildren<Spawnpoint>();
        plazaSpawnpoints = plaza.GetComponentsInChildren<Spawnpoint>();
        // for testing, spawn some at start
        StartCoroutine(SpawnInitial());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.AREA_STARTED, SpawnInStartArea);
        Messenger.RemoveListener(GameEvent.PLAZA_REACHED, SpawnInPlazaArea);
    }

    private void SpawnInStartArea()
    {
        StartCoroutine(SpawnInStart());
    }
    private void SpawnInPlazaArea()
    {
        StartCoroutine(SpawnInPlaza());
    }

    IEnumerator SpawnInStart()
    {
        yield return new WaitForEndOfFrame();
        foreach (Spawnpoint s in firstAreaSpawnpoints)
        {
            s.Spawn(robotEnemy);
            s.Spawn(robotEnemy);
            s.Spawn(robotEnemy);
            s.Spawn(robotEnemy);
            s.Spawn(robotEnemy);
            s.Spawn(robotEnemy);
        }
    }
    IEnumerator SpawnInitial()
    {
        yield return new WaitForEndOfFrame();
        initialSpawn.Spawn(robotEnemy);
        initialSpawn.Spawn(robotEnemy);
        initialSpawn.Spawn(robotEnemy);
        initialSpawn.Spawn(robotEnemy);
        initialSpawn.Spawn(robotEnemy);
        initialSpawn.Spawn(robotEnemy);
        initialSpawn.Spawn(robotEnemy);
        initialSpawn.Spawn(robotEnemy);
        initialSpawn.Spawn(robotEnemy);
    }
    IEnumerator SpawnInPlaza()
    {
        if (!plazaEntered)
        {
            int spawns = 0;
            foreach (Spawnpoint s in plazaSpawnpoints)
            {
                s.Spawn(robotEnemy);
                s.Spawn(robotEnemy);
                s.Spawn(robotEnemy);
                s.Spawn(robotEnemy);
                s.Spawn(robotEnemy);
                s.Spawn(robotEnemy);
            }
            while (spawns < 5)
            {
                yield return new WaitForSeconds(30f);
                foreach (Spawnpoint s in plazaSpawnpoints)
                {
                    s.Spawn(robotEnemy);
                    s.Spawn(robotEnemy);
                    s.Spawn(robotEnemy);
                }
                spawns++;
            }
            plazaEntered = true;
        }
    }
}
