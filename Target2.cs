using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target2 : MonoBehaviour
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
	
	public float zombieSpeed;


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
		//healthBar.GiveFullHealth(zombieHealth);
		zombieagent = GetComponent<NavMeshAgent>();
	}
	
	private void Update()
	{
		playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
		playerInattackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

		if(!playerInvisionRadius && !playerInattackingRadius) Idle();
		if(playerInvisionRadius && !playerInattackingRadius) Pursueplayer();
		if(playerInvisionRadius && playerInattackingRadius) AttackPlayer();

	}

	private void Idle()
	{
		zombieagent.SetDestination(transform.position);
        anime.SetBool("Idle", true);
        anime.SetBool("Running", false);
        

	}
	private void Pursueplayer()
	{
		if(zombieagent.SetDestination(playerBody.position))
		{
			//animations
            anime.SetBool("Idle", false);
            anime.SetBool("Running", true);
            anime.SetBool("Attacking", false);
			
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

				PlayerMotor playerBody = hitInfo.transform.GetComponent<PlayerMotor>();

				if(playerBody != null)
				{
					playerBody.playerHitDamage(giveDamage);
				}
		            anime.SetBool("Running", false);
                    anime.SetBool("Attacking", true);

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
			
			anime.SetBool("Died", true);
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
		Object.Destroy(gameObject, 5.0f);
	}
}
