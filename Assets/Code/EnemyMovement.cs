using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{


    public Transform player;
    public float moveSpeed = 5f;

    public float bufferDistance;

    public float health;

    public bool damaged;

    int timer;



    private Rigidbody2D rb;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        health = 100;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;

        if(health <= 0)
        {
            Destroy(gameObject);
        }

        transform.GetChild(0).transform.localScale = new Vector2 (health / 150,0.05f);
    }

    private void FixedUpdate()
    {
        //print(Vector2.Distance(player.position, transform.position));

        if (!damaged && Vector2.Distance(player.position, transform.position) > bufferDistance)
        {
            rb.velocity = new Vector2(0, 0);
            moveCharacter(movement);
        }

        else if (damaged)
        {
            timer++;
            if (timer is 10)
            {
                rb.velocity = new Vector2(0, 0);
                damaged = false;
                timer = 0;
            }
        }
    }

    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
}
