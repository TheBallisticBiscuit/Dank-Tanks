﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{

    public float detectionRadius = 3f;
    public float waitBetweenTries = 0.5f;
    public int maxTries = 100;
    private bool cleared = false;
    private int spawnQueue = 0;
    public int SpawnQueue { get { return spawnQueue; } }

    public List<GameObject> spawned;

    // Use this for initialization
    void Start()
    {
        Messenger.AddListener(GameEvent.DONE_SPAWNING, checkForClear);
    }

    public void Spawn(GameObject toSpawn)
    {
        StartCoroutine(AttemptSpawn(toSpawn));
    }

    private IEnumerator AttemptSpawn(GameObject toSpawn)
    {
        spawnQueue++;
        for (int i = 0; i < maxTries; i++)
        {
            if (!Physics.CheckSphere(transform.position, detectionRadius))
            {
                spawned.Add(Instantiate(toSpawn, transform.position, transform.rotation) as GameObject);
                spawnQueue--;
                yield break;
            }
            yield return new WaitForSeconds(waitBetweenTries);
        }
        spawnQueue--;
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.DONE_SPAWNING, checkForClear);
    }

    private void checkForClear()
    {
        if (!cleared)
        {
            StartCoroutine(clearSpawnpoint());
        }
    }

    private IEnumerator clearSpawnpoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i < spawned.Count; i++)
            {
                if (spawned[i] == null)
                {
                    spawned.RemoveAt(i);
                }
            }
            if (spawned.Count == 0)
            {
                Messenger.Broadcast(GameEvent.SPAWNPOINT_CLEARED);
                if (!cleared)
                {
                    Debug.Log("Spawnpoint cleared!");
                }
                else
                {
                    Debug.Log("error...");
                }
                cleared = true;
                break;
            }

        }
    }
}
