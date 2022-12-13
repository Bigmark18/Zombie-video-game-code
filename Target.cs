using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Target : MonoBehaviour
{
	[Header("Zombie Health and Damage")]
	private float zombieHealth = 100f;
    public float presentHealth;
	public float giveDamage = 5f;
	public HealthBar healthBar;
	
	[Header("Zombie Things")]
	public NavMeshAgent zombieagent;
	public Camera AttackRaycastArea;
	public Transform LookPoint;
	public Transform playerBody;
	public LayerMask PlayerLayer;

	[Header("Zombie Guarding Var")]
	public GameObject[] walkPoints;
	int currentZombiePosition = 0;
	public float zombieSpeed;
	float walkingpointRadius = 2;

	[Header("Zombie Attack Var")]
	public float timeBtAttack;
	bool perviouslyAttack;

	[Header("Zombie Animation")]
	public Animator anime;

	[Header("Zombie mood/states")]
	public float visionRadius;
	public float attackingRadius;
	public bool playerInvisionRadius;
	public bool playerInattackingRadius;

	private void Awake()
	{
		presentHealth = zombieHealth;
		healthBar.GiveFullHealth(zombieHealth);
		zombieagent = GetComponent<NavMeshAgent>();
	}
	
	private void Update()
	{
		playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
		playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

		if(!playerInvisionRadius && !playerInattackingRadius) Guard();
		if(playerInvisionRadius && !playerInattackingRadius) Pursueplayer();
		if(playerInvisionRadius && playerInattackingRadius) AttackPlayer();

	}

	private void Guard()
	{
		if(Vector3.Distance(walkPoints[currentZombiePosition].transform.position, transform.position) < walkingpointRadius)
		{
			currentZombiePosition = Random.Range(0, walkPoints.Length);
			if(currentZombiePosition >= walkPoints.Length)
			{
				currentZombiePosition = 0;
			}
		}
		transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentZombiePosition].transform.position, Time.deltaTime * zombieSpeed);

		//change zombie facing
		transform.LookAt(walkPoints[currentZombiePosition].transform.position);

	}
	private void Pursueplayer()
	{
		if(zombieagent.SetDestination(playerBody.position))
		{
			//animations
			 anime.SetBool("Walking", false);
			 anime.SetBool("Running", true);
			 anime.SetBool("Attacking", false);
			 anime.SetBool("Died", false);
		}
		else
		{
			 anime.SetBool("Walking", false);
			 anime.SetBool("Running", false);
			 anime.SetBool("Attacking", false);
			 anime.SetBool("Died", true);
		}
	}
	private void AttackPlayer()
	{	
		zombieagent.SetDestination(transform.position);
		transform.LookAt(LookPoint);
		if(!perviouslyAttack)
		{
			RaycastHit hitInfo;
			if(Physics.Raycast(AttackRaycastArea.transform.position, AttackRaycastArea.transform.forward, out hitInfo, attackingRadius))
			{
				Debug.Log("Attacking" + hitInfo.transform.name);

				PlayerScript playerBody = hitInfo.transform.GetComponent<PlayerScript>();

				if(playerBody != null)
				{
					playerBody.playerHitDamage(giveDamage);
				}
			 anime.SetBool("Walking", false);
			 anime.SetBool("Running", false);
			 anime.SetBool("Attacking", true);
			 anime.SetBool("Died", false);
			}
			perviouslyAttack = true;
			Invoke(nameof(ActiveAttacking), timeBtAttack);
		}
	}

	private void ActiveAttacking()
	{
		perviouslyAttack = false;
	}
  	 public void zombieHitDamage(float takeDamage)
	 {
		presentHealth -= takeDamage;
		healthBar.SetHealth(presentHealth);

		if(presentHealth <= 0)
		{
			 anime.SetBool("Walking", false);
			 anime.SetBool("Running", false);
			 anime.SetBool("Attacking", false);
			 anime.SetBool("Died", true);
			 zombieDie();
		}
	 }
	
	private void zombieDie()
	{
		zombieagent.SetDestination(transform.position);
		zombieSpeed = 0f;
		attackingRadius = 0f;
		visionRadius = 0f;
		playerInattackingRadius = false;
		playerInvisionRadius = false;
		Object.Destroy(gameObject, 1.8f);
	}
	
	public void takeDamage (float amount)
    {
		presentHealth -= amount;
		if (presentHealth <= 0f)
        {
			anime.SetBool("Died", true);
        }
		

    }
	void Die()
    {
		Destroy(gameObject);
    }



}
