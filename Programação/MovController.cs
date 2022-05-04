using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovController : MonoBehaviour
{
    //Rigidbody2D  rb;

    int velocidade;

    Vector2  dire;
    

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        velocidade = 2;

        dire = Vector2.zero;

    }

    // Update is called once per frame
    void Update()
    {
        InputPersonagem();
        transform.Translate(dire * velocidade * Time.deltaTime);
    }
    void InputPersonagem()
    {
        dire = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            dire += Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            dire += Vector2.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dire += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dire += Vector2.right;
        }
    }

}
