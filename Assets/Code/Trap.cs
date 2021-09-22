using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    GameObject script;
    Animator anime;
    //GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.Find("Canvas");
        anime = GetComponent<Animator>();
        //player = GameObject.Find("playerChar");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "playerChar")
        {
            anime.SetTrigger("Trapped");
            collision.GetComponent<PlayerController>().damage = true;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-collision.gameObject.GetComponent<Rigidbody2D>().velocity.x , -collision.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            script.GetComponent<UIcontroller>().health -= 10;
            //player.GetComponent<PlayerController>().Trap_Bounce();
        }
    }
}
