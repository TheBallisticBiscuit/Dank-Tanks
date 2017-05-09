using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour {

    public RectTransform healthbar;
    private int maxHealth;
    private int currentHealth;
    private Camera mainCam;

    private void Start()
    {
        currentHealth = maxHealth;
        mainCam = Camera.main;
    }

    void Update()
    {
        transform.LookAt(mainCam.transform);
    }

    public void UpdateBar(int current, int max)
    {
        currentHealth = current;
        maxHealth = max;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        healthbar.sizeDelta = new Vector2(currentHealth*100/max, healthbar.sizeDelta.y);
    }
}
