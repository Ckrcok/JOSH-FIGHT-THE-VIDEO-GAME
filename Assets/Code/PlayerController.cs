using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb2d;

    Animator anime;

    GameObject audioController;

    public AudioClip footSteps, swing;
    //attack variables
    public Transform attackArea;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;


    float horizontal, vertical, moveX,moveY;

    public bool damage;
    public bool attack;


    GameObject uiInfo;

    int timer;



    float speed = 4.0f;

    bool moving;

    public bool dodging;

    public bool blocking;


    // Start is called before the first frame update
    void Start()
    {
        damage = false;
        moving = false;

        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anime = gameObject.GetComponent<Animator>();
        uiInfo = GameObject.Find("Canvas");
        audioController = FindObjectOfType<AudioController>().gameObject;
        //attackArea = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxis("Vertical");
        ////float velX = horizontal *10;
        //Vector2 vel = new Vector2(horizontal/2, vertical.).normalized;
        //rb2d.velocity = vel*speed;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,-10);
        anime.SetBool("Moving", moving);
//<<<<<<< HEAD
//=======
        
//        //print(attack);
//>>>>>>> 3cc007ca3db4a9bdf7b14f8ffb8f0bedef972a10


        if (uiInfo.GetComponent<UIcontroller>().stamina > 0f && Input.GetKey(KeyCode.LeftShift))
        {
            //print(uiInfo.GetComponent<UIcontroller>().stamina);
            speed = 6.0f;
            uiInfo.GetComponent<UIcontroller>().stamina -= 0.25f;
            anime.speed = 1.5f;

        }


        else if (uiInfo.GetComponent<UIcontroller>().stamina <= 100)
        {
            speed = 4.0f;
            uiInfo.GetComponent<UIcontroller>().stamina += 0.025f;
            anime.speed = 1f;
        }

        
        else
        {
            speed = 4.0f;
            anime.speed = 1f;
        }

        if (Input.GetMouseButtonDown(0)&&uiInfo.GetComponent<UIcontroller>().stamina >=10)
        {
            FindObjectOfType<AudioController>().Play("playerSwing");
            uiInfo.GetComponent<UIcontroller>().stamina -= 10;
            SlashAttack();
            
        }



        if (Input.GetKeyDown(KeyCode.Space)&& uiInfo.GetComponent<UIcontroller>().stamina >=20)
        {
            anime.SetTrigger("Dodge");
            uiInfo.GetComponent<UIcontroller>().stamina -= 20;
            rb2d.velocity=(new Vector2(rb2d.velocity.x * 6, rb2d.velocity.y * 6));
            damage = true;
        }

        if (Input.GetMouseButtonDown(1) && uiInfo.GetComponent<UIcontroller>().stamina >=30)
        {
            anime.SetTrigger("Block");
            blocking = true;
        }
        if (Input.GetMouseButton(1) && uiInfo.GetComponent<UIcontroller>().stamina >= 30)
        {
            anime.SetBool("Blocking", true);
            rb2d.velocity = new Vector2(0, 0);
        }
        else
        {
            anime.SetBool("Blocking", false);
            blocking = false;
        }


        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1;
            transform.eulerAngles = new Vector2(0, 0);
            

        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveX = -1;
            transform.eulerAngles = new Vector2(0, 180);
        }
        else
        {
            moveX = 0;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveY = -1;
        }
        else
        {
            moveY = 0;
        }

        if(rb2d.velocity.x >0 || rb2d.velocity.x < 0 || rb2d.velocity.y > 0 || rb2d.velocity.y < 0 )
        {
            moving = true;


        }
        else if(rb2d.velocity.x == 0 || rb2d.velocity.y == 0)
        {
            moving = false;
        }
        if (moving)
        {
            if (FindObjectOfType<AudioController>().gameObject.GetComponent<AudioSource>().clip.name != "playerWalk")
            {
               
                FindObjectOfType<AudioController>().StopPlaying("playerWalk");
            }
        }
        else
             FindObjectOfType<AudioController>().Play("playerWalk");

        if (uiInfo.GetComponent<UIcontroller>().health <= 0)
        {
            gameObject.transform.position = new Vector3(-12, 0, 2);
        }


    }

    private void FixedUpdate()
    {
        Vector2 velVec = new Vector2(moveX, moveY).normalized;
        if (!damage && !dodging && !blocking)
        {
            rb2d.velocity = velVec * speed;
        }
        else if (damage || dodging)
        {
            timer++;
            if (timer is 7)
            {
                dodging = false;
                damage = false;
                rb2d.velocity = new Vector2(0, 0);
                timer = 0;
            }
        }
    }

    public void Trap_Bounce()
    {
        for(int x = 0; x < 1000; x++)
        {
            rb2d.AddForce(new Vector2(-rb2d.velocity.x*2, -rb2d.velocity.y*2));
        }

    }

    void SlashAttack()
    {
        anime.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackArea.position, attackRange, enemyLayers);
        

        foreach(Collider2D enemy in hitEnemies)
        {
            print("hit: " + enemy.name);
            enemy.GetComponent<EnemyMovement>().beenHit = true;
            //print(transform.rotation.eulerAngles);
            enemy.GetComponent<EnemyMovement>().damaged = true;
            enemy.GetComponent<EnemyMovement>().health -= 10;
            if (transform.rotation.eulerAngles.y == 180)
            {
               // print("rotated");
                enemy.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-300,0));
            }
            else if (transform.rotation.eulerAngles.y == 0)
            {
               // print("rotated");
                enemy.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(300, 0));
            }

            //enemy.gameObject.GetComponent<Rigidbody2D>().AddForce
        }
        
    }


    private void OnDrawGizmosSelected()
    {
        if(attackArea == null)
            return;

        Gizmos.DrawWireSphere(attackArea.position, attackRange);
    }

}
