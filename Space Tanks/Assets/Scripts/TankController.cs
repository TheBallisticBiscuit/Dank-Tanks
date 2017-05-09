using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour, IDamageable
{
    Rigidbody rb;

    public int startingHP = 100;
    public float speed;
    public float turnSpeed;
    public int damage = 5;
    public ParticleSystem[] fireParticles;
    public Transform[] cannonBarrelPoints;
    public GameObject cannonShotPrefab;
    public GameObject turret;
    public float recoilKnockbackTime = .5f;
    public float recoilRecoveryTime = 1.5f;
    public float recoilDelayTime = .25f;
    public float gravityModifier = 4;
    private GameManager gameManager;
    private UIManager ui;
    private WheelCollider[] wheels;

    private bool onCooldown;
    private int hp;

    private Vector3 recoilStartPos;
    private Vector3 recoilEndPos;

    private float recoilTime = 5;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<WheelCollider>();
        onCooldown = false;
        hp = startingHP;
        gameManager = FindObjectOfType<GameManager>();
        ui = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsPaused) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(FireCannon());
        }

        if (recoilTime < recoilKnockbackTime + recoilDelayTime + recoilRecoveryTime)
            recoilTime += Time.deltaTime;

        if(recoilTime < recoilKnockbackTime)
        {
            turret.transform.localPosition = Vector3.Lerp(recoilStartPos, recoilEndPos, recoilTime / recoilKnockbackTime);

        }
        //else if(recoilTime > recoilKnockbackTime && recoilTime < recoilKnockbackTime + recoilDelayTime)
        //{
        //    turret.transform.localPosition = recoilEndPos;
        //}
        else if(recoilTime > recoilKnockbackTime + recoilDelayTime && recoilTime < recoilRecoveryTime + recoilKnockbackTime + recoilDelayTime)
        {
            turret.transform.localPosition = Vector3.Lerp( recoilEndPos, recoilStartPos, (recoilTime - (recoilKnockbackTime + recoilDelayTime))/ recoilRecoveryTime);
        }
    }

    private void FixedUpdate()
    {
        if (gameManager.IsPaused) return;
        float moveDirection;
        
        moveDirection = Input.GetAxis("Vertical") * speed;

        rb.AddRelativeForce(0, 0, moveDirection);
        float turnDirection;
        turnDirection = Input.GetAxis("Horizontal") * turnSpeed;
        rb.AddRelativeTorque(0, turnDirection, 0);
        rb.AddForce(Physics.gravity * rb.mass*2 * gravityModifier);
    }

    private IEnumerator FireCannon()
    {
        if (!onCooldown)
        {
            onCooldown = true;
            StartCoroutine(CannonCooldown());
            foreach (ParticleSystem i in fireParticles)
            {
                if (!i.isPlaying)
                {
                    i.Play();
                }
            }
            yield return new WaitForSeconds(1.5f);
            foreach (Transform i in cannonBarrelPoints)
            {
                GameObject shot = Instantiate(cannonShotPrefab, i.transform.position, i.transform.rotation);
                shot.GetComponent<ShotController>().damage = damage;
                Destroy(shot, 3.0f);
            }
            recoilTime = 0;
            recoilStartPos = Vector3.zero;
            recoilEndPos = turret.transform.forward;
            //recoilEndPos.y = 0;
            recoilEndPos = recoilStartPos - (recoilEndPos.normalized * 4);
        }
        else
        {
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator CannonCooldown()
    {
        yield return new WaitForSeconds(2.0f);
        onCooldown = false;
    }

    public void TakeDamage(int amt)
    {
        hp -= amt;
        //Debug.Log("Tank damaged by " + amt);
        if (hp <= 0) Death();
        ui.UpdateHealthbar(hp, startingHP);
    }

    public void Death()
    {
        hp = 0;
        Messenger.Broadcast(GameEvent.PLAYER_DEATH);
    }

    public int HP
    {
        get { return hp; }
    }
}
