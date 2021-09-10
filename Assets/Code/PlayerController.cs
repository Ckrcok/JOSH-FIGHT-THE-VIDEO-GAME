using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb2d;
    float horizontal, vertical, moveX,moveY;

    public bool damage;

    GameObject uiInfo;

    int timer;

    float speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        damage = false;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
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
        //Camera.main.transform.position = new Vector3(0, Camera.main.transform.position.y/(transform.position.y*0.3f),-10);
        if (Input.GetKey(KeyCode.LeftShift) && uiInfo.GetComponent<UIcontroller>().stamina > 0)
        {
            speed = 4.0f;
            uiInfo.GetComponent<UIcontroller>().stamina -= 0.3f;
        }
        else if (uiInfo.GetComponent<UIcontroller>().stamina <= 100)
        {
            speed = 2.0f;
            uiInfo.GetComponent<UIcontroller>().stamina += 0.2f;
        }
        else
        {
            speed = 2.0f;
        }





        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveX = -1;
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
            if (timer is 25)
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
