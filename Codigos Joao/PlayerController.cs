using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float recoveryCounter;

    public bool recovering;

    public bool canMove = true;

    public int facingDirection = 1;

    public int health;

    public float recoveryTime;

    private float attackTimer;


    [Header("Combate")]

    public LayerMask enemyLayers;

    public int attackDamage;

    public Transform attackHit;

    public float attackRange;

    public float attackCooldown; 
    

    // Update is called once per frame
    void Update()
    {
     
        CheckInput();

        if (recovering)
        {
            recoveryCounter += Time.deltaTime;
            if (recoveryCounter >= recoveryTime)
            {
                recoveryCounter = 0;
                recovering = false;
            }
        }
        
    }
    void Flip()
    {
        facingDirection *= -1;
    }

    public void TakeDamage(int damage)
    {
        if (!recovering)
        {
            Knockback();
            health -= damage;

            recovering = true;

            if (health <= 0)
            {
               Die();
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Knockback()
    {

        StartCoroutine("freeze");
    }


    // tirar controle personagem
    IEnumerator Freeza()
    {
        canMove = false;

        yield return new WaitForSeconds(.5f);

        canMove = true;
    }

    void Attack()
    {

        Collider2D[] targets = Physics2D.OverlapCircleAll(attackHit.position, attackRange, enemyLayers);

        foreach(Collider2D target in targets)
        {
            target.GetComponent<InimigoMov>().TakeDamage(attackDamage);

        }
    }

    void CheckInput()
    {
        if (attackTimer <= 0)
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }


} 
