using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private Transform flagTarget;
	private GameObject player;
	private NavMeshAgent agent;

	private readonly float focusFlagRadius = 10f;
	private readonly float chaseRadius = 10f;
	private readonly float attackRadius = 2f;
	private readonly float attackCooldown = 1.2f;
	private readonly int playerDamage = 10;
	private readonly string playerTag = "Player";
	private readonly float maxHealth = 20f;
    
    private float currentHealth;
    enum State { ToFlag, Chase, Attack }
	private State state = State.ToFlag;

	public void Init(Transform flagTargetParam)
	{
		currentHealth = maxHealth;
		agent = GetComponent<NavMeshAgent>();
		player = PlayerController.instance?.gameObject;
		flagTarget = flagTargetParam;
		if (agent == null || flagTarget == null || player == null || transform == null)
		{
			Debug.LogError("Enemy initialization error: missing components or references.");
			Debug.LogError("Agent: " + (agent == null) + ", FlagTarget: " + (flagTarget == null) + ", Player: " + (player == null));
			enabled = false;
		}
		else
		{
			enabled = true;
		}
	}

	void Update()
	{
		if (flagTarget == null)
		{
			Debug.LogError("Flag target is null. Enemy cannot operate.");
			return;
		}

		if (player == null)
		{
			Debug.Log("Player reference lost. Attempting to find player...");
			player = GameObject.FindWithTag(playerTag);
			if (player == null) return;
		}

		float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

		switch (state)
		{
			case State.ToFlag:
				if (!agent.pathPending) agent.SetDestination(flagTarget.position);
				if (distToPlayer <= chaseRadius) state = State.Chase;
				break;

			case State.Chase:
				agent.isStopped = false;
				agent.SetDestination(player.transform.position);



				// close enough -> attack
				if (distToPlayer <= attackRadius)
				{
					agent.isStopped = true;
					state = State.Attack;
				}
				// player too far or flag too close -> go back to flag
				else
				{
					float distToFlag = Vector3.Distance(transform.position, flagTarget.position);
					if (distToPlayer > chaseRadius || distToFlag < focusFlagRadius)
					{
						state = State.ToFlag;
						agent.isStopped = false;
					}
				}
				break;

			case State.Attack:
				// face player (only rotate on Y)
				Vector3 lookPos = player.transform.position;
				lookPos.y = transform.position.y;
				transform.LookAt(lookPos);

				/*
				if (Time.time - lastAttackTime >= attackCooldown)
				{
					lastAttackTime = Time.time;
					AttackPlayer();
				}
				*/
				AttackPlayer();

				// if player moves out of attack range, chase again
				if (distToPlayer > attackRadius + 0.5f)
				{
					state = State.Chase;
					agent.isStopped = false;
				}
				break;
		}
	}

	public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(gameObject.name + " a pris " + amount + " dégâts. PV restants : " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " est mort !");

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(100);
        }

        Destroy(gameObject);
    }

	void AttackPlayer()
	{
		PlayerController playerController = player.GetComponent<PlayerController>();
		if (playerController != null)
		{
			playerController.TakeDamage(playerDamage);
			Debug.Log(gameObject.name + " attaque le joueur ! Dégâts infligés : " + playerDamage + ".");
		}
	}
}
