using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazaEscapeSceneController : MonoBehaviour {

    public GameObject firstArea;
    public GameObject plaza;
    public GameObject robotEnemy;
    public Spawnpoint initialSpawn;
    public ParticleSystem endLevelParticles;

    private Spawnpoint[] firstAreaSpawnpoints;
    private Spawnpoint[] plazaSpawnpoints;
    private int spawnpointsCleared = 0;
    private bool plazaEntered = false;
    // Use this for initialization
    void Start () {
        Messenger.AddListener(GameEvent.AREA_STARTED, SpawnInStartArea);
        Messenger.AddListener(GameEvent.PLAZA_REACHED, SpawnInPlazaArea);
        Messenger.AddListener(GameEvent.SPAWNPOINT_CLEARED, clearSpawnpoint);
        firstAreaSpawnpoints = firstArea.GetComponentsInChildren<Spawnpoint>();
        plazaSpawnpoints = plaza.GetComponentsInChildren<Spawnpoint>();
        // for testing, spawn some at start
        StartCoroutine(SpawnInitial());
    }
	
	// Update is called once per frame
	void Update () {
		if(spawnpointsCleared == firstAreaSpawnpoints.Length + plazaSpawnpoints.Length + 1)
        {
            Messenger.Broadcast(GameEvent.PLAZA_CLEARED);
            FindObjectOfType<UIManager>().UpdateMessage("Plaza Cleared, Get To Extraction!");
            endLevelParticles.Play();
        }
	}
    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.AREA_STARTED, SpawnInStartArea);
        Messenger.RemoveListener(GameEvent.PLAZA_REACHED, SpawnInPlazaArea);
        Messenger.RemoveListener(GameEvent.SPAWNPOINT_CLEARED, clearSpawnpoint);
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
            while (spawns < 3)
            {
                yield return new WaitForSeconds(20f);
                foreach (Spawnpoint s in plazaSpawnpoints)
                {
                    s.Spawn(robotEnemy);
                    s.Spawn(robotEnemy);
                    s.Spawn(robotEnemy);
                }
                spawns++;
            }
            Messenger.Broadcast(GameEvent.DONE_SPAWNING);
            plazaEntered = true;
        }
    }

    private void clearSpawnpoint()
    {
        spawnpointsCleared++;
    }
}
