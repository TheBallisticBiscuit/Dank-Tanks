using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour {

    public RectTransform healthbar;
    private int maxHealth;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
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
