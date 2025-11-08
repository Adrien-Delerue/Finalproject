using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectMob : MonoBehaviour
{
    private float detectionRadius = 2.5f;
    private string mobTag = "Mob";

    public AudioSource backgroundMusic;
    public AudioSource mobOnFlagMusic;

    public GameController gameController;

    private int currentCount = 0;
    private int oldCount = 0; //number of ennemy on the previous frame

    private bool isGonnaDie = false;

    public void Init()
    {
        gameController = GetComponent<GameController>();
    }
    void Update()
    {
        DetectMobFlag();
        if(isGonnaDie & mobOnFlagMusic.time==mobOnFlagMusic.clip.length)gameController.GameOver();
    }

    void DetectMobFlag()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        currentCount = 0;

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag(mobTag))
            {
                currentCount++;   
            }
        }
        if (currentCount != oldCount)
        {
            if (oldCount == 0 && currentCount > 0)
            {
                EnnemyEnterZone();
            }
            else if (currentCount == 0 && oldCount>0) 
            {
                EnnemyLeaveZone();
            }
                oldCount = currentCount;
        }

    }

    void EnnemyEnterZone()
    {
        backgroundMusic.Stop();
        mobOnFlagMusic.Play();
        isGonnaDie = true;
    }

    void EnnemyLeaveZone()
    {
        backgroundMusic.Play();
        mobOnFlagMusic.Stop();
        isGonnaDie = false;
    }

  
}
