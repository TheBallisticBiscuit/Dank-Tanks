using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour {
    public float speed = 10f;
    public Vector3 fwd;
    public float drop = 0.5f;
    
    public GameObject explosion;

    public int damage
    {
        get; set;
    }

    // Use this for initialization
    void Start () {
        fwd = transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = ((transform.position + fwd * Time.deltaTime * speed) - transform.position).normalized;
        transform.position += fwd * Time.deltaTime * speed;//transform.forward * Time.deltaTime * speed;

        GetComponent<Rigidbody>().AddForce(Physics.gravity * GetComponent<Rigidbody>().mass * drop);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "DontTriggerExplosion") 
        {
            if(explosion)
            {
                GameObject newExplosion = Instantiate(explosion) as GameObject;
                newExplosion.GetComponent<Explosion>().damage = damage;
                newExplosion.transform.position = this.transform.position;
            }
            
            Destroy(this.gameObject);
        }
    }
}
