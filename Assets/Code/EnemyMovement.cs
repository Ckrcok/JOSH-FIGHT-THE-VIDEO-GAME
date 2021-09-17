using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{


    public Transform player;

    Transform attackArea;

    Animator anime;

    public float moveSpeed = 5f;


    public float attackRange = 5.5f;
    public float bufferDistance;

    public LayerMask playerLayer;

    float lastPos;

    public float health;

    public bool damaged;

    int timer;

    int attackTimer;

    bool canAttack;

    public bool hit;

    GameObject uiInfo;

    private Rigidbody2D rb;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        health = 100;
        canAttack = true;
        bufferDistance = 1.5f;
        

        player = GameObject.FindGameObjectWithTag("Player").transform;

        anime = gameObject.GetComponent<Animator>();

        attackArea = gameObject.GetComponentInChildren<Transform>();
        //anime.speed = 0.25f;

        uiInfo = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;
        if (rb.velocity.x>0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        if (rb.velocity.x<0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
       
        //print("Current: " + movement.x + "    Last: " + lastPos);



        if (health <= 0)
        {
            Destroy(gameObject);
        }

        transform.GetChild(0).transform.localScale = new Vector2 (health / 150,0.05f);
    }

    private void FixedUpdate()
    {
        //print(Vector2.Distance(player.position, transform.position));
        Collider2D[] playerInRange = Physics2D.OverlapCircleAll(attackArea.position, attackRange);

        if (!damaged && Vector2.Distance(player.position, transform.position) > bufferDistance)
        {
            //rb.velocity = new Vector2(0, 0);
            moveCharacter(movement);
        }

        else if(!damaged && Vector2.Distance(player.position, transform.position) < bufferDistance)
        {
            rb.velocity = new Vector2(0, 0);
            //moveCharacter(movement);
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
        foreach (Collider2D player in playerInRange)
        {
            if (player.CompareTag("Player") && canAttack)
            {
                anime.SetTrigger("Attack");
                
                canAttack = false;
            }
        }

        if (hit)
        {
            uiInfo.GetComponent<UIcontroller>().health -= 10;
            hit = false;
        }

        if (!canAttack)
        {
            //print(attackTimer);
            attackTimer++;
            if (attackTimer >= 100)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
    }

    void EnemyAttack()
    {

    }

    void moveCharacter(Vector2 direction)
    {
        //rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        rb.velocity=direction.normalized*3f;
    }


    private void OnDrawGizmosSelected()
    {
        if (attackArea == null)
            return;

        Gizmos.DrawWireSphere(attackArea.position, attackRange);
    }
}
