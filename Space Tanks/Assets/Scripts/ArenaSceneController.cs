using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSceneController : MonoBehaviour {

    public GameObject spawnpointsParent;
    public GameObject robotEnemy;

    public int baseAmountOfEnemies = 10;
    public int waveEnemyMultiplier = 5;

    private UIManager ui;
    private Spawnpoint[] spawnpoints;
    private int wave = 0;
    private int spawnpointCounter = 0;

    // Use this for initialization
    void Start () {
        spawnpoints = spawnpointsParent.GetComponentsInChildren<Spawnpoint>();
        ui = FindObjectOfType<UIManager>();

        Messenger.AddListener(GameEvent.PLAYER_DEATH, LevelOver);
        
        StartCoroutine(WaveControl());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_DEATH, LevelOver);
    }

    void SpawnEnemies(int amt)
    {
        while(amt > 0)
        {
            spawnpoints[spawnpointCounter++].Spawn(robotEnemy);
            if (spawnpointCounter >= spawnpoints.Length) spawnpointCounter = 0;
            amt--;
        }
    }

    void LevelOver()
    {
        StopAllCoroutines();
        ui.UpdateMessage("");
    }

    IEnumerator WaveControl()
    {
        ui.UpdateMessage("Survive the waves of enemies!");
        yield return new WaitForSeconds(4f);
        
        while(true)
        {
            wave++;
            ui.UpdateMessage("Wave " + wave + " spawning");
            int totalEnemies = baseAmountOfEnemies + wave * waveEnemyMultiplier;
            SpawnEnemies(totalEnemies);
            yield return new WaitForSeconds(3f);
            while(true)
            {
                int remaining = FindObjectsOfType<RobotEnemyController>().Length;
                ui.UpdateMessage("Wave " + wave + ": " + remaining + " of " +totalEnemies + " remaining");
                if (remaining < 1) break;
                yield return new WaitForSeconds(1f);
            }
            ui.UpdateMessage("Wave Complete!");
            yield return new WaitForSeconds(4f);
        }
    }
}
