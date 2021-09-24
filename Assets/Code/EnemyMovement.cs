using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{


    public Transform player;

    Transform attackArea;

    Animator anime;

    AudioSource Audio;

    public float moveSpeed = 4f;


    public float attackRange = 1f;
    public float bufferDistance;

    public LayerMask playerLayer;

    float lastPos;

    //public GameObject healthPickup, staminaPickup;

    public GameObject[] pickups;


    public float health;

    public bool damaged;

    float timer;

    float attackTimer;

    bool canAttack;

    public bool dead = false;

    public bool hit;
    public bool beenHit;

    bool stepSound;


    bool moving;

    float deathCount;

    GameObject uiInfo;

    private Rigidbody2D rb;
    private Vector2 movement;

    Vector2 attackVec;

    // Start is called before the first frame update
    void Start()
    {
        deathCount = 0;
        rb = GetComponent<Rigidbody2D>();
        health = 100;
        canAttack = true;
        bufferDistance = 1f;
        //dead = false;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        anime = gameObject.GetComponent<Animator>();

        attackArea = gameObject.GetComponentInChildren<Transform>();
        //anime.speed = 0.25f;
       // Time.timeScale = 0.01f;
        uiInfo = GameObject.Find("Canvas");
        attackArea.position = new Vector2(0, -1);

    }

    // Update is called once per frame
    void Update()
    {
        attackVec = new Vector2(transform.position.x, transform.position.y - 0.5f);
        anime.SetBool("Hitb", beenHit);
        anime.SetBool("Moving", moving);

        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;


        //print("Current: " + movement.x + "    Last: " + lastPos);



        if (health <= 0 || deathCount is 31)
        {
            print("death");
            if(health <= 0)
            {
                anime.SetTrigger("Death");
            }
            // Destroy(gameObject);
            //Destroy(GetComponentInChildren<Transform>().gameObject);
            Debug.Log(uiInfo.GetComponent<UIcontroller>().killCount);

            FindObjectOfType<AudioController>().StopPlaying("enemySwing");
            FindObjectOfType<AudioController>().StopPlaying("enemyWalk");
            rb.velocity = new Vector2(0, 0);
            canAttack = false;
            //deathCount += Time.deltaTime;
            health = 0.01f;
            deathCount = 31;
            //if(deathCount >= 1f)
            //{
            //    dead = true;
            //}
            //'health = 100;
            //Destroy(gameObject);
        }
        if (dead)
        {
            print("death");
            uiInfo.GetComponent<UIcontroller>().killCount++;
            if (Random.Range(0, 2) == 1)
            {
                Instantiate(pickups[Random.Range(0, 2)],rb.position,transform.rotation,GameObject.Find("Pickups").transform);
            }
            
            //string[] pickups = { "healthPickup", "staminaPickup" };
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
            if (stepSound == true)
            {
                return;
            }
            else
            {
                stepSound = true;
                FindObjectOfType<AudioController>().Play("enemyWalk");
            }
        }
        else
            FindObjectOfType<AudioController>().StopPlaying("enemyWalk");
    }

    private void FixedUpdate()
    {
        anime.SetBool("Hitb", beenHit);
        //print(Vector2.Distance(player.position, transform.position));
        Collider2D[] playerInRange = Physics2D.OverlapCircleAll(attackVec, attackRange);
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
            timer+=Time.deltaTime;
            beenHit = false;
            if (timer >= 0.25f)
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
            Collider2D[] playerInRangeDoubleCheck = Physics2D.OverlapCircleAll(attackVec, attackRange);
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
            attackTimer+= Time.deltaTime;
            if (attackTimer >= 0.5f)
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

        Gizmos.DrawWireSphere(attackVec, attackRange);
    }
}
