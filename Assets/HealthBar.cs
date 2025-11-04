using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
	public static HealthBar instance; // permet d'y accéder depuis d'autres scripts

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
    public void takeDammage(int damage)
    {
        slider.value -= damage;
    }
    // Start is called before the first frame update
    void Start()
    {
        setMaxHealth(100);
    }

    // Update is called once per frame
    void Update()
    {


    }
}
