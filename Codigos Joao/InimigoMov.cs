using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoMov : MonoBehaviour
{

    public float velo;

    public float distancia;

    bool isRight = true;

    public Transform DetectaChao;

    public bool canMove = true;

    private bool recovering;

    private float recoveryCounter;

    [Header("Combate")]

    public int health;
    public float recoveryTime;
   


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * velo * Time.deltaTime);

        RaycastHit2D ground = Physics2D.Raycast(DetectaChao.position, Vector2.down, distancia);

        if (isRight == true)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            isRight = true;
        }

       //-------------------------combate

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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerController>().TakeDamage(2);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!recovering)
        {
            health -= damage;
            StartCoroutine("StopMove");

            recovering = true;

            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator StopMove()
    {
        canMove = false;

        yield return new WaitForSeconds(.5f);

        canMove = true;
    }

}

