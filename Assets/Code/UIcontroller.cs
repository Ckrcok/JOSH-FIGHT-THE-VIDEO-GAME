using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{

    public Text healthUi, staminaUi, livesUi;
    public int health;
    public float stamina;
    public int lives;
    public int timer;
    public int killCount;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        stamina = 100;
        lives = 3;
        //   healthUi.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        healthUi.text = "Health: " + health + "/100";
        staminaUi.text = "Stamina: " + Mathf.Round(stamina) + "/100";
        livesUi.text = "Lives: " + lives;
        if (health <= 0)
        {
            if(lives == 0)
            {
                FindObjectOfType<GameManager>().EndGame();
            }
            else
            {
                timer++;
            }
            
            if(timer is 100)
            {
                timer = 0;
                health = 100;
                lives--;
            }
        }
    }
}
