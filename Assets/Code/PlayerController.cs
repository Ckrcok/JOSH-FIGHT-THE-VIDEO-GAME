using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb2d;

    Animator anime;


    float horizontal, vertical, moveX,moveY;

    public bool damage;

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

        if (Input.GetKey(KeyCode.LeftShift) && uiInfo.GetComponent<UIcontroller>().stamina > 0)
        {
            speed = 4.0f;
            uiInfo.GetComponent<UIcontroller>().stamina -= 0.3f;
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
}
