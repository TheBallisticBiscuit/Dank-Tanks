using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    bool CanComplete = false;
    // Use this for initialization
    void Start () {
        Messenger.AddListener(GameEvent.PLAZA_CLEARED, AllowEscape);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAZA_CLEARED, AllowEscape);
    }

    // Update is called once per frame
    void Update () {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CanComplete && other.tag == "Player")
            Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
    }

    private void OnTriggerStay(Collider other)
    {
        if (CanComplete && other.tag == "Player")
            Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
    }

    void AllowEscape() {
        CanComplete = true;
    }
}
