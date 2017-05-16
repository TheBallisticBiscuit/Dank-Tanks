using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
	public float sensitivityX = 5;
	public float sensitivityY = 5;
	public float rotationX = 0;
	public float rotationY = 0;
	public float minimumVertical = -45;
	public float maximumVertical = 45;
    public float maxRotationSpeed = 5;
    private GameManager gameManager;
    private float overflowDelta = 0;

    // Use this for initialization
    void Start () {
        gameManager = FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameManager.IsPaused) return;
        float rotationXdelta = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        rotationX -= Mathf.Clamp(rotationXdelta, -maxRotationSpeed, maxRotationSpeed);
		rotationX = Mathf.Clamp (rotationX, minimumVertical, maximumVertical);

		float tmp = Input.GetAxis ("Mouse X") * sensitivityX * Time.deltaTime + overflowDelta/1.4f;
        float delta = Mathf.Clamp(tmp, -maxRotationSpeed, maxRotationSpeed);
        overflowDelta = tmp - delta;

		float rotationY = transform.localEulerAngles.y + delta;
		this.transform.localEulerAngles = new Vector3 (rotationX, rotationY, 0);
	}
}
