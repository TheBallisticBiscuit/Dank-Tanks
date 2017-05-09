using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotController : MonoBehaviour {
    public float speed = 10f;
    public int damage { get; set; }
    public float lifetime = 2f;
    public IShotInformation source;
    // Use this for initialization
    void Start () {
        StartCoroutine(KillSelfInTime(lifetime));
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy")
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
            }
            else if (source != null)
            {
                source.MissedShot();
            }
            
            Destroy(this.gameObject);
        }
    }

    IEnumerator KillSelfInTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (source != null)
        {
            source.MissedShot();
        }
        Destroy(this.gameObject);
    }
}
