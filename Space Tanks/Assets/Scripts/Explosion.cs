using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    private float lerpTime = 0;
    public float maxSize = 0;
    private float minSize = 0;
    public float expansionSpeed = 1f;
    public float force = 100.0f;
    public float radius = 5.0f;
    public float upwardsModifier = 0.0f;
    public ForceMode forceMode;

    
    public int damage
    {
        get; set;
    }

    // Use this for initialization
    void Start () {
        minSize = transform.localScale.x;
        foreach (Collider col in Physics.OverlapSphere(transform.position, radius))
        {
            if (col.GetComponent<Rigidbody>() != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, col.transform.position - transform.position, out hit, Mathf.Infinity))
                {
                    if (hit.collider == col)
                    {
                        hit.rigidbody.AddExplosionForce(force, transform.position, radius, upwardsModifier, forceMode);
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        transform.localScale = new Vector3(Mathf.Lerp(minSize, maxSize, lerpTime*expansionSpeed), 
            Mathf.Lerp(minSize, maxSize, lerpTime*expansionSpeed), Mathf.Lerp(minSize, maxSize, lerpTime*expansionSpeed));
        this.GetComponent<ExplosionMat>()._alpha = Mathf.Lerp(1, 0, lerpTime*expansionSpeed);
        lerpTime += Time.deltaTime;
        if(this.GetComponent<ExplosionMat>()._alpha == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Explosion")
        {
            return;
        }

        IDamageable[] things = other.GetComponentsInChildren<IDamageable>();

        foreach(IDamageable thing in things)
        {
            thing.TakeDamage(damage);
        }
    }
}
