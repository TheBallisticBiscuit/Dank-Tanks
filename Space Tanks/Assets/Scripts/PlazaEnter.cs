using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlazaEnter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Messenger.Broadcast(GameEvent.PLAZA_REACHED);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Messenger.Broadcast(GameEvent.PLAZA_REACHED);
        }
    }
}
