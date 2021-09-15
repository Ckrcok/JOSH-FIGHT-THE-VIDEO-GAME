using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb2d;

    Animator anime;

    //attack variables
    public Transform attackArea;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;


    float horizontal, vertical, moveX,moveY;

    public bool damage;
    public bool attack;

    GameObject uiInfo;

    int timer;

    float speed = 2.0f;

    bool moving;

    // Start is called before the first frame update
    void Start()
    {
        damage = false;
        moving = false;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anime = gameObject.GetComponent<Animator>();
        uiInfo = GameObject.Find("Canvas");
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
        
        print(attack);

        if (Input.GetKey(KeyCode.LeftShift) && uiInfo.GetComponent<UIcontroller>().stamina > 0)
        {
            speed = 4.0f;
            uiInfo.GetComponent<UIcontroller>().stamina -= 0.03f;
            anime.speed = 1.5f;
        }
        else if (uiInfo.GetComponent<UIcontroller>().stamina <= 100)
        {
            speed = 2.0f;
            uiInfo.GetComponent<UIcontroller>().stamina += 0.2f;
            anime.speed = 1f;
        }
        else
        {
            speed = 2.0f;
            anime.speed = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            SlashAttack();
        }

        //if (Input.GetKey(KeyCode.Space))
        //{

        //    rb2d.AddForce(new Vector2(rb2d.velocity.x * 4, rb2d.velocity.y * 4));
        //    damage = true;
        //}



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




    }

    private void FixedUpdate()
    {
        Vector2 velVec = new Vector2(moveX, moveY).normalized;
        if (!damage)
        {
            rb2d.velocity = velVec * speed;
        }
        else if (damage)
        {
            timer++;
            if (timer is 15)
            {
                damage = false;
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
            print("hit" + enemy.name);
            print(transform.rotation.eulerAngles);
            enemy.GetComponent<EnemyMovement>().damaged = true;
            enemy.GetComponent<EnemyMovement>().health -= 10;
            if (transform.rotation.eulerAngles.y == 180)
            {
                print("rotated");
                enemy.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-300,0));
            }
            else if (transform.rotation.eulerAngles.y == 0)
            {
                print("rotated");
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
