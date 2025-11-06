using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] public Slider slider;
	public static HealthBar instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void setHealth(int health)
    {
        slider.value = health;
    }
    public void giveHP(int damage)
    {
        slider.value += damage;
    }

    public void takeDammage(int damage)
    {
        slider.value -= damage;
    }

    void Start()
    {
        setMaxHealth(100);
    }
}
