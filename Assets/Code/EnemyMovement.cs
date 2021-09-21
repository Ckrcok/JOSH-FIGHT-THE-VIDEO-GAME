using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{


    public Transform player;

    Transform attackArea;

    Animator anime;

    AudioSource Audio;

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

    public bool dead;

    public bool hit;
    public bool beenHit;

    
    bool moving;

    GameObject uiInfo;

    private Rigidbody2D rb;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        health = 100;
        canAttack = true;
        bufferDistance = 1.5f;
        

        player = GameObject.FindGameObjectWithTag("Player").transform;

        anime = gameObject.GetComponent<Animator>();

        attackArea = gameObject.GetComponentInChildren<Transform>();
        //anime.speed = 0.25f;

        uiInfo = GameObject.Find("Canvas");
        attackArea.position = new Vector2(0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        
        anime.SetBool("Hitb", beenHit);
        anime.SetBool("Moving", moving);

        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;

       
        //print("Current: " + movement.x + "    Last: " + lastPos);



        if (health <= 0)
        {
            Destroy(gameObject);
            uiInfo.GetComponent<UIcontroller>().killCount++;
            Debug.Log(uiInfo.GetComponent<UIcontroller>().killCount);
            anime.SetTrigger("Death");
            FindObjectOfType<AudioController>().StopPlaying("enemySwing");
            FindObjectOfType<AudioController>().StopPlaying("enemyWalk");
            Destroy(gameObject);       

}

        if (rb.velocity.x >= 0.1 || rb.velocity.x <= -0.1 || rb.velocity.y >= 0.1 || rb.velocity.y <= -0.1)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        transform.GetChild(0).transform.localScale = new Vector2 (health / 150,0.05f);
        if (moving)
        {
           // if (FindObjectOfType<AudioController>().gameObject.GetComponent<AudioSource>().clip.name != "enemyWalk")
           // {

                FindObjectOfType<AudioController>().Play("enemyWalk");
            //}
        }
        else
            FindObjectOfType<AudioController>().StopPlaying("enemyWalk");
    }

    private void FixedUpdate()
    {
        anime.SetBool("Hitb", beenHit);
        //print(Vector2.Distance(player.position, transform.position));
        Collider2D[] playerInRange = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + 0.1f,transform.position.y -0.5f), attackRange);
        //if (player.gameObject.GetComponent<PlayerController>().eHit)
        //{
        //    anime.SetTrigger("Hit");
        //    player.gameObject.GetComponent<PlayerController>().eHit = false;
        //}
        //if (beenHit)
        //{

        //    anime.SetBool("Hitb", beenHit);
        //}

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
            if (timer is 5)
            {

                rb.velocity = new Vector2(0, 0);
                beenHit = false;
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
            Collider2D[] playerInRangeDoubleCheck = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + 0.1f, transform.position.y - 0.5f), attackRange);
            foreach (Collider2D player in playerInRangeDoubleCheck)
        {
            print(player.name);
            if (player.name == "playerChar" && !player.GetComponent<PlayerController>().blocking)
            {
                print("hit");
                uiInfo.GetComponent<UIcontroller>().health -= 10;
                FindObjectOfType<AudioController>().Play("enemySwing");
                hit = false;
                    FindObjectOfType<AudioController>().Play("attackHit");
                }
            else if (player.name != "playerChar")
            {
                print("miss");
                FindObjectOfType<AudioController>().Play("enemySwing");
                hit = false;

            }
                else if (player.name == "playerChar" && player.GetComponent<PlayerController>().blocking)
                {
                    print("blocked");

                    canAttack = true;
                    FindObjectOfType<AudioController>().Play("enemySwing");
                    uiInfo.GetComponent<UIcontroller>().stamina -= 30;
                    FindObjectOfType<AudioController>().Play("attackBlock");
                    hit = false;

                }

            }

        }

        //if (hit && Physics2D.OverlapCircle(attackArea.position, attackRange, layerMask: 8))
        //{

        //}
        //else if(hit && !Physics2D.OverlapCircle(attackArea.position, attackRange, layerMask: 8))
        //{
        //    FindObjectOfType<AudioController>().Play("enemySwing");
        //    hit = false;
        //}

        if (!canAttack)
        {
            //print(attackTimer);
            attackTimer++;
            if (attackTimer >= 50)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        if (rb.velocity.x > 0 && !damaged)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        if (rb.velocity.x < 0 && !damaged)
        {
            transform.eulerAngles = new Vector2(0, 180);
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

        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y - 0.5f), attackRange);
    }
}
